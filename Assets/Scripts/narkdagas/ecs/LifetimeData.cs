using Unity.Entities;

namespace narkdagas.ecs {
    [GenerateAuthoringComponent]
    public struct LifetimeData : IComponentData {
        public float TimeLeft;
    }
}