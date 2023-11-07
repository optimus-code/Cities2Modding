using Colossal.IO;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ExampleMod.UI
{
    /// <summary>
    /// Helper class to handle texture requests from Unity to UI.
    /// </summary>
    public static class ImageHelper
    {
        private static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>( );

        public static void Add( Texture2D texture, string name )
        {
            if ( cache.ContainsKey( name ) )
                return;

            texture.name = name;

            if ( texture.format != TextureFormat.RGBA32 )
            {                
                cache.Add( name, ConvertToCoherentImage( texture ) );
            }
            else
                cache.Add( name, texture );
        }

        public static void AddOrUpdate( Texture2D texture, string name )
        {
            if ( cache.ContainsKey( name ) )
            {
                if ( texture.format != TextureFormat.RGBA32 )
                    cache[name] = ConvertToCoherentImage( texture );
                else
                    cache[name] = texture;
            }
            else
                Add( texture, name );
        }

        public static void Remove( string name )
        {
            if ( !cache.ContainsKey( name ) )
                return;

            cache.Remove( name );
        }

        public static Texture2D Get( string name )
        {
            if ( cache.ContainsKey( name ) )
                return cache[name];

            if ( TryLoadEmedded( name, out var texture ) )
            {
                Add( texture, name );
                return texture;
            }

            return null;
        }

        private static Texture2D ConvertToCoherentImage( Texture2D originalTexture )
        {
            // Create a new Texture2D with the desired format
            Texture2D convertedTexture = new Texture2D( originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false );

            // Copy the original pixels into the new texture
            convertedTexture.SetPixels( originalTexture.GetPixels( ) );
            convertedTexture.Apply( );

            return convertedTexture;
        }

        private static bool TryLoadEmedded( string fileName, out Texture2D texture )
        {
            texture = null;

            var assembly = Assembly.GetExecutingAssembly( );

            using ( var stream = assembly.GetManifestResourceStream( "ExampleMod.Resources." + fileName ) )
            {
                if ( stream == null )
                    return false;

                var buffer = stream.ReadAllBytes( );

                if ( buffer == null )
                    return false;

                texture = new Texture2D( 1, 1, TextureFormat.RGBA32, false );
                texture.LoadImage( buffer );
                texture.Apply( );
                return true;
            }
        }
    }
}
