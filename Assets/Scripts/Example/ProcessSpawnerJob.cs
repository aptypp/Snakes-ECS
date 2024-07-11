using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct ProcessSpawnerJob : IJobEntity
    {
        public float3 Position;
        public double ElapsedTime;
        public EntityCommandBuffer.ParallelWriter Ecb;

        private void Execute(
                [ChunkIndexInQuery] int chunkIndex,
                ref Spawner spawner)
        {
            if (spawner.NextSpawnTime > ElapsedTime) return;

            var newEntity = Ecb.Instantiate(
                chunkIndex,
                spawner.Prefab);

            Ecb.SetComponent(
                chunkIndex,
                newEntity,
                LocalTransform.FromPosition(Position));

            spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
        }
    }
}