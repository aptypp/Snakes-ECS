using Game.Components.Food;
using Unity.Entities;
using UnityEngine;

namespace Game.Authorings
{
    public class FoodSpawnerComponentAuthoring : MonoBehaviour
    {
        [field: SerializeField]
        public int FoodCount { get; private set; }

        [field: SerializeField]
        public GameObject FoodPrefab { get; private set; }

        public class FoodSpawnerComponentBaker : Baker<FoodSpawnerComponentAuthoring>
        {
            public override void Bake(
                FoodSpawnerComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(
                    entity,
                    new FoodSpawnerComponent
                    {
                        SpawnCount = authoring.FoodCount,
                        FoodPrefab = GetEntity(
                            authoring.FoodPrefab,
                            TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}