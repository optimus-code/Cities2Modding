using Game.Audio;
using Game.Prefabs;
using Game.Tools;
using Game;
using Unity.Entities;
using UnityEngine.InputSystem;
using Game.Rendering;
using System.Linq;
using cohtml.Net;
using System.Collections.Generic;

namespace ExampleMod.Systems
{
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

        private void ToggleAnarchy( )
        {
            var toolSystem = World.GetExistingSystemManaged<ToolSystem>( );
            toolSystem.ignoreErrors = !toolSystem.ignoreErrors;

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );
        }

        private void ToggleRoadLaneOverlay( )
        {
            ToggleShader( "BH/Decals/CurvedDecalDeteriorationShader" );

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );
        }

        private void ToggleFPSSaver( )
        {
            // Makes characters bald
            ToggleShader( "BH/Characters/SG_HairCardsDyed" );

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            AudioManager.instance.PlayUISound( soundQuery.GetSingleton<ToolUXSoundSettingsData>( ).m_TutorialStartedSound );
        }

        private void ToggleShader( string shaderName )
        {
            var renderSystem = World.GetExistingSystemManaged<RenderingSystem>( );
            var roadShader = renderSystem.enabledShaders.Where( e => e.Value && e.Key.name == shaderName ).FirstOrDefault( );

            var isEnabled = renderSystem.IsShaderEnabled( roadShader.Key );

            renderSystem.SetShaderEnabled( roadShader.Key, !isEnabled );
        }
    }
}
