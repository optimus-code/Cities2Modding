using ExampleMod.Systems;
using Game.Tools;
using HarmonyLib;

namespace ExampleMod.Patches
{
    /// <summary>
    /// Adds support for our custom toggle whiteness command
    /// to the tool system.
    /// </summary>
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
