using Game.Common;
using Game.Objects;
using Game.Simulation;
using Game.Tools;
using Game;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using HarmonyLib;

namespace ExampleMod.Systems
{
    /// <summary>
    /// Overrides tree growth system so trees skip to adult phase ASAP.
    /// The adult phase lasts as long as child-elder would last usually.
    /// </summary>
    [HarmonyPatch( typeof( TreeGrowthSystem ), "OnCreate" )]
    public class TreeGrowthSystem_OnCreatePatch
    {
        static bool Prefix( TreeGrowthSystem __instance )
        {
            __instance.World.GetOrCreateSystemManaged<TreeGrowthSystem_Custom>( );
            __instance.World.GetOrCreateSystemManaged<UpdateSystem>( ).UpdateAt<TreeGrowthSystem_Custom>( SystemUpdatePhase.GameSimulation );
            return false; // Ignore original function
        }
    }

    [HarmonyPatch( typeof( TreeGrowthSystem ), "OnUpdate" )]
    public class TreeGrowthSystem_OnUpdatePatch
    {
        static bool Prefix( TreeGrowthSystem __instance )
        {
            return false; // Ignore original function
        }
    }

    [HarmonyPatch( typeof( TreeGrowthSystem ), "OnCreateForCompiler" )]
    public class TreeGrowthSystem_OnCreateForCompilerPatch
    {
        static bool Prefix( TreeGrowthSystem __instance )
        {
            return false; // Ignore original function
        }
    }
    [CompilerGenerated]
    public class TreeGrowthSystem_Custom : GameSystemBase
    {
        public const int UPDATES_PER_DAY = 32;
        public const int TICK_SPEED_CHILD = 1280;
        public const int TICK_SPEED_TEEN = 938;
        //public const int TICK_SPEED_ADULT = 548;

        // Adults now live the total time that a teen-adult would live
        public const int TICK_SPEED_ADULT = TICK_SPEED_CHILD + TICK_SPEED_TEEN + 548;
        public const int TICK_SPEED_ELDERLY = 548;
        public const int TICK_SPEED_DEAD = 2304;

        private SimulationSystem m_SimulationSystem;
        private EndFrameBarrier m_EndFrameBarrier;
        private EntityQuery m_TreeQuery;
        private TreeGrowthSystem_Custom.TypeHandle __TypeHandle;

        public override int GetUpdateInterval( SystemUpdatePhase phase ) => 512;

        [UnityEngine.Scripting.Preserve]
        protected override void OnCreate( )
        {
            base.OnCreate( );
            
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>( );
            
            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>( );
            
            m_TreeQuery = GetEntityQuery( ComponentType.ReadWrite<Tree>( ), ComponentType.ReadOnly<UpdateFrame>( ), ComponentType.Exclude<Deleted>( ), ComponentType.Exclude<Overridden>( ), ComponentType.Exclude<Temp>( ) );
            
            RequireForUpdate( m_TreeQuery );
        }

        [UnityEngine.Scripting.Preserve]
        protected override void OnUpdate( )
        {            
            var updateFrame = SimulationUtils.GetUpdateFrame( m_SimulationSystem.frameIndex, 32, 16 );

            m_TreeQuery.ResetFilter( );            
            m_TreeQuery.SetSharedComponentFilter<UpdateFrame>( new UpdateFrame( updateFrame ) );

            __TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle.Update( ref CheckedStateRef );
            __TypeHandle.__Game_Common_Destroyed_RW_ComponentTypeHandle.Update( ref CheckedStateRef );
            __TypeHandle.__Game_Objects_Tree_RW_ComponentTypeHandle.Update( ref CheckedStateRef );
            __TypeHandle.__Unity_Entities_Entity_TypeHandle.Update( ref CheckedStateRef );

            var producerJob = new TreeGrowthSystem_Custom.TreeGrowthJob( )
            {
                m_EntityType = __TypeHandle.__Unity_Entities_Entity_TypeHandle,
                m_TreeType = __TypeHandle.__Game_Objects_Tree_RW_ComponentTypeHandle,
                m_DestroyedType = __TypeHandle.__Game_Common_Destroyed_RW_ComponentTypeHandle,
                m_DamagedType = __TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle,
                m_RandomSeed = RandomSeed.Next( ),
                m_CommandBuffer = m_EndFrameBarrier.CreateCommandBuffer( ).AsParallelWriter( )
            }.ScheduleParallel<TreeGrowthSystem_Custom.TreeGrowthJob>( m_TreeQuery, Dependency );
            
            m_EndFrameBarrier.AddJobHandleForProducer( producerJob );
            Dependency = producerJob;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private void __AssignQueries( ref SystemState state )
        {
        }

        protected override void OnCreateForCompiler( )
        {
            base.OnCreateForCompiler( );
            __AssignQueries( ref CheckedStateRef );
            __TypeHandle.__AssignHandles( ref CheckedStateRef );
        }

        [UnityEngine.Scripting.Preserve]
        public TreeGrowthSystem_Custom( )
        {
        }

        [BurstCompile]
        private struct TreeGrowthJob : IJobChunk
        {
            [ReadOnly]
            public EntityTypeHandle m_EntityType;
            public ComponentTypeHandle<Tree> m_TreeType;
            public ComponentTypeHandle<Destroyed> m_DestroyedType;
            public ComponentTypeHandle<Damaged> m_DamagedType;
            [ReadOnly]
            public RandomSeed m_RandomSeed;
            public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

            public void Execute(
              in ArchetypeChunk chunk,
              int unfilteredChunkIndex,
              bool useEnabledMask,
              in v128 chunkEnabledMask )
            {
                
                var nativeArray1 = chunk.GetNativeArray( m_EntityType );
                
                var nativeArray2 = chunk.GetNativeArray<Tree>( ref m_TreeType );
                
                var nativeArray3 = chunk.GetNativeArray<Destroyed>( ref m_DestroyedType );
                
                var nativeArray4 = chunk.GetNativeArray<Damaged>( ref m_DamagedType );
                
                var random = m_RandomSeed.GetRandom( unfilteredChunkIndex );
                if ( nativeArray3.Length != 0 )
                {
                    for ( var index = 0; index < nativeArray2.Length; ++index )
                    {
                        var tree = nativeArray2[index];
                        var destroyed = nativeArray3[index];
                        if ( TickTree( ref tree, ref destroyed, ref random ) )
                        {
                            var e = nativeArray1[index];
                            
                            m_CommandBuffer.AddComponent<BatchesUpdated>( unfilteredChunkIndex, e, new BatchesUpdated( ) );
                            
                            m_CommandBuffer.RemoveComponent<Destroyed>( unfilteredChunkIndex, e );
                            
                            m_CommandBuffer.RemoveComponent<Damaged>( unfilteredChunkIndex, e );
                        }
                        nativeArray2[index] = tree;
                        nativeArray3[index] = destroyed;
                    }
                }
                else if ( nativeArray4.Length != 0 )
                {
                    for ( var index = 0; index < nativeArray2.Length; ++index )
                    {
                        var tree = nativeArray2[index];
                        var damaged = nativeArray4[index];
                        bool stateChanged;
                        if ( TickTree( ref tree, ref damaged, ref random, out stateChanged ) )
                        {
                            var e = nativeArray1[index];
                            
                            m_CommandBuffer.AddComponent<BatchesUpdated>( unfilteredChunkIndex, e, new BatchesUpdated( ) );
                            
                            m_CommandBuffer.RemoveComponent<Damaged>( unfilteredChunkIndex, e );
                        }
                        if ( stateChanged )
                        {
                            
                            m_CommandBuffer.AddComponent<BatchesUpdated>( unfilteredChunkIndex, nativeArray1[index], new BatchesUpdated( ) );
                        }
                        nativeArray2[index] = tree;
                        nativeArray4[index] = damaged;
                    }
                }
                else
                {
                    for ( var index = 0; index < nativeArray2.Length; ++index )
                    {
                        var tree = nativeArray2[index];
                        if ( TickTree( ref tree, ref random ) )
                        {
                            
                            m_CommandBuffer.AddComponent<BatchesUpdated>( unfilteredChunkIndex, nativeArray1[index], new BatchesUpdated( ) );
                        }
                        nativeArray2[index] = tree;
                    }
                }
            }

            private bool TickTree( ref Tree tree, ref Random random )
            {
                switch ( tree.m_State & ( TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump ) )
                {
                    case TreeState.Teen:
                        TickTeen( ref tree, ref random );
                        return TickAdult( ref tree, ref random );
                    case TreeState.Adult:
                        return TickAdult( ref tree, ref random );
                    case TreeState.Elderly:
                        return TickElderly( ref tree, ref random );
                    case TreeState.Dead:
                    case TreeState.Stump:
                        return TickDead( ref tree, ref random );
                    default:
                        TickChild( ref tree, ref random );
                        TickTeen( ref tree, ref random );
                        return TickAdult( ref tree, ref random );
                }
            }

            private bool TickTree(
              ref Tree tree,
              ref Damaged damaged,
              ref Random random,
              out bool stateChanged )
            {
                switch ( tree.m_State & ( TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump ) )
                {
                    case TreeState.Elderly:
                        stateChanged = TickElderly( ref tree, ref random );
                        damaged.m_Damage -= random.NextFloat3( ( float3 ) 0.03137255f );
                        damaged.m_Damage = math.max( damaged.m_Damage, float3.zero );
                        return damaged.m_Damage.Equals( float3.zero );
                    case TreeState.Dead:
                    case TreeState.Stump:
                        stateChanged = TickDead( ref tree, ref random );
                        return stateChanged;
                    default:
                        stateChanged = false;
                        damaged.m_Damage -= random.NextFloat3( ( float3 ) 0.03137255f );
                        damaged.m_Damage = math.max( damaged.m_Damage, float3.zero );
                        return damaged.m_Damage.Equals( float3.zero );
                }
            }

            private bool TickTree( ref Tree tree, ref Destroyed destroyed, ref Random random )
            {
                destroyed.m_Cleared += random.NextFloat( 0.03137255f );
                if ( ( double ) destroyed.m_Cleared < 1.0 )
                    return false;
                tree.m_State &= ~( TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump );
                tree.m_Growth = ( byte ) 0;
                destroyed.m_Cleared = 1f;
                return true;
            }

            private bool TickChild( ref Tree tree, ref Random random )
            {
                //int num = ( int ) tree.m_Growth + ( random.NextInt( TICK_SPEED_CHILD ) >> 8 );
                //if ( num < 256 )
                //{
                //    tree.m_Growth = ( byte ) num;
                //    return false;
                //}

                // Skip growth and go to next stage
                tree.m_State |= TreeState.Teen;
                tree.m_Growth = ( byte ) 0;
                return true;
            }

            private bool TickTeen( ref Tree tree, ref Random random )
            {
                //int num = ( int ) tree.m_Growth + ( random.NextInt( TICK_SPEED_TEEN ) >> 8 );
                //if ( num < 256 )
                //{
                //    tree.m_Growth = ( byte ) num;
                //    return false;
                //}

                // Skip growth and go to next stage
                tree.m_State = tree.m_State & ~TreeState.Teen | TreeState.Adult;
                tree.m_Growth = ( byte ) 0;
                return true;
            }

            private bool TickAdult( ref Tree tree, ref Random random )
            {
                var num = ( int ) tree.m_Growth + ( random.NextInt( TICK_SPEED_ADULT ) >> 8 );
                if ( num < 256 )
                {
                    tree.m_Growth = ( byte ) num;
                    return false;
                }
                tree.m_State = tree.m_State & ~TreeState.Adult | TreeState.Elderly;
                tree.m_Growth = ( byte ) 0;
                return true;
            }

            private bool TickElderly( ref Tree tree, ref Random random )
            {
                var num = ( int ) tree.m_Growth + ( random.NextInt( TICK_SPEED_ELDERLY ) >> 8 );
                if ( num < 256 )
                {
                    tree.m_Growth = ( byte ) num;
                    return false;
                }
                tree.m_State = tree.m_State & ~TreeState.Elderly | TreeState.Dead;
                tree.m_Growth = ( byte ) 0;
                return true;
            }

            private bool TickDead( ref Tree tree, ref Random random )
            {
                var num = ( int ) tree.m_Growth + ( random.NextInt( TICK_SPEED_DEAD ) >> 8 );
                if ( num < 256 )
                {
                    tree.m_Growth = ( byte ) num;
                    return false;
                }
                tree.m_State &= ~( TreeState.Dead | TreeState.Stump );
                tree.m_Growth = ( byte ) 0;
                return true;
            }

            void IJobChunk.Execute(
              in ArchetypeChunk chunk,
              int unfilteredChunkIndex,
              bool useEnabledMask,
              in v128 chunkEnabledMask )
            {
                Execute( in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask );
            }
        }

        private struct TypeHandle
        {
            [ReadOnly]
            public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
            public ComponentTypeHandle<Tree> __Game_Objects_Tree_RW_ComponentTypeHandle;
            public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RW_ComponentTypeHandle;
            public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RW_ComponentTypeHandle;

            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void __AssignHandles( ref SystemState state )
            {                
                __Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle( );
                
                __Game_Objects_Tree_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>( );
                
                __Game_Common_Destroyed_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>( );
                
                __Game_Objects_Damaged_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>( );
            }
        }
    }
}
