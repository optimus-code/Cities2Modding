using Game.Audio;
using Game.Prefabs;
using Game.Tools;
using Game;
using Unity.Entities;
using UnityEngine.InputSystem;
using Game.Rendering;
using System.Linq;

namespace ExampleMod.Systems
{
    /// <summary>
    /// Just an example mod system with a few mods.
    /// </summary>
    public class ExampleModSystem : GameSystemBase
    {
        public bool ShowWhiteness
        {
            get;
            private set;
        } = true;

        private RenderingSystem renderSystem;

        protected override void OnCreate( )
        {
            base.OnCreate( );

            CreateKeyBinding( );

            World.GetOrCreateSystem<ImageOverlaySystem>( ); // Ensure image overlay system is instantiated

            renderSystem = World.GetExistingSystemManaged<RenderingSystem>( );

            ToggleRoadLaneOverlay( ); // Turn off by default
            ToggleFPSSaver( ); // Turn on by default

            UnityEngine.Debug.Log( "ExampleModSystem OnCreate" );
        }

        protected override void OnUpdate( )
        {
        }

        /// <summary>
        /// This is just a basic Unity Input setup, ideally we'd like to 
        /// integrate directly with the games native systems. Needs research.
        /// </summary>
        private void CreateKeyBinding( )
        {
            var inputAction = new InputAction( "ToggleWhiteness" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/ctrl" )
                .With( "Button", "<Keyboard>/w" );
            inputAction.performed += OnToggleWhiteness;
            inputAction.Enable( );

            inputAction = new InputAction( "AnarchyToggle" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/shift" )
                .With( "Button", "<Keyboard>/a" );
            inputAction.performed += ( a ) => ToggleAnarchy( );
            inputAction.Enable( );

            inputAction = new InputAction( "RoadLaneOverlayToggle" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/shift" )
                .With( "Button", "<Keyboard>/r" );
            inputAction.performed += ( a ) => ToggleRoadLaneOverlay( );
            inputAction.Enable( );

            inputAction = new InputAction( "ToggleFPSSaver" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/shift" )
                .With( "Button", "<Keyboard>/f" );
            inputAction.performed += ( a ) => ToggleFPSSaver( );
            inputAction.Enable( );
        }

        /// <summary>
        /// Toggle the infomode whiteness mode overlay. (CTRL+W)
        /// </summary>
        /// <remarks>
        /// (Has some issues that occur due to overriding, usually can be solved by 
        /// toggling it off and re-selecting the info mode desired.)
        /// </remarks>
        private void ToggleWhiteness( )
        {
            ShowWhiteness = !ShowWhiteness;

            UnityEngine.Shader.SetGlobalInt( "colossal_InfoviewOn", ShowWhiteness ? 1 : 0 );

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );

            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );

            UnityEngine.Debug.Log( "Show whiteness set to: " + ShowWhiteness );
        }

        private void OnToggleWhiteness( InputAction.CallbackContext obj )
        {
            ToggleWhiteness( );
        }

        /// <summary>
        /// Toggle the ignore validation errors option (SHIFT+A)
        /// </summary>
        private void ToggleAnarchy( )
        {
            var toolSystem = World.GetExistingSystemManaged<ToolSystem>( );
            toolSystem.ignoreErrors = !toolSystem.ignoreErrors;

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );
        }

        /// <summary>
        /// Toggle the white overlay for road lanes. (SHIFT+R)
        /// </summary>
        private void ToggleRoadLaneOverlay( )
        {
            ToggleShader( "BH/Decals/CurvedDecalDeteriorationShader" );

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );
        }

        /// <summary>
        /// Action for toggling some FPS saving utilities. (SHIFT+F)
        /// </summary>
        /// <remarks>
        /// (Can be expanded later.)
        /// </remarks>
        private void ToggleFPSSaver( )
        {
            // Makes characters bald
            ToggleShader( "BH/Characters/SG_HairCardsDyed" );

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );
        }

        /// <summary>
        /// Helper function for toggling a shader
        /// </summary>
        /// <param name="shaderName"></param>
        private void ToggleShader( string shaderName )
        {
            var renderSystem = World.GetExistingSystemManaged<RenderingSystem>( );
            var roadShader = renderSystem.enabledShaders.Where( e => e.Value && e.Key.name == shaderName ).FirstOrDefault( );

            var isEnabled = renderSystem.IsShaderEnabled( roadShader.Key );

            renderSystem.SetShaderEnabled( roadShader.Key, !isEnabled );
        }
    }
}
