using Unity.Entities;

namespace narkdagas.ecs {
    [GenerateAuthoringComponent]
     public struct EntityReferenceData : IComponentData {
        public Entity EntityRef;
        public uint Seed;
     }
}
