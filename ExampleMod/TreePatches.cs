using Game.Debug;
using Game.Objects;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using HarmonyLib;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace ExampleMod
{
    //
    // Modifies tree spawning so there are no longer Children and Teen trees
    // (Not sure if this is working properly.)
    //

    /// <summary>
    /// Make all trees adults unless they are too old and should die
    /// </summary>
    [HarmonyPatch( typeof( ObjectUtils ), "InitializeTreeState" )]
    class ObjectUtils_InitializeTreeStatePatch
    {
        const float ADULT_START_AGE = 0.251f;

        static bool Prefix( Tree __result, float age )
        {
            Tree tree = new Tree( );
            tree.m_State = age >= 0.95000004768371582 ? TreeState.Dead : TreeState.Adult;
            tree.m_Growth = ( byte ) UnityEngine.Mathf.Clamp( UnityEngine.Mathf.FloorToInt( ( float ) ( ( ( double ) ADULT_START_AGE - 0.25 ) / 0.34999999403953552 ) ), 0, ( int ) byte.MaxValue );
            return false;
        }
    }

    /// <summary>
    /// Modifies the tree spawn system to only spawn adults or dead trees
    /// </summary>
    [HarmonyPatch( typeof( TreeSpawnSystem ), "OnUpdate" )]
    class TreeSpawnSystem_OnUpdatePatch
    {
        static bool Prefix( TreeSpawnSystem __instance )
        {
            var loadGameSystem = Traverse.Create( __instance ).Field( "m_LoadGameSystem" ).GetValue<LoadGameSystem>( );
            var treeQuery = Traverse.Create( __instance ).Field( "m_TreeQuery" ).GetValue<EntityQuery>( );

            if ( loadGameSystem.context.purpose != Colossal.Serialization.Entities.Purpose.NewGame || !treeQuery.IsEmptyIgnoreFilter )
                return false;

            var terrainSystem = Traverse.Create( __instance ).Field( "m_TerrainSystem" ).GetValue<TerrainSystem>( );
            var prefabs = Traverse.Create( __instance ).Field( "m_Prefabs" ).GetValue<EntityQuery>( );

            Unity.Mathematics.Random random = new Unity.Mathematics.Random( ( uint ) DateTime.Now.Ticks );
            // ISSUE: reference to a compiler-generated method
            TerrainHeightData heightData = terrainSystem.GetHeightData( true );
            using ( NativeArray<Entity> entityArray = prefabs.ToEntityArray( ( AllocatorManager.AllocatorHandle ) Allocator.TempJob ) )
            {
                for ( int index = 0; index < 5000; ++index )
                {
                    Entity entity1 = entityArray[random.NextInt( entityArray.Length )];
                    EntityManager entityManager = __instance.EntityManager;
                    ObjectData componentData1 = entityManager.GetComponentData<ObjectData>( entity1 );
                    float2 float2 = random.NextFloat2( -1000f, 1000f );
                    Transform componentData2;
                    componentData2.m_Rotation = quaternion.RotateY( random.NextFloat( 6.28318548f ) );
                    componentData2.m_Position = new float3( float2.x, 0.0f, float2.y );
                    componentData2.m_Position.y = TerrainUtils.SampleHeight( ref heightData, componentData2.m_Position );
                    Tree componentData3;

                    /*switch ( random.NextInt( 13 ) )
                    {
                        case 2:
                        case 3:
                            componentData3.m_State = TreeState.Teen;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            componentData3.m_State = TreeState.Adult;
                            break;
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            componentData3.m_State = TreeState.Elderly;
                            break;
                        case 12:
                            componentData3.m_State = TreeState.Dead;
                            break;
                        default:
                            componentData3.m_State = ( TreeState ) 0;
                            break;
                    }*/
                    switch ( random.NextInt( 13 ) )
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            componentData3.m_State = TreeState.Adult;
                            break;
                        case 12:
                            componentData3.m_State = TreeState.Dead;
                            break;
                        default:
                            componentData3.m_State = ( TreeState ) 0;
                            break;
                    }

                    componentData3.m_Growth = ( byte ) random.NextInt( 256 );
                    entityManager = __instance.EntityManager;
                    Entity entity2 = entityManager.CreateEntity( componentData1.m_Archetype );
                    entityManager = __instance.EntityManager;
                    entityManager.SetComponentData<PrefabRef>( entity2, new PrefabRef( entity1 ) );
                    entityManager = __instance.EntityManager;
                    entityManager.SetComponentData<Transform>( entity2, componentData2 );
                    entityManager = __instance.EntityManager;
                    entityManager.SetComponentData<Tree>( entity2, componentData3 );
                }
            }

            return false;
        }
    }
}
