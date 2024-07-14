using Game.Components.Snake;
using Unity.Entities;
using UnityEngine;

namespace Game.Authorings
{
    public class SnakeSpawnerComponentAuthoring : MonoBehaviour
    {
        [field: SerializeField]
        public int SnakesCount { get; private set; }

        [field: SerializeField]
        public int TailsCount { get; private set; }

        [field: SerializeField]
        public float MoveSpeed { get; private set; }

        [field: SerializeField]
        public GameObject HeadPrefab { get; private set; }

        [field: SerializeField]
        public GameObject TailPrefab { get; private set; }

        public class SnakeSpawnerComponentBaker : Baker<SnakeSpawnerComponentAuthoring>
        {
            public override void Bake(
                SnakeSpawnerComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(
                    entity,
                    new SnakeSpawnerComponent
                    {
                        SpawnCount = authoring.SnakesCount,
                        TailsCount = authoring.TailsCount,
                        MoveSpeed = authoring.MoveSpeed,
                        HeadPrefab = GetEntity(
                            authoring.HeadPrefab,
                            TransformUsageFlags.Dynamic),
                        TailPrefab = GetEntity(
                            authoring.TailPrefab,
                            TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}