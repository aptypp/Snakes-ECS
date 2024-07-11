using Unity.Entities;

namespace Game
{
    namespace Bootstrap
    {
        public class WorldBootstrap : ICustomBootstrap
        {
            public bool Initialize(
                string defaultWorldName)
            {
                var world = new World(
                    defaultWorldName,
                    WorldFlags.Game);

                CreateSystems(world);

                World.DefaultGameObjectInjectionWorld = world;

                ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(world);

                return true;
            }

            private void CreateSystems(
                World world)
            {
                var simulationGroup = world.GetOrCreateSystemManaged<SimulationSystemGroup>();
                var presentationGroup = world.GetOrCreateSystemManaged<PresentationSystemGroup>();
                var initializationGroup = world.GetOrCreateSystemManaged<InitializationSystemGroup>();

                var testSystemHandle = world.CreateSystem<TestSystem>();

                simulationGroup.AddSystemToUpdateList(testSystemHandle);
            }
        }
    }
}