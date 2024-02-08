using Leopotam.EcsLite;
using Nazar.Components;
using StereoKit;

namespace Nazar.Systems;

class UISystem : IEcsRunSystem {
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var uiPool = world.GetPool<UIComponent>();

        foreach (var entity in world.Filter<UIComponent>().End()) {
            ref var ui = ref uiPool.Get(entity);

            if (UI.Button(ui.Label)) {
                ui.OnClick?.Invoke();
            }
        }
    }
}