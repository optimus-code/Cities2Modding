using ExampleMod.MonoBehaviours;
using ExampleMod.Systems;
using Game;
using Game.Audio;
using HarmonyLib;
using UnityEngine;

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

    /// <summary>
    /// Used to load songs before the map is loaded.
    /// </summary>
    /// <remarks>
    /// (Could be more robust, may encounter errors as is
    /// if loading is too slow.)
    /// </remarks>
    [HarmonyPatch( typeof( AudioManager ), "OnGameLoaded" )]
    class AudioManager_OnGameLoadedPatch
    {
        static void Postfix( Colossal.Serialization.Entities.Context serializationContext )
        {
            var musicLoader = new GameObject( "MusicLoader" );
            musicLoader.AddComponent<MusicLoader>( ); // Our custom music loader
        }
    }
}
