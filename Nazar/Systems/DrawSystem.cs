using Leopotam.EcsLite;
using Nazar.Components;

namespace Nazar.Systems;

class DrawSystem : IEcsRunSystem {
    public void Run(IEcsSystems systems) {
        var world = systems.GetWorld();
        var posePool = world.GetPool<PoseComponent>();
        var modelPool = world.GetPool<ModelComponent>();
        foreach (var entity in world.Filter<PoseComponent>().Inc<ModelComponent>().End()) {
            ref var pose = ref posePool.Get(entity);
            ref var model = ref modelPool.Get(entity);
            model.Value.Draw(pose.Value.ToMatrix());
        }
    }
}