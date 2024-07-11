using Unity.Entities;

namespace Game
{
    internal class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(
                SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(
                entity,
                new Spawner
                {
                    Prefab = GetEntity(
                        authoring.Prefab,
                        TransformUsageFlags.Dynamic),
                    SpawnPosition = authoring.transform.position,
                    NextSpawnTime = 0.0f,
                    SpawnRate = authoring.SpawnRate
                });
        }
    }
}