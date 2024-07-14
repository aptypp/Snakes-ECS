using Unity.Entities;

namespace Game.Components
{
    public struct SpawnSnakeTailComponent : IComponentData
    {
        public Entity Owner;
    }
}