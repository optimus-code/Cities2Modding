using BepInEx.Unity.Mono;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using Game.Simulation;
using Game.Audio;
using Game;

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
        static void Postfix( Colossal.Serialization.Entities.Purpose purpose, GameMode mode )
        {
            if ( !mode.IsGameOrEditor( ) )
                return;

            UnityEngine.Debug.Log( "Game loaded!" );
        }
    }
}
