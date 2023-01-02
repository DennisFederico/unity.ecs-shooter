using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace narkdagas.ecs {
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(StepPhysicsWorld))]
    [UpdateBefore(typeof(EndFramePhysicsSystem))]
    public partial class BulletCollisionEventSystem : SystemBase {
        private StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnCreate() {
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        struct CollisionEventImpulseJob : ICollisionEventsJob {

            [ReadOnly] public ComponentDataFromEntity<BulletData> BulletGroup;
            public ComponentDataFromEntity<VirusData> VirusGroup;
            
            public void Execute(CollisionEvent collisionEvent) {
                var entityA = collisionEvent.EntityA;
                var entityB = collisionEvent.EntityB;

                Debug.Log($"Query Collision {entityA} - {entityB}");
                
                if ((BulletGroup.HasComponent(entityA) && VirusGroup.HasComponent(entityB)) ||
                    (BulletGroup.HasComponent(entityB) && VirusGroup.HasComponent(entityA))) {
                    var (virusEntity, virusData) = VirusGroup.HasComponent(entityA) ? (entityA, VirusGroup[entityA]) : (entityB, VirusGroup[entityB]);
                    virusData.IsAlive = false;
                    VirusGroup[virusEntity] = virusData;
                }
            }
        }
        
        protected override void OnStartRunning() {
            base.OnStartRunning();
            this.RegisterPhysicsRuntimeSystemReadOnly();
        }

        protected override void OnUpdate() {
            Dependency = new CollisionEventImpulseJob() {
                BulletGroup = GetComponentDataFromEntity<BulletData>(true),
                VirusGroup = GetComponentDataFromEntity<VirusData>()
            }.Schedule(_stepPhysicsWorld.Simulation, Dependency);
            Dependency.Complete();
        }
    }
}
