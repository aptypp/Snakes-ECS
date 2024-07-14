using Unity.Entities;

namespace Game.Components.Snake
{
    public struct SpawnSnakeTailComponent : IComponentData
    {
        public Entity Owner;
    }
}