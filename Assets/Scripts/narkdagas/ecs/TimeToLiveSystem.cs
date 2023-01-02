using Unity.Entities;

namespace narkdagas.ecs {
    public partial class TimeToLiveSystem : SystemBase {
        
        private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        protected override void OnCreate() {
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            var entityCommandBuffer = _entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var timeDeltaTime = Time.DeltaTime;
            Entities.WithName("TimeToLiveSystem")
                .ForEach((Entity entity,
                    int entityInQueryIndex,
                    ref LifetimeData lifeTimeData) => {
                    lifeTimeData.TimeLeft -= timeDeltaTime;
                    if (lifeTimeData.TimeLeft <= 0) {
                        entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}