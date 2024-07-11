using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct Spawner : IComponentData
    {
        public float NextSpawnTime;
        public float SpawnRate;
        public float3 SpawnPosition;
        public Entity Prefab;
    }
}