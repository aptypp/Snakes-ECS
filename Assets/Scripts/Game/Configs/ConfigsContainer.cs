using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(
        menuName = "Game/Configs/" + nameof(ConfigsContainer),
        fileName = nameof(ConfigsContainer))]
    public class ConfigsContainer : ScriptableObject
    {
        [field: SerializeField]
        public FoodConfig FoodConfig { get; private set; }
    }
}