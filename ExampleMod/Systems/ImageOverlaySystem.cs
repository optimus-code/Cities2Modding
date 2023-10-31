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
            using ( Stream stream = assembly.GetManifestResourceStream( "ExampleMod.Resources.additiveshader" ) )
            {
                if ( stream == null )
                {
                    Debug.LogError( "Failed to load embedded resource." );
                    return null;
                }

                byte[] assetBytes = new byte[stream.Length];
                stream.Read( assetBytes, 0, assetBytes.Length );

                AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromMemory( assetBytes );
                if ( myLoadedAssetBundle == null )
                {
                    Debug.LogError( "Failed to load AssetBundle from memory." );
                    return null;
                }

                string[] assetNames = myLoadedAssetBundle.GetAllAssetNames( );
                foreach ( string name in assetNames )
                {
                    Debug.Log( name );
                }

                // Load an asset from the bundle
                Shader loadedShader = myLoadedAssetBundle.LoadAsset<Shader>( "assets/customoverlay.shader" );

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

            TerrainHeightData heightData = terrainSystem.GetHeightData( );
            WaterSurfaceData surfaceData = waterSystem.GetSurfaceData( out _ );
            return ( float ) WaterUtils.SampleHeight( ref surfaceData, ref heightData, Vector3.zero );
        }
    }

    // Left here for when/if we can find a way to get this working right so we can overlay it directly
    // on the terrain

    //[HarmonyPatch( typeof( TerrainMaterialSystem ), "UpdateMaterial" )]
    //class TerrainMaterialSystem_UpdateMaterialPatch
    //{
    //    static Material myMaterial;
    //    static Material original;

    //    static void Postfix( TerrainMaterialSystem __instance, Material material )
    //    {
    //        var exampleMod = __instance.World.GetExistingSystemManaged<ExampleModSystem>( );

    //        if ( exampleMod != null && exampleMod.ImageOverlay != null )
    //        {
    //            if ( exampleMod.ShowImageOverlay )
    //            {
    //                if ( myMaterial == null )
    //                {
    //                    original = new Material( material );

    //                    //material.shader = Shader.Find( "HDRP/Lit" );
    //                    material.mainTexture = exampleMod.ImageOverlay;
    //                    myMaterial = material;
    //                    //myMaterial = new Material( Shader.Find( "HDRP/Lit" ) );
    //                    //myMaterial.mainTexture = exampleMod.ImageOverlay;
    //                }
    //                else
    //                {
    //                    //material.shader = myMaterial.shader;
    //                    material.mainTexture = exampleMod.ImageOverlay;
    //                }
    //            }
    //            else if ( original != null )
    //            {
    //                //material.shader = original.shader;
    //                myMaterial = null;
    //            }
    //            //material.SetTexture( TerrainRenderSystem.ShaderID._BaseColorMap, exampleMod.ImageOverlay );

    //            //material.DisableKeyword( "OVERRIDE_OVERLAY_EXTRA" );
    //            //material.EnableKeyword( "OVERRIDE_OVERLAY_SIMPLE" );
    //        }
    //    }
    //}

    //[HarmonyPatch( typeof( TerrainRenderSystem ), "UpdateMaterial" )]
    //class TerrainRenderSystem_UpdateMaterialPatch
    //{
    //    static bool Prefix( TerrainRenderSystem __instance )
    //    {
    //        var terrainSystem = Traverse.Create( __instance ).Field( "m_TerrainSystem" ).GetValue<TerrainSystem>( );
    //        var terrainMaterialSystem = Traverse.Create( __instance ).Field( "m_TerrainMaterialSystem" ).GetValue<TerrainMaterialSystem>( );
    //        var overlayInfomodeSystem = Traverse.Create( __instance ).Field( "m_OverlayInfomodeSystem" ).GetValue<OverlayInfomodeSystem>( );
    //        var instanceMaterial = Traverse.Create( __instance ).Field( "material" ).GetValue<Material>( );
    //        var snowSystem = Traverse.Create( __instance ).Field( "m_SnowSystem" ).GetValue<SnowSystem>( );
    //        var setKeywords = typeof( TerrainRenderSystem ).GetMethod( "Setkeywords", BindingFlags.Instance | BindingFlags.NonPublic );

    //        // ISSUE: reference to a compiler-generated method
    //        overlayInfomodeSystem.ApplyOverlay( );
    //        TerrainSurface validSurface = TerrainSurface.GetValidSurface( );

    //        int baseLOD;
    //        float4x4 areas;
    //        float4 ranges;
    //        // ISSUE: reference to a compiler-generated method
    //        terrainSystem.GetCascadeInfo( out int _, out baseLOD, out areas, out ranges, out float4 _ );

    //        Shader.SetGlobalMatrix( TerrainRenderSystem.ShaderID._COTerrainTextureArrayLODArea, ( Matrix4x4 ) areas );
    //        Shader.SetGlobalVector( TerrainRenderSystem.ShaderID._COTerrainTextureArrayLODRange, ( Vector4 ) ranges );
    //        Shader.SetGlobalInt( TerrainRenderSystem.ShaderID._COTerrainTextureArrayBaseLod, baseLOD );
    //        Shader.SetGlobalVector( TerrainRenderSystem.ShaderID._COTerrainHeightScaleOffset, ( Vector4 ) new float4( terrainSystem.heightScaleOffset.x, terrainSystem.heightScaleOffset.y, 0.0f, 0.0f ) );

    //        if ( validSurface == null )
    //            return false;

    //        Material material = instanceMaterial == null ? validSurface.material : instanceMaterial;

    //        if ( material == null )
    //            return false;

    //        setKeywords.Invoke( __instance, new object[] { material } );
    //        material.SetMatrix( TerrainRenderSystem.ShaderID._LODArea, ( Matrix4x4 ) areas );
    //        material.SetVector( TerrainRenderSystem.ShaderID._LODRange, ( Vector4 ) ranges );
    //        material.SetVector( TerrainRenderSystem.ShaderID._TerrainScaleOffset, ( Vector4 ) new float4( terrainSystem.heightScaleOffset.x, terrainSystem.heightScaleOffset.y, 0.0f, 0.0f ) );
    //        material.SetVector( TerrainRenderSystem.ShaderID._VTScaleOffset, terrainSystem.VTScaleOffset );

    //        var heightmap = terrainSystem.heightmap;
    //        var overrideOverlaymap = __instance.overrideOverlaymap;
    //        var snowDepth = snowSystem.SnowDepth;
    //        // ISSUE: reference to a compiler-generated method
    //        var cascadeTexture = terrainSystem.GetCascadeTexture( );
    //        var splatmap = terrainMaterialSystem.splatmap;

    //        if ( heightmap != null )
    //            material.SetTexture( TerrainRenderSystem.ShaderID._HeightMap, heightmap );

    //        if ( splatmap != null )
    //            material.SetTexture( TerrainRenderSystem.ShaderID._SplatMap, splatmap );

    //        if ( cascadeTexture != null )
    //            material.SetTexture( TerrainRenderSystem.ShaderID._HeightMapArray, cascadeTexture );

    //        if ( overrideOverlaymap != null )
    //            material.SetTexture( TerrainRenderSystem.ShaderID._BaseColorMap, overrideOverlaymap );

    //        if ( __instance.overlayExtramap != null )
    //            material.SetTexture( TerrainRenderSystem.ShaderID._OverlayExtra, __instance.overlayExtramap );

    //        if ( snowDepth != null )
    //            material.SetTexture( TerrainRenderSystem.ShaderID._SnowMap, snowDepth );

    //        material.SetVector( TerrainRenderSystem.ShaderID._OverlayArrowMask, ( Vector4 ) __instance.overlayArrowMask );

    //        terrainMaterialSystem.UpdateMaterial( material );
    //        validSurface.material = material;

    //        return false;
    //    }
    //}
}
