using Unity.Entities;

namespace Game.Components.Food
{
    public struct FoodSpawnerComponent : IComponentData
    {
        public int SpawnCount;
        public Entity FoodPrefab;
    }
}