using Game.Bakers;
using Game.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct SnakeMovementSystem : ISystem
    {
        public void OnCreate(
            ref SystemState state)
        {
            state.RequireForUpdate<SnakeSpawnerComponent>();
        }

        [BurstCompile]
        public void OnUpdate(
            ref SystemState state)
        {
            var random = new Random(1239841);

            var snakeSpawnerComponent = SystemAPI.GetSingleton<SnakeSpawnerComponent>();

            using var foodPositions = new NativeList<float3>(Allocator.Temp);

            foreach (var data in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<FoodComponent>()
                         .WithEntityAccess())
            {
                foodPositions.Add(data.Item1.ValueRO.Position);
            }

            foreach (var snakeData in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<SnakeHeadComponent>()
                         .WithEntityAccess())
            {
                var headPosition = snakeData.Item1.ValueRO.Position;

                var minDistance = float.PositiveInfinity;
                var closestFoodPosition = float3.zero;

                foreach (var position in foodPositions)
                {
                    var distance = math.length(position - headPosition);

                    if (distance > minDistance) continue;

                    minDistance = distance;
                    closestFoodPosition = position;
                }

                var directionToFood =
                    math.normalize(closestFoodPosition - headPosition);

                var speed = snakeSpawnerComponent.MoveSpeed * random.NextFloat(
                    0.8f,
                    1.2f);

                snakeData.Item1.ValueRW.Position += directionToFood * speed * SystemAPI.Time.DeltaTime;

                var headComponent = SystemAPI.GetComponentRW<SnakeHeadComponent>(snakeData.Item2);

                for (var index = headComponent.ValueRW.Tails.Length - 1; index >= 0; index--)
                {
                    var tail = headComponent.ValueRW.Tails[index];
                    var tailTransform = SystemAPI.GetComponentRW<LocalTransform>(tail);

                    if (index == 0)
                    {
                        tailTransform.ValueRW.Position = math.lerp(
                            tailTransform.ValueRO.Position,
                            snakeData.Item1.ValueRO.Position,
                            speed * SystemAPI.Time.DeltaTime);

                        continue;
                    }

                    var nextTail = headComponent.ValueRW.Tails[index - 1];
                    var nextTailTransform = SystemAPI.GetComponentRW<LocalTransform>(nextTail);

                    tailTransform.ValueRW.Position = math.lerp(
                        tailTransform.ValueRO.Position,
                        nextTailTransform.ValueRO.Position,
                        speed * SystemAPI.Time.DeltaTime);
                }
            }
        }
    }
}