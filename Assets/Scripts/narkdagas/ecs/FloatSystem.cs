using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace narkdagas.ecs {
    public partial class FloatSystem : SystemBase {
        private static readonly Random Rnd = Random.CreateFromIndex(999);
        protected override void OnUpdate() {
            var timeDeltaTime = Time.DeltaTime;
            Entities.WithName("FloatSystem")
                .ForEach((ref PhysicsVelocity physics,
                    ref Translation position,
                    ref FloatData floatData) => {
                    float s = math.sin((timeDeltaTime + position.Value.x) * 0.5f) * floatData.LinearSpeed;
                    float c = math.cos((timeDeltaTime + position.Value.y) * 0.5f) * floatData.LinearSpeed;
                    float3 dir = new float3(s, c, s);
                    physics.Linear += dir;
                    physics.Angular = Rnd.NextFloat3Direction() * 2;
                }).ScheduleParallel();
            Dependency.Complete();
        }
    }
}