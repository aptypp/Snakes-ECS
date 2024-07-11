#if UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP
using Unity.Entities;
using UnityEngine;

namespace Game.Bootstrap
{
    public static class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            DefaultWorldInitialization.Initialize("Main");
        }
    }
}
#endif