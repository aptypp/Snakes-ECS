using Unity.Collections;
using Unity.Entities;

namespace Game.Components.Snake
{
    public struct SnakeHeadComponent : IComponentData
    {
        public NativeList<Entity> Tails;
    }
}