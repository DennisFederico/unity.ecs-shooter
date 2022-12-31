using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

namespace narkdagas.ecs {
    public partial class BulletSystem : SystemBase {
        protected override void OnUpdate() {
            var timeDeltaTime = Time.DeltaTime;
            Entities.WithName("BulletSystem")
                .ForEach((ref PhysicsVelocity physics,
                    ref Translation position,
                    ref Rotation rotation,
                    ref BulletData bulletData) => {
                    physics.Angular = float3.zero;
                    physics.Linear += timeDeltaTime * bulletData.Speed * math.forward(rotation.Value);
                }).ScheduleParallel();
        }
    }
}