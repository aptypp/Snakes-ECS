using Game.Components.Food;
using Game.Components.Snake;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems.Snake
{
    [BurstCompile]
    public partial struct SnakeSpawnerSystem : ISystem
    {
        private Random _random;

        [BurstCompile]
        public void OnCreate(
            ref SystemState state)
        {
            _random = new Random(15123);

            state.RequireForUpdate<FoodSpawnerComponent>();
            state.RequireForUpdate<SnakeSpawnerComponent>();

            CreateSpawnAllSnakesEntity(ref state);
        }

        [BurstCompile]
        public void OnUpdate(
            ref SystemState state)
        {
            var snakeSpawnerComponent = SystemAPI.GetSingleton<SnakeSpawnerComponent>();

            var spawnAllSnakesEntity = Entity.Null;

            foreach (var tuple in SystemAPI.Query<RefRO<SpawnAllSnakesComponent>>().WithEntityAccess())
            {
                spawnAllSnakesEntity = tuple.Item2;
            }

            if (spawnAllSnakesEntity != Entity.Null)
            {
                for (var snakeIndex = 0; snakeIndex < snakeSpawnerComponent.SpawnCount; snakeIndex++)
                {
                    CreateSpawnSnakeEntity(ref state);
                }

                state.EntityManager.DestroyEntity(spawnAllSnakesEntity);
            }

            using var spawnSnakeEntities = new NativeList<Entity>(Allocator.Temp);

            foreach (var spawnData in SystemAPI.Query<RefRO<SpawnSnakeComponent>>().WithEntityAccess())
            {
                spawnSnakeEntities.Add(spawnData.Item2);
            }

            foreach (var entity in spawnSnakeEntities)
            {
                SpawnSnake(
                    ref state,
                    ref snakeSpawnerComponent);
                state.EntityManager.DestroyEntity(entity);
            }

            using var spawnSnakeTails = new NativeList<(Entity HeadEntity, Entity SpawnEntity)>(Allocator.Temp);

            foreach (var data in SystemAPI.Query<RefRO<SpawnSnakeTailComponent>>().WithEntityAccess())
            {
                spawnSnakeTails.Add((data.Item1.ValueRO.Owner, data.Item2));
            }

            foreach (var data in spawnSnakeTails)
            {
                var snakeHeadComponent = state.EntityManager.GetComponentData<SnakeHeadComponent>(data.HeadEntity);

                var tailEntity = snakeHeadComponent.Tails[^1];

                var tailPosition = state.EntityManager.GetComponentData<LocalTransform>(tailEntity).Position;

                SpawnTail(
                    ref state,
                    tailPosition,
                    data.HeadEntity,
                    snakeSpawnerComponent.TailPrefab);

                state.EntityManager.DestroyEntity(data.SpawnEntity);
            }
        }

        [BurstCompile]
        public void OnDestroy(
            ref SystemState state)
        {
            foreach (var head in SystemAPI.Query<RefRW<SnakeHeadComponent>>())
            {
                head.ValueRW.Tails.Dispose();
            }
        }

        private void SpawnSnake(
            ref SystemState state,
            ref SnakeSpawnerComponent snakeSpawnerComponent)
        {
            var headEntity = state.EntityManager.Instantiate(snakeSpawnerComponent.HeadPrefab);

            var headTransform = new LocalTransform();

            headTransform.Position = _random.NextFloat3(
                -40,
                40);

            headTransform.Position.y = 0;

            headTransform.Scale = 1;

            state.EntityManager.SetComponentData(
                headEntity,
                headTransform);

            var headComponent = new SnakeHeadComponent();

            headComponent.Tails = new NativeList<Entity>(Allocator.Persistent);

            state.EntityManager.AddComponent<SnakeHeadComponent>(headEntity);

            state.EntityManager.SetComponentData(
                headEntity,
                headComponent);

            for (var tailIndex = 0; tailIndex < snakeSpawnerComponent.TailsCount; tailIndex++)
            {
                SpawnTail(
                    ref state,
                    headTransform.Position,
                    headEntity,
                    snakeSpawnerComponent.TailPrefab);
            }
        }

        private void SpawnTail(
            ref SystemState state,
            float3 position,
            Entity headEntity,
            Entity prefab)
        {
            var tail = state.EntityManager.Instantiate(prefab);

            var transform = new LocalTransform();

            transform.Scale = 1;
            transform.Position = position;

            state.EntityManager.SetComponentData(
                tail,
                transform);

            var snakeHeadComponent = state.EntityManager.GetComponentData<SnakeHeadComponent>(headEntity);

            snakeHeadComponent.Tails.Add(tail);

            state.EntityManager.SetComponentData(
                headEntity,
                snakeHeadComponent);
        }

        private void CreateSpawnAllSnakesEntity(
            ref SystemState state)
        {
            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponent<SpawnAllSnakesComponent>(entity);
        }

        private void CreateSpawnSnakeEntity(
            ref SystemState state)
        {
            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponent<SpawnSnakeComponent>(entity);
        }
    }
}