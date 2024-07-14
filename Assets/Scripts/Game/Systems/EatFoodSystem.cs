using Game.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct EatFoodSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(
            ref SystemState state)
        {
            using var availableFood = new NativeList<(float3 Position, Entity Entity)>(Allocator.Temp);
            using var ateFoodEntities = new NativeList<Entity>(Allocator.Temp);
            using var heads = new NativeList<(float3 Position, Entity Entity)>(Allocator.Temp);

            foreach (var data in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<FoodComponent>()
                         .WithEntityAccess())
            {
                availableFood.Add((data.Item1.ValueRO.Position, data.Item2));
            }

            foreach (var data in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<SnakeHeadComponent>()
                         .WithEntityAccess())
            {
                heads.Add((data.Item1.ValueRO.Position, data.Item2));
            }

            foreach (var headData in heads)
            {
                for (var foodIndex = availableFood.Length - 1; foodIndex >= 0; foodIndex--)
                {
                    var foodData = availableFood[foodIndex];

                    var distance = math.length(headData.Position - foodData.Position);

                    if (distance > .25f) continue;

                    state.EntityManager.DestroyEntity(foodData.Entity);

                    availableFood.RemoveAt(foodIndex);

                    CreateSpawnFoodEntity(
                        ref state);

                    CreateSpawnSnakeTailEntity(
                        ref state,
                        headData.Entity);
                }
            }
        }

        [BurstCompile]
        private void CreateSpawnFoodEntity(
            ref SystemState state)
        {
            var spawnFoodEntity = state.EntityManager.CreateEntity();

            state.EntityManager.AddComponent<SpawnFoodComponent>(spawnFoodEntity);
        }

        [BurstCompile]
        private void CreateSpawnSnakeTailEntity(
            ref SystemState state,
            Entity headEntity)
        {
            var spawnFoodEntity = state.EntityManager.CreateEntity();

            var spawnSnakeTailComponent = new SpawnSnakeTailComponent();

            spawnSnakeTailComponent.Owner = headEntity;

            state.EntityManager.AddComponent<SpawnSnakeTailComponent>(spawnFoodEntity);

            state.EntityManager.SetComponentData(
                spawnFoodEntity,
                spawnSnakeTailComponent);
        }
    }
}