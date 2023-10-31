using Game.Audio.Radio;
using HarmonyLib;
using static Game.Audio.Radio.Radio;

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
        static bool Prefix(ClipInfo clip, bool pushToFront)
        {
            if (clip.m_SegmentType != SegmentType.Playlist)
            {
                UnityEngine.Debug.Log("Skipped radio clip: " + clip.m_SegmentType.ToString());
                return false;
            }

            return true;
        }
    }
}
