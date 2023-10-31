using ExampleMod.Systems;
using Game.Audio;
using Game;
using HarmonyLib;

namespace ExampleMod.Patches
{
    /// <summary>
    /// An example patch
    /// </summary>
    /// <remarks>
    /// (So far the best way I've found to determine when the game AND map is fully loaded.)
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
}
