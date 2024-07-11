using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    [BurstCompile]
    public partial struct OptimizedSpawnerSystem : ISystem
    {
        private Random _random;

        public void OnCreate(
                ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            _random = new Random(
                (uint)UnityEngine.Random.Range(
                    0,
                    int.MaxValue / 2));
        }

        public void OnDestroy(
                ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(
                ref SystemState state)
        {
            var ecb = GetEntityCommandBuffer(ref state);

            var position = _random.NextFloat3(
                0,
                5);

            position.y = 0;

            new ProcessSpawnerJob
            {
                Position = position,
                ElapsedTime = SystemAPI.Time.ElapsedTime,
                Ecb = ecb
            }.ScheduleParallel();
        }

        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(
                ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            return ecb.AsParallelWriter();
        }
    }
}