using Game;
using cohtml.Net;
using Game.SceneFlow;
using System.IO;
using System.Reflection;
using System;
using System.Text;
using Game.Audio;
using Game.Prefabs;
using Unity.Entities;
using System.Collections.Generic;
using Game.Common;
using Game.Tools;
using Game.Rendering;

namespace ExampleMod.Systems
{
    // Note: The game uses React, this is not the 'proper' way to do it

    /// <summary>
    /// An example UI system
    /// </summary>
    /// <remarks>
    /// (Loads HTML, CSS and JS embedded resources and can execute JS.)<para/>
    /// Thanks to 89pleasure for finding out about the UI which can be seen at localhost:9444)
    /// <para/>(Does not do proper clean up.)
    /// </remarks>
    public class ExampleUISystem : GameSystemBase
    {
        private View uiView;
        private ToolUXSoundSettingsData soundQuery;
        private Dictionary<string, bool> windowVisibility = new Dictionary<string, bool>
        {
            { "custom-window", true },
            { "custom-window2", true },
        };

        private ImageOverlaySystem imageOverlaySystem;

        protected override void OnCreate( )
        {
            base.OnCreate( );
                        
            GetUIView( );

            SetupTestEventListener( );

            InjectBase64Function( );

            LoadEmbeddedCSS( "ui.css" );
            LoadEmbeddedHTML( "ui.html" );
            LoadEmbeddedJS( "ui.js" );

            soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) ).GetSingleton<ToolUXSoundSettingsData>( );

            imageOverlaySystem = World.GetExistingSystemManaged<ImageOverlaySystem>( );

            UnityEngine.Debug.Log( "ExampleUISystem OnCreate" );
        }

        protected override void OnUpdate( )
        {
        }

        /// <summary>
        /// Grab the GameFace view
        /// </summary>
        private void GetUIView( )
        {
            uiView = GameManager.instance.userInterface.view.View;
        }

        /// <summary>
        /// Execute Javascript on the main view
        /// </summary>
        /// <param name="javaScript"></param>
        public void ExecuteJS( string javaScript )
        {
            uiView?.ExecuteScript( javaScript );
        }

        /// <summary>
        /// Load HTML from an embedded resource in the assembly
        /// </summary>
        /// <param name="file"></param>
        public void LoadEmbeddedHTML( string file, string container = "main-container__E2" )
        {
            var html = LoadResourceText( "ExampleMod.Resources." + file );

            if ( html == null )
                return;

            // Create the JavaScript code
            var js = $"var div = document.createElement('div'); div.innerHTML = atob('{ToBase64( html )}'); document.body.appendChild(div);";

            ExecuteJS( js );

            UnityEngine.Debug.Log( "LoadEmbeddedHTML:" + file );
        }

        /// <summary>
        /// Load CSS from an embedded resource in the assembly
        /// </summary>
        /// <param name="file"></param>
        public void LoadEmbeddedCSS( string file )
        {
            var css = LoadResourceText( "ExampleMod.Resources." + file );

            if ( css == null )
                return;

            // Create the JavaScript code to append the CSS to the head
            var js = $"var style = document.createElement('style'); style.type = 'text/css'; style.innerHTML = atob('{ToBase64( css )}'); document.head.appendChild(style);";

            ExecuteJS( js );

            UnityEngine.Debug.Log( "LoadEmbeddedCSS:" + file );
        }

        /// <summary>
        /// Load CSS from a URL
        /// </summary>
        /// <param name="url">The URL of the CSS file</param>
        /// <param name="integrity"></param>
        /// <param name="crossorigin"></param>
        public void LoadCSSFromURL( string url, string integrity = null, string crossorigin = null )
        {
            // Create the JavaScript code to append the CSS link to the head with optional attributes
            var js = $"var link = document.createElement('link');" +
                     $"link.type = 'text/css';" +
                     $"link.rel = 'stylesheet';" +
                     $"link.href = '{url}';";

            if ( !string.IsNullOrEmpty( integrity ) )
            {
                js += $"link.integrity = '{integrity}';";
            }

            if ( !string.IsNullOrEmpty( crossorigin ) )
            {
                js += $"link.crossOrigin = '{crossorigin}';";
            }

            js += "document.head.appendChild(link);";

            ExecuteJS( js );

            UnityEngine.Debug.Log( "LoadCSSFromURL: " + url );
        }

        /// <summary>
        /// Load JavaScript from a URL
        /// </summary>
        /// <param name="url">The URL of the JavaScript file</param>
        /// <param name="integrity"></param>
        /// <param name="crossorigin"></param>
        public void LoadJSFromURL( string url, string integrity = null, string crossorigin = null )
        {
            // Create the JavaScript code to append the script to the body with optional attributes
            var js = $"var script = document.createElement('script');" +
                     $"script.type = 'text/javascript';" +
                     $"script.src = '{url}';";

            if ( !string.IsNullOrEmpty( integrity ) )
            {
                js += $"script.integrity = '{integrity}';";
            }

            if ( !string.IsNullOrEmpty( crossorigin ) )
            {
                js += $"script.crossOrigin = '{crossorigin}';";
            }

            js += "document.body.appendChild(script);";

            ExecuteJS( js );

            UnityEngine.Debug.Log( "LoadJSFromURL: " + url );
        }

        /// <summary>
        /// Load JS from an embedded resource in the assembly
        /// </summary>
        /// <param name="file"></param>
        public void LoadEmbeddedJS( string file )
        {
            var js = LoadResourceText( "ExampleMod.Resources." + file );

            if ( js == null )
                return;

            ExecuteJS( js );

            UnityEngine.Debug.Log( "LoadEmbeddedJS:" + file );
        }

        /// <summary>
        /// Helper function to load an embedded resource
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        private string LoadResourceText( string resourceName )
        {
            var assembly = Assembly.GetExecutingAssembly( );

            using ( var stream = assembly.GetManifestResourceStream( resourceName ) )
            using ( var reader = new StreamReader( stream ) )
            {
                return reader.ReadToEnd( );
            }
        }

        /// <summary>
        /// Injects a base64 decoder that is missing (atob)
        /// </summary>
        /// <remarks>
        /// (To safely escape the HTML and CSS this is the best way due to how we
        /// inject our custom UI)
        /// </remarks>
        private void InjectBase64Function()
        {
            var base64DecodeFunction = @"
                if (typeof atob !== 'function') {
                    function atob(str) {
                        // Going to use a lookup table to find characters.
                        var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
                        var output = '';

                        str = String(str).replace(/=+$/, ''); // Remove any trailing '='s

                        if (str.length % 4 == 1) {
                            throw new Error(""'atob' failed: The string to be decoded is not correctly encoded."");
                        }

                        for (
                            // initialize variables
                            var bc = 0, bs, buffer, idx = 0, output = '';
                            // get next character
                            buffer = str.charAt(idx++);
                            // character found in table? initialize bit storage and add its ascii value;
                            ~buffer && (bs = bc % 4 ? bs * 64 + buffer : buffer,
                                // and if not first of each 4 characters,
                                // convert the first 8 bits to one ascii character
                                bc++ % 4) ? output += String.fromCharCode(255 & bs >> (-2 * bc & 6)) : 0
                        ) {
                            buffer = chars.indexOf(buffer);
                        }

                        return output;
                    }                
                }";

            ExecuteJS( base64DecodeFunction );
        }

        /// <summary>
        /// Convert a string to base64
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ToBase64( string input )
        {
            return Convert.ToBase64String( Encoding.UTF8.GetBytes( input ) );
        }

        /// <summary>
        /// Helper function to base64 encode text and decode it on the JS side.
        /// </summary>
        /// <remarks>
        /// (To be executed in a JS string.)
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        private string JSFriendlyString( string text )
        {
            return "atob('" + ToBase64( text ) + "')";
        }

        /// <summary>
        /// Just play a sound
        /// </summary>
        private void PlaySound()
        {
            AudioManager.instance.PlayUISound( soundQuery.m_PolygonToolDropPointSound );
        }

        /// <summary>
        /// Setup JS event listener for our C# function
        /// </summary>
        private void SetupTestEventListener( )
        {
            uiView?.RegisterForEvent( "OnPotatoButtonClick", OnPotatoButtonClick );
            uiView?.RegisterForEvent( "OnExampleButtonClick", OnExampleButtonClick );
            uiView?.RegisterForEvent( "OnWindowCloseClick", OnWindowCloseClick );
            uiView?.RegisterForEvent( "OnReloadImageOverlayClick", OnReloadImageOverlayClick );
            UnityEngine.Debug.Log( "SetupTestEventListeners" );
        }

        /// <summary>
        /// A callback that is executed from JS
        /// </summary>
        /// <param name="text"></param>
        private void OnPotatoButtonClick( string text )
        {
            UnityEngine.Debug.Log( "We clicked the JS, comms from JS to C#! Value from JS: " + text );

            PlaySound( );
        }

        /// <summary>
        /// A callback for the example button
        /// </summary>
        private void OnExampleButtonClick( )
        {
            var text = $@"We changed something on click! The current time is: {DateTime.Now}.";

            // ToBase64 and Atob ensures it's escaped properly and you don't get errors
            ExecuteJS( @"document.getElementById('example-content').innerHTML = " + JSFriendlyString( text ) + ";" );

            PlaySound( );
        }

        /// <summary>
        /// Window close click event
        /// </summary>
        /// <param name="text"></param>
        private void OnWindowCloseClick( string elementID )
        {
            windowVisibility[elementID] = false;

            ExecuteJS( @"hideCustomWindow('" + elementID + "');" );
            PlaySound( );
        }

        /// <summary>
        /// Button was clicked to refresh image overlay
        /// </summary>
        private void OnReloadImageOverlayClick()
        {
            imageOverlaySystem.ReloadImage( );
            ExecuteJS( "setTimeout(function(){ reloadImageOverlaySource(); },1500);" );
            UnityEngine.Debug.Log( "Reloaded image overlay!!" );
        }

        public void ToggleUI( bool hidden )
        {
            var renderingSystem = World.GetOrCreateSystemManaged<RenderingSystem>( );
            renderingSystem.hideOverlay = hidden;
            Colossal.UI.UIManager.defaultUISystem.enabled = !hidden;

            if ( hidden )
            {
                World.GetExistingSystemManaged<ToolRaycastSystem>( ).raycastFlags |= RaycastFlags.FreeCameraDisable;
                World.GetExistingSystemManaged<ToolSystem>().activeTool = ( ToolBaseSystem ) World.GetExistingSystemManaged<DefaultToolSystem>( );
            }
            else
            {
                World.GetExistingSystemManaged<ToolRaycastSystem>( ).raycastFlags &= ~RaycastFlags.FreeCameraDisable;
            }
        }
    }
}
