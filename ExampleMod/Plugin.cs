using BepInEx.Unity.Mono;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using Game.Audio;
using Game;
using Game.SceneFlow;
using Game.Tools;
using UnityEngine.InputSystem;
using Game.Prefabs;
using Unity.Entities;

namespace ExampleMod
{
    [BepInPlugin( MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION )]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake( )
        {
            var harmony = Harmony.CreateAndPatchAll( Assembly.GetExecutingAssembly( ), MyPluginInfo.PLUGIN_GUID + "_Cities2Harmony" );
            var patchedMethods = harmony.GetPatchedMethods( ).ToArray( );

            // Plugin startup logic
            Logger.LogInfo( $"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded! Patched methods: " + patchedMethods.Length );

            foreach ( var patchedMethod in patchedMethods )
            {
                Logger.LogInfo( $"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}" );
            }
        }
    }
    
    /// <summary>
    /// An example patch
    /// </summary>
    /// <remarks>
    /// (So far the best way I've found to determine when the game is fully loaded.)
    /// </remarks>
    [HarmonyPatch( typeof( AudioManager ), "OnGameLoadingComplete" )]
    class AudioManager_OnGameLoadingCompletePatch
    {
        static void Postfix( AudioManager __instance, Colossal.Serialization.Entities.Purpose purpose, GameMode mode )
        {
            if ( !mode.IsGameOrEditor( ) )
                return;

            UnityEngine.Debug.Log( "Game loaded!" );

            __instance.World.GetOrCreateSystem<ExampleModSystem>( );
        }
    }

    /// <summary>
    /// Force enable developer mode
    /// </summary>
    /// <remarks>
    /// (Sometimes get intermittent crashes, not sure why or if it's related to this.)
    /// </remarks>
    [HarmonyPatch( typeof( GameManager ), "ParseOptions" )]
    class GameManager_ParseOptionsPatch
    {
        static void Postfix( GameManager __instance )
        {
            GameManager.Configuration configuration = __instance.configuration;

            if ( configuration != null )
            {
                configuration.developerMode = true;

                UnityEngine.Debug.Log( "Turned on Developer Mode! Press TAB for the dev/debug menu." );
            }
        }
    }

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

            UnityEngine.Debug.Log( "ExampleModSystem OnCreate" );
        }

        protected override void OnUpdate( )
        {
        }

        private void CreateKeyBinding()
        {
            var inputAction = new InputAction( "ToggleWhiteness" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/ctrl" )
                .With( "Button", "<Keyboard>/w" );
            inputAction.performed += OnToggleWhiteness;
            inputAction.Enable( );
        }

        private void ToggleWhiteness()
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
    }

    [HarmonyPatch( typeof( ToolSystem ), "UpdateInfoviewColors" )]
    class ToolSystem_UpdateInfoviewColorsPatch
    {
        static void Postfix( ToolSystem __instance )
        {
            var exampleModSystem = __instance.World.GetExistingSystemManaged<ExampleModSystem>( );

            if ( exampleModSystem == null )
                return;

            if ( __instance.activeInfoview != null )
                UnityEngine.Shader.SetGlobalInt( "colossal_InfoviewOn", exampleModSystem.ShowWhiteness ? 1 : 0 );
        }
    }
}
