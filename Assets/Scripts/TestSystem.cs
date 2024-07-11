using Unity.Entities;
using UnityEngine;

namespace Game.Bootstrap
{
    public partial struct TestSystem : ISystem
    {
        public void OnCreate(
            ref SystemState state)
        {
            Debug.Log("TestSystem Created");
        }

        public void OnUpdate(
            ref SystemState state)
        {
            Debug.Log("TestSystem Update");
        }
    }
}