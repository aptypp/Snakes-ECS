using Unity.Entities;

namespace Game.Components
{
    public struct FoodSpawnerComponent : IComponentData
    {
        public int SpawnCount;
        public Entity FoodPrefab;
    }
}