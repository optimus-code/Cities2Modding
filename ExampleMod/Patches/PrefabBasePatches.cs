using Game.Prefabs;
using HarmonyLib;

namespace ExampleMod.Patches
{
    /// <summary>
    /// Changes the empty zone colour to transparent rather than
    /// white.
    /// </summary>
    [HarmonyPatch( typeof( PrefabBase ), "OnEnable" )]
    class PrefabBase_OnEnablePatch
    {
        static void Postfix( PrefabBase __instance )
        {
            if ( __instance is ZonePrefab zonePrefab 
                && zonePrefab.m_AreaType == Game.Zones.AreaType.None )
            {
                zonePrefab.m_Color = new UnityEngine.Color( 1f, 1f, 1f, 0f );
            }
        }
    }
}
