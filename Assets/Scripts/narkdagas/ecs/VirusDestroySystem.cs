using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace narkdagas.ecs {
    public partial class VirusDestroySystem : SystemBase {
        private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;


        protected override void OnCreate() {
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            var entityCommandBuffer = _entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.WithName("VirusAliveSystem")
                .ForEach((Entity entity,
                    int entityInQueryIndex,
                    ref VirusData virusData,
                    ref EntityReferenceData entityRefData,
                    in Translation position) => {
                    if (!virusData.IsAlive) {
                        var random = Random.CreateFromIndex(entityRefData.Seed);
                        for (int count = 0; count < EcsManager.WhiteCellBurstCount; count++) {
                            var splat = entityCommandBuffer.Instantiate(entityInQueryIndex, entityRefData.EntityRef);
                            float3 offset = random.NextFloat3Direction() * 2f;
                            entityCommandBuffer.SetComponent(entityInQueryIndex, splat, new Translation { Value = position.Value + offset });
                            entityCommandBuffer.SetComponent(entityInQueryIndex, splat, new Rotation { Value = random.NextQuaternionRotation() });
                            float3 velocity = random.NextFloat3Direction() * random.NextInt(1, 5);
                            float3 angular = random.NextFloat3Direction() * random.NextFloat(0.5f, 1.5f);
                            entityCommandBuffer.SetComponent(entityInQueryIndex, splat, new PhysicsVelocity { Linear = velocity, Angular = angular});
                        }

                        entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}