using cohtml.Net;
using Colossal.UI;
using ExampleMod.UI;
using Game.UI;
using Game.UI.InGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleMod.Patches
{
    [HarmonyPatch( typeof( GameUIResourceHandler ), "OnResourceRequest" )]
    class GameUIResourceHandler_OnResourceRequestPatch
    {
        // We keep this because we need to unload old textures if they update
        private static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>( );

        static bool Prefix( GameUIResourceHandler __instance, IResourceRequest request, IResourceResponse response )
        {
            try
            {
                var url = request.GetURL( );

                if ( url?.ToLower().StartsWith( "examplemod://" ) == true )
                {
                    __instance.coroutineHost.StartCoroutine( LoadCustomResource( __instance, url, response ) );
                    Debug.Log( "Custom mod resource request: " + url );
                    return false; // Override default function it's one of our requests!
                }
            }
            catch ( Exception ex )
            {
                Debug.LogException( ex );
                response.Finish( ResourceResponse.Status.Failure );
            }
            return true; // Run default function
        }

        static System.Collections.IEnumerator LoadCustomResource( GameUIResourceHandler __instance, string url, IResourceResponse response )
        {
            var fileName = System.IO.Path.GetFileName( url );
            try
            {
                var texture = ImageHelper.Get( fileName );

                if ( !cache.ContainsKey( fileName ) )
                {
                    cache.Add( fileName, texture );
                }
                else // We need to tell it to update the existing reference
                {
                    var oldTexture = cache[fileName];
                    cache[fileName] = texture;

                    __instance.userImagesManager.UpdateNativePtr( oldTexture, texture );
                }

                response.ReceiveUserImage( __instance.userImagesManager.GetUserImageData( texture, UserImagesManager.ResourceType.Managed, false, new Rect( 0, 0, texture.width, texture.height ) ) );

                response.Finish( ResourceResponse.Status.Success );
            }
            catch
            {
                response.Finish( ResourceResponse.Status.Failure );
            }

            yield return null;
        }
    }
}
