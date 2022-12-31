using Unity.Entities;

namespace narkdagas.ecs {
    [GenerateAuthoringComponent]
    public struct BulletData : IComponentData {
        public float Speed;
    }
}
