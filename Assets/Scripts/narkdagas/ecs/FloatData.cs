using Unity.Entities;

namespace narkdagas.ecs {
    [GenerateAuthoringComponent]
    public struct FloatData : IComponentData {
        public float LinearSpeed;
        public float AngularSpeed;
    }
}