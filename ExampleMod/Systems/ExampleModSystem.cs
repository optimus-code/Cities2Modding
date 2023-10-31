using Game.Audio;
using Game.Prefabs;
using Game.Tools;
using Game;
using Unity.Entities;
using UnityEngine.InputSystem;

namespace ExampleMod.Systems
{
    public class ExampleModSystem : GameSystemBase
    {
        public bool ShowWhiteness
        {
            get;
            private set;
        } = true;

        protected override void OnCreate( )
        {
            base.OnCreate( );

            CreateKeyBinding( );

            World.GetOrCreateSystem<ImageOverlaySystem>( ); // Ensure image overlay system is instantiated

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
    }
}
