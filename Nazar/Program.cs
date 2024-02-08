using Leopotam.EcsLite;
using Nazar.Components;
using Nazar.Systems;
using StereoKit;

class Program 
{
    private static readonly EcsWorld World = new EcsWorld();
    private static readonly EcsSystems Systems = new EcsSystems(World);

    static void Main(string[] args) 
    {
        if (InitializeNazar())
            RunApplication();
    }

    static bool InitializeNazar()
    {
        if (InitializeStereoKit()) 
        {
            InitializeEcs();
            CreateEntities();
            return true;    
        }
        return false;
    }

    static bool InitializeStereoKit()
    {
        return SK.Initialize(new SKSettings 
        {
            appName = "Nazar",
            assetsFolder = "Assets",
        });
    }

    static void InitializeEcs() {
        Systems
            .Add(new HandleSystem())
            .Add(new DrawSystem())
            .Add(new AnimationSystem())
            .Add(new InteractionSystem())
            .Add(new UISystem())
            .Init();
    }

    static void CreateEntities() {
        var entity = World.NewEntity();
        ref var poseComponent = ref World.GetPool<PoseComponent>().Add(entity);
        poseComponent.Value = new Pose(0.2f, 0, -0.5f, Quat.Identity);
        ref var modelComponent = ref World.GetPool<ModelComponent>().Add(entity);
        modelComponent.Value = Model.FromMesh(Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f), Default.MaterialUI);
        ref var animationComponent = ref World.GetPool<AnimationComponent>().Add(entity);
        animationComponent = new AnimationComponent { Speed = 30f, Axis = Vec3.Up };

        var interactableEntity = World.NewEntity();
        ref var interactablePose = ref World.GetPool<PoseComponent>().Add(interactableEntity);
        interactablePose.Value = new Pose(-0.2f, 0, -0.5f, Quat.Identity);
        ref var interactableModel = ref World.GetPool<ModelComponent>().Add(interactableEntity);
        interactableModel.Value = Model.FromMesh(Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f), Default.Material);
        World.GetPool<InteractableComponent>().Add(interactableEntity); // No need to assign a value to a marker component

        var uiEntity = World.NewEntity();
        ref var uiComponent = ref World.GetPool<UIComponent>().Add(uiEntity);
        uiComponent = new UIComponent { Label = "Press Me!", OnClick = () => System.Console.WriteLine("Button Pressed!") };
        
        
    }
    
    static void RunApplication()
    {
        SK.Run(() => Systems.Run());
        Cleanup();
    }
    
    static void Cleanup()
    {
        Systems.Destroy();
        World.Destroy();
        SK.Shutdown();
    }
}