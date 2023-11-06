using ExampleMod.MonoBehaviours;
using Game.Audio.Radio;
using HarmonyLib;
using System.Diagnostics;
using UnityEngine;

namespace ExampleMod.Patches
{
    //
    // Disables all radio clips other than Playlist items (Music related)
    //

    [HarmonyPatch(typeof(Radio), "QueueEmergencyClips")]
    class Radio_QueueEmergencyClipsPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(Radio), "QueueEmergencyIntroClip")]
    class Radio_QueueEmergencyIntroClipPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(Radio), "QueueClip")]
    class Radio_QueueClipPatch
    {
        static bool Prefix(Radio.ClipInfo clip, bool pushToFront)
        {
            if (clip.m_SegmentType != Radio.SegmentType.Playlist)
            {
                UnityEngine.Debug.Log("Skipped radio clip: " + clip.m_SegmentType.ToString());
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Hook into songs that are played and replace them with ours :)
    /// </summary>
    /// <remarks>
    /// Looks for .ogg files located in {assemblyPath}\music
    /// (Could probably integrate natively and feed proper title info to
    /// the game and do it 'properly', this is a quick and dirty way of
    /// doing it bypassing CO's custom classes and architecture and 
    /// plugging in our own.)
    /// </remarks>
    [HarmonyPatch( typeof( Radio.RadioPlayer ), "Play" )]
    class RadioPlayer_PlayPatch
    {
        static MusicLoader musicLoader;

        static bool Prefix( Radio __instance, AudioClip clip, int timeSamples = 0 )
        {
            var m_AudioSource = Traverse.Create( __instance ).Field( "m_AudioSource" ).GetValue<AudioSource>( );

            if ( m_AudioSource == null )
                return false;

            if ( musicLoader == null )
                musicLoader = GameObject.Find( "MusicLoader" ).GetComponent<MusicLoader>();

            if ( musicLoader == null )
                m_AudioSource.clip = clip;
            else
                m_AudioSource.clip = musicLoader.GetRandomClip() ?? clip;

            m_AudioSource.timeSamples = timeSamples;
            m_AudioSource.Play( );

            Traverse.Create( __instance ).Field( "m_Elapsed" ).SetValue( GetAudioSourceTimeElapsed( __instance, m_AudioSource ) );

            var m_Timer = Traverse.Create( __instance ).Field( "m_Timer" ).GetValue<Stopwatch>( );
            m_Timer.Restart( );

            return false;
        }

        public static double GetAudioSourceTimeElapsed( Radio __instance, AudioSource audioSource )
        {
            var isCreated = Traverse.Create( __instance ).Field( "isCreated" ).GetValue<bool>( );
            return isCreated && audioSource.clip != null ? audioSource.timeSamples / ( double ) audioSource.clip.frequency : 0.0;
        }
    }
}
