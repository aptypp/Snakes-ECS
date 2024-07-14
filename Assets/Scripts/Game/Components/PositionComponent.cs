using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct PositionComponent : IComponentData
    {
        public float3 Position;
    }
}