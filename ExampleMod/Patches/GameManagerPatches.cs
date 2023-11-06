using Game.SceneFlow;
using HarmonyLib;

namespace ExampleMod.Patches
{
    /// <summary>
    /// Force enable developer mode
    /// </summary>
    /// <remarks>
    /// (This can bypass achievement so the unlock tool gives you achievements I think?)
    /// </remarks>
    [HarmonyPatch( typeof( GameManager ), "ParseOptions" )]
    class GameManager_ParseOptionsPatch
    {
        static void Postfix( GameManager __instance )
        {
            var configuration = __instance.configuration;

            if ( configuration != null )
            {
                configuration.developerMode = true;

                UnityEngine.Debug.Log( "Turned on Developer Mode! Press TAB for the dev/debug menu." );
            }
        }
    }
}
