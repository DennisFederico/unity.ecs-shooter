using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace narkdagas.ecs {
    public partial class TimeToLiveSystem : SystemBase {
        protected override void OnUpdate() {
            var timeDeltaTime = Time.DeltaTime;
            Entities.WithName("TimeToLiveSystem")
                .ForEach((Entity entity,
                    ref Translation position,
                    ref LifetimeData lifeTimeData) => {
                    lifeTimeData.Timeleft -= Time.DeltaTime;
                    if (lifeTimeData.Timeleft <= 0) {
                        EntityManager.DestroyEntity(entity);
                    }
                }).WithStructuralChanges()
                .Run();
               
        }
    }
}