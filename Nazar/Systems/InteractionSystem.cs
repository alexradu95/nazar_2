using Leopotam.EcsLite;
using Nazar.Components;
using StereoKit;

namespace Nazar.Systems;

class InteractionSystem : IEcsRunSystem {
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var posePool = world.GetPool<PoseComponent>();

        foreach (var entity in world.Filter<PoseComponent>().Inc<InteractableComponent>().End()) {
            ref var pose = ref posePool.Get(entity);

            if (UI.Handle("Interactable", ref pose.Value, new Bounds(Vec3.Zero, Vec3.One * 0.1f))) {
                System.Console.WriteLine("Interacted with entity!");
            }
        }
    }
}