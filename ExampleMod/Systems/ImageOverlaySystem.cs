using Game.Audio;
using Game.Prefabs;
using Game.Simulation;
using Game;
using System.IO;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.Mathematics;

namespace ExampleMod.Systems
{
    /// <summary>
    /// Overlays an image on world using a custom shader loaded via AssetBundle
    /// </summary>
    /// <remarks>
    /// Currently uses a flat plane, commented code at end of file is a possible 
    /// way to do it in the terrain material system but i could not get it 
    /// working properly, needs experimentation.
    /// </remarks>
    public class ImageOverlaySystem : GameSystemBase
    {        
        public bool ShowImageOverlay
        {
            get;
            private set;
        } = false;

        public Texture2D ImageOverlay
        {
            get
            {
                return overlayTexture;
            }
        }

        private Texture2D overlayTexture;
        private GameObject imageOverlayObj;
        private DateTime lastOverlayCheck;
        private Renderer renderer;
        private float transparency = 0.6f;
        private ToolUXSoundSettingsData soundSettings;

        protected override void OnCreate( )
        {
            base.OnCreate( );

            CreateKeyBinding( );
            InitialiseImageOverlay( );

            Debug.Log( "ImagerOverlaySystem OnCreate" );
        }

        protected override void OnUpdate( )
        {
        }

        private void CreateKeyBinding( )
        {
            var inputAction = new InputAction( "ToggleImageOverlay" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/ctrl" )
                .With( "Button", "<Keyboard>/i" );
            inputAction.performed += OnToggleImageOverlay;
            inputAction.Enable( );

            inputAction = new InputAction( "ImageOverlayHeightIncrease" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/ctrl" )
                .With( "Button", "<Keyboard>/equals" );
            inputAction.performed += OnImageOverlayIncreaseHeight;
            inputAction.Enable( );

            inputAction = new InputAction( "ImageOverlayHeightDecrease" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/ctrl" )
                .With( "Button", "<Keyboard>/minus" );
            inputAction.performed += OnImageOverlayDecreaseHeight;
            inputAction.Enable( );

            inputAction = new InputAction( "ImageOverlaySizeIncrease" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/shift" )
                .With( "Button", "<Keyboard>/equals" );
            inputAction.performed += OnImageOverlayIncreaseSize;
            inputAction.Enable( );

            inputAction = new InputAction( "ImageOverlaySizeDecrease" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/shift" )
                .With( "Button", "<Keyboard>/minus" );
            inputAction.performed += OnImageOverlayDecreaseSize;
            inputAction.Enable( );

            inputAction = new InputAction( "ImageOverlayTransparencyIncrease" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/alt" )
                .With( "Button", "<Keyboard>/equals" );
            inputAction.performed += OnImageOverlayIncreaseTransparency;
            inputAction.Enable( );

            inputAction = new InputAction( "ImageOverlayTransparencyDecrease" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/alt" )
                .With( "Button", "<Keyboard>/minus" );
            inputAction.performed += OnImageOverlayDecreaseTransparency;
            inputAction.Enable( );
        }

        private void InitialiseImageOverlay( )
        {
            lastOverlayCheck = DateTime.Now;

            var path = Path.GetDirectoryName( typeof( Plugin ).Assembly.Location ) + "\\overlay.png";

            if ( !File.Exists( path ) )
                return;

            overlayTexture = new Texture2D( 1, 1, TextureFormat.ARGB32, false );
            overlayTexture.LoadImage( File.ReadAllBytes( path ) );
            overlayTexture.Apply( );
        }

        private void CheckForOverlayChange()
        {
            var path = Path.GetDirectoryName( typeof( Plugin ).Assembly.Location ) + "\\overlay.png";

            var fileInfo = new FileInfo( path );

            if ( fileInfo.Exists && ( overlayTexture != null && fileInfo.LastWriteTime > lastOverlayCheck || overlayTexture == null ) )
            {
                InitialiseImageOverlay( );

                if ( overlayTexture != null && renderer != null )
                    renderer.material.mainTexture = overlayTexture;
            }
        }

        private void ToggleImageOverlay( )
        {
            CheckForOverlayChange( );

            if ( overlayTexture == null )
                return;

            ShowImageOverlay = !ShowImageOverlay;

            var soundQuery = GetEntityQuery( ComponentType.ReadOnly<ToolUXSoundSettingsData>( ) );
            soundSettings = soundQuery.GetSingleton<ToolUXSoundSettingsData>( );

            AudioManager.instance.PlayUISound( soundSettings.m_TutorialStartedSound );

            if ( imageOverlayObj == null )
            {
                // Create a new GameObject for Decal Projector
                imageOverlayObj = GameObject.CreatePrimitive( PrimitiveType.Plane );
                imageOverlayObj.name = "ImageOverlay";
                imageOverlayObj.transform.localScale = Vector3.one * 1435f;
                imageOverlayObj.transform.position = Vector3.zero + ( Vector3.up * SampleWaterHeight( ) );

                var shader = LoadAssetBundle( );

                if ( shader != null )
                {
                    // Create material for the Decal Projector
                    var mat = new Material( shader );
                    mat.enableInstancing = true;
                    mat.mainTexture = ImageOverlay;
                    mat.SetFloat( "_Transparency", transparency );

                    // Set the material to Decal Projector
                    renderer = imageOverlayObj.GetComponent<Renderer>( );
                    renderer.material = mat;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }

            imageOverlayObj.SetActive( ShowImageOverlay );
            Debug.Log( "Image overlay set to: " + ShowImageOverlay );
        }

        private void OnToggleImageOverlay( InputAction.CallbackContext obj )
        {
            ToggleImageOverlay( );
        }

        private void OnImageOverlayIncreaseHeight( InputAction.CallbackContext obj )
        {
            if ( imageOverlayObj == null )
                return;

            imageOverlayObj.transform.position += Vector3.up * 1f;
            AudioManager.instance.PlayUISound( soundSettings.m_TransportLineStartSound );
        }

        private void OnImageOverlayDecreaseHeight( InputAction.CallbackContext obj )
        {
            if ( imageOverlayObj == null )
                return;

            imageOverlayObj.transform.position -= Vector3.up * 1f;
            AudioManager.instance.PlayUISound( soundSettings.m_TransportLineCompleteSound );
        }

        private void OnImageOverlayIncreaseSize( InputAction.CallbackContext obj )
        {
            if ( imageOverlayObj == null )
                return;

            imageOverlayObj.transform.localScale += Vector3.one * 10f;
            AudioManager.instance.PlayUISound( soundSettings.m_PolygonToolDropPointSound );
        }

        private void OnImageOverlayDecreaseSize( InputAction.CallbackContext obj )
        {
            if ( imageOverlayObj == null )
                return;

            imageOverlayObj.transform.localScale -= Vector3.one * 10f;
            AudioManager.instance.PlayUISound( soundSettings.m_PolygonToolDropPointSound );
        }

        private void OnImageOverlayIncreaseTransparency( InputAction.CallbackContext obj )
        {
            if ( imageOverlayObj == null )
                return;

            transparency = Mathf.Clamp( transparency + 0.1f, 0f, 1f );
            renderer.material.SetFloat( "_Transparency", transparency );
            AudioManager.instance.PlayUISound( soundSettings.m_AreaMarqueeStartSound );
        }

        private void OnImageOverlayDecreaseTransparency( InputAction.CallbackContext obj )
        {
            if ( imageOverlayObj == null )
                return;

            transparency = Mathf.Clamp( transparency - 0.1f, 0f, 1f );
            renderer.material.SetFloat( "_Transparency", transparency );
            AudioManager.instance.PlayUISound( soundSettings.m_AreaMarqueeEndSound );
        }

        private Shader LoadAssetBundle( )
        {
            var assembly = Assembly.GetExecutingAssembly( );
            using ( var stream = assembly.GetManifestResourceStream( "ExampleMod.Resources.additiveshader" ) )
            {
                if ( stream == null )
                {
                    Debug.LogError( "Failed to load embedded resource." );
                    return null;
                }

                var assetBytes = new byte[stream.Length];
                stream.Read( assetBytes, 0, assetBytes.Length );

                var myLoadedAssetBundle = AssetBundle.LoadFromMemory( assetBytes );
                if ( myLoadedAssetBundle == null )
                {
                    Debug.LogError( "Failed to load AssetBundle from memory." );
                    return null;
                }

                var assetNames = myLoadedAssetBundle.GetAllAssetNames( );
                foreach ( var name in assetNames )
                {
                    Debug.Log( name );
                }

                // Load an asset from the bundle
                var loadedShader = myLoadedAssetBundle.LoadAsset<Shader>( "assets/customoverlay.shader" );

                if ( loadedShader == null )
                {
                    Debug.LogError( "Failed to load the customoverlay shader from the AssetBundle." );
                    return null;
                }
                myLoadedAssetBundle.Unload( false );
                return loadedShader;
            }
        }

        private float SampleWaterHeight( )
        {
            var waterSystem = World.GetExistingSystemManaged<WaterSystem>( );
            var terrainSystem = World.GetExistingSystemManaged<TerrainSystem>( );

            var heightData = terrainSystem.GetHeightData( );
            var surfaceData = waterSystem.GetSurfaceData( out _ );
            return ( float ) WaterUtils.SampleHeight( ref surfaceData, ref heightData, Vector3.zero );
        }
    }    
}
