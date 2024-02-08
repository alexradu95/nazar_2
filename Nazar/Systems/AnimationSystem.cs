using Leopotam.EcsLite;
using Nazar.Components;
using StereoKit;

namespace Nazar.Systems;

class AnimationSystem : IEcsRunSystem {
    public void Run(IEcsSystems systems) {
        var world = systems.GetWorld();
        var posePool = world.GetPool<PoseComponent>();
        var animPool = world.GetPool<AnimationComponent>();

        foreach (var entity in world.Filter<PoseComponent>().Inc<AnimationComponent>().End()) {
            ref var pose = ref posePool.Get(entity);
            ref var anim = ref animPool.Get(entity);
            
            pose.Value.orientation *= Quat.FromAngles(anim.Axis * (anim.Speed * Time.Stepf));
        }
    }
}