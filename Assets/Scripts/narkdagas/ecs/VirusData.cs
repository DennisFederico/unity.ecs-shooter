using Unity.Entities;

namespace narkdagas.ecs {
    
    [GenerateAuthoringComponent]
    public struct VirusData : IComponentData {
        public bool IsAlive;
    }
}
