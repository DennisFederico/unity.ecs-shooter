using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace narkdagas.ecs {
    public partial class FloatSystem : SystemBase {
        protected override void OnUpdate() {
            var timeDeltaTime = Time.DeltaTime;
            Entities.WithName("FloatSystem")
                .ForEach((ref PhysicsVelocity physics,
                    ref Translation position,
                    ref Rotation rotation,
                    ref FloatData floatData) => {
                    float s = math.sin((timeDeltaTime + position.Value.x) * 0.5f) * floatData.Speed;
                    float c = math.cos((timeDeltaTime + position.Value.y) * 0.5f) * floatData.Speed;
                    float3 dir = new float3(s, c, s);
                    physics.Linear += dir;
                }).ScheduleParallel();
        }
    }
}