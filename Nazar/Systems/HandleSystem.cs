using Leopotam.EcsLite;
using Nazar.Components;
using StereoKit;

namespace Nazar.Systems;

class HandleSystem : IEcsRunSystem {
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var posePool = world.GetPool<PoseComponent>();
        var modelPool = world.GetPool<ModelComponent>();

        foreach (var entity in world.Filter<PoseComponent>().Inc<ModelComponent>().End()) {
            ref var pose = ref posePool.Get(entity);
            ref var model = ref modelPool.Get(entity);

            UI.Handle("Cube", ref pose.Value, model.Value.Bounds);
        }
    }
}







