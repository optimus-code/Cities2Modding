using Colossal.IO.AssetDatabase;
using Game.Rendering;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExampleMod.Patches
{
    // This is a way for us to override the white lane overlay on roads
    // I haven't yet found a way to modify a colour, if we could figure
    // out how the original shader works, it's possible to remake it it
    // in the Unity Editor and add a parameter to control opacity/colour.
    // Need someone to spend some time playing with the shader currently
    // used to see about the viability of re-implementing it for customisation.

    //[HarmonyPatch( typeof( ManagedBatchSystem ), "CreateMaterial" )]
    //class ManagedBatchSystem_CreateMaterialPatch
    //{
    //    static void Prefix( ManagedBatchSystem __instance, SurfaceAsset sourceSurface, Material sourceMaterial,
    //        ManagedBatchSystem.MaterialKey materialKey )
    //    {
    //        if ( sourceSurface != null )
    //        {
    //            if ( materialKey.template.shader.name == "BH/Decals/CurvedDecalDeteriorationShader" )
    //            {
    //                foreach ( var tex in materialKey.template.GetTexturePropertyNames( ) )
    //                {
    //                    UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Texture Parameter: " + tex );
    //                }
    //                foreach ( var keyword in materialKey.template.enabledKeywords )
    //                {
    //                    UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Enabled Keyword: " + keyword.name );
    //                }
    //                foreach ( var propertyName in materialKey.template.GetPropertyNames( MaterialPropertyType.Float ) )
    //                {
    //                    UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Float Property: " + propertyName + " = " + materialKey.template.GetFloat( propertyName ) );
    //                }
    //                foreach ( var propertyName in materialKey.template.GetPropertyNames( MaterialPropertyType.Int ) )
    //                {
    //                    UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Int Property: " + propertyName + " = " + materialKey.template.GetInt( propertyName ) );
    //                }
    //                foreach ( var propertyName in materialKey.template.GetPropertyNames( MaterialPropertyType.Vector ) )
    //                {
    //                    UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Vector Property: " + propertyName + " = " + materialKey.template.GetVector( propertyName ) );
    //                }
    //                foreach ( var propertyName in materialKey.template.GetPropertyNames( MaterialPropertyType.Matrix ) )
    //                {
    //                    UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Matrix Property: " + propertyName + " = " + materialKey.template.GetMatrix( propertyName ) );
    //                }
    //                UnityEngine.Debug.Log( "CurvedDecalDeteriorationShader Render Queue: " + materialKey.template.renderQueue );
    //                //materialKey.template.shader = Shader.Find( "HDRP/Unlit" );
    //                UnityEngine.Debug.Log( "ManagedBatchSystem_CreateMaterial_SourceSurface: " + materialKey.template.name + " (" + materialKey.template.shader.name + ")" );

    //            }
    //            return;
    //        }
    //    }
    //}






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
