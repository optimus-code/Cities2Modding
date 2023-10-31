using BepInEx.Unity.Mono;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using ExampleMod.Patches;
using Game.Audio.Radio;
using static Game.Audio.Radio.Radio;
using Game.Prefabs;
using UnityEngine;
using Game.Areas;
using Game.Rendering.Debug;
using Game.UI.Tooltip;
using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game.Rendering;
using static Colossal.AssetPipeline.Constants.Material;
using Unity.Burst;

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
}
