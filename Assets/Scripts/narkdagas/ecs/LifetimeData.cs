using Unity.Entities;
using UnityEngine;

namespace narkdagas.ecs {
    [GenerateAuthoringComponent]
    public struct LifetimeData : IComponentData {
        public float Timeleft;
    }
}