using Unity.Entities;

namespace Game.Components.Snake
{
    public struct SnakeSpawnerComponent : IComponentData
    {
        public int SpawnCount;
        public int TailsCount;
        public float MoveSpeed;
        public Entity HeadPrefab;
        public Entity TailPrefab;
    }
}