using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ExampleMod.MonoBehaviours
{
    /// <summary>
    /// A custom .ogg music loader
    /// </summary>
    /// <remarks>
    /// (Looks in {assemblyPath}\music for .ogg files, loads async.)
    /// </remarks>
    public class MusicLoader : MonoBehaviour
    {
        private List<AudioClip> audioClips = new List<AudioClip>( );

        private void Start( )
        {
            DontDestroyOnLoad( this );

            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly( ).Location;
            var directoryPath = Path.GetDirectoryName( assemblyLocation );
            var musicFolderPath = Path.Combine( directoryPath, "music" );

            LoadAllAudioClips( musicFolderPath );
        }

        private void LoadAllAudioClips( string path )
        {
            var oggFiles = Directory.GetFiles( path, "*.ogg" );

            foreach ( var oggFile in oggFiles )
            {
                StartCoroutine( LoadAudioClip( oggFile ) );
            }
        }

        private IEnumerator LoadAudioClip( string filePath )
        {
            var url = "file://" + filePath;
            using ( var www = new WWW( url ) )
            {
                yield return www;
                var clip = www.GetAudioClip( false, true, AudioType.OGGVORBIS );
                if ( clip != null )
                {
                    audioClips.Add( clip );
                    Debug.Log( "Loaded audio clip: " + clip.name );
                }
                else
                {
                    Debug.LogError( "Failed to load AudioClip from: " + filePath );
                }
            }
        }

        public AudioClip GetRandomClip( )
        {
            if ( audioClips.Count > 0 )
            {
                var randomIndex = Random.Range( 0, audioClips.Count );
                return audioClips[randomIndex];
            }

            return null;
        }
    }
}
