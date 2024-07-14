using Game.Components.Food;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems.Food
{
    [BurstCompile]
    public partial struct FoodSpawnerSystem : ISystem
    {
        private Random _random;

        [BurstCompile]
        public void OnCreate(
            ref SystemState state)
        {
            _random = new Random(3124);

            state.RequireForUpdate<FoodSpawnerComponent>();

            CreateSpawnAllFoodEntity(ref state);
        }

        [BurstCompile]
        public void OnUpdate(
            ref SystemState state)
        {
            var foodSpawnerComponent = SystemAPI.GetSingleton<FoodSpawnerComponent>();

            var spawnAllFoodEntity = Entity.Null;

            foreach (var data in SystemAPI.Query<RefRO<SpawnAllFoodComponent>>().WithEntityAccess())
            {
                spawnAllFoodEntity = data.Item2;
            }

            if (spawnAllFoodEntity != Entity.Null)
            {
                for (var foodIndex = 0; foodIndex < foodSpawnerComponent.SpawnCount; foodIndex++)
                {
                    CreateSpawnFoodEntity(ref state);
                    state.EntityManager.DestroyEntity(spawnAllFoodEntity);
                }
            }

            var spawnFoodEntities = new NativeList<Entity>(Allocator.Temp);

            foreach (var data in SystemAPI.Query<RefRO<SpawnFoodComponent>>().WithEntityAccess())
            {
                spawnFoodEntities.Add(data.Item2);
            }

            foreach (var entity in spawnFoodEntities)
            {
                SpawnFood(
                    ref state,
                    ref foodSpawnerComponent);
                state.EntityManager.DestroyEntity(entity);
            }

            spawnFoodEntities.Dispose();
        }

        private void SpawnFood(
            ref SystemState state,
            ref FoodSpawnerComponent foodSpawnerComponent)
        {
            var food = state.EntityManager.Instantiate(foodSpawnerComponent.FoodPrefab);

            var localTransform = state.EntityManager.GetComponentData<LocalTransform>(food);

            localTransform.Position = _random.NextFloat3(
                -45,
                45);

            localTransform.Position.y = 0;

            state.EntityManager.SetComponentData(
                food,
                localTransform);

            state.EntityManager.AddComponent<FoodComponent>(food);
        }

        private void CreateSpawnFoodEntity(
            ref SystemState state)
        {
            var entity = state.EntityManager.CreateEntity();

            state.EntityManager.AddComponent<SpawnFoodComponent>(entity);
        }

        private void CreateSpawnAllFoodEntity(
            ref SystemState state)
        {
            var entity = state.EntityManager.CreateEntity();

            state.EntityManager.AddComponent<SpawnAllFoodComponent>(entity);
        }
    }
}