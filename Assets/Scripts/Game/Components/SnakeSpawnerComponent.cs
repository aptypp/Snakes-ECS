using Unity.Entities;

namespace Game.Bakers
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