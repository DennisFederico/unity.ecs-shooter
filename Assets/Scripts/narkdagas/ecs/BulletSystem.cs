using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace narkdagas.ecs {
    public partial class BulletSystem : SystemBase {
        protected override void OnUpdate() {
            var timeDeltaTime = Time.DeltaTime;
            Entities.WithName("BulletSystem")
                .ForEach((ref PhysicsVelocity physics,
                    ref Rotation rotation,
                    ref BulletData bulletData) => {
                    physics.Angular = float3.zero;
                    physics.Linear += timeDeltaTime * bulletData.Speed * math.forward(rotation.Value);
                }).ScheduleParallel();
        }
    }
}