using Unity.Entities;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(
        menuName = "Game/Configs/" + nameof(FoodConfig),
        fileName = nameof(FoodConfig))]
    public class FoodConfig : ScriptableObject
    {
        [field: SerializeField]
        public Vector3 MinPosition { get; private set; }

        [field: SerializeField]
        public Vector3 MaxPosition { get; private set; }

        [field: SerializeField]
        public GameObject Prefab { get; private set; }
    }
}