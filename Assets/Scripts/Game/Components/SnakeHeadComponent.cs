using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct SnakeHeadComponent : IComponentData
    {
        public NativeList<Entity> Tails;
    }
}