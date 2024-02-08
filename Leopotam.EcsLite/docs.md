Creating a comprehensive markdown documentation for LeoEcsLite, including analysis-based examples, requires a detailed look into the framework's capabilities and the provided code snippets. Below is an enhanced markdown guide that incorporates the original examples from the README and introduces new, hypothetical examples to illustrate potential uses of the framework.

# LeoEcsLite: Lightweight C# ECS Framework Documentation

LeoEcsLite is a high-performance, minimal allocation Entity Component System (ECS) framework designed for C#. It focuses on performance, minimal memory usage, and has no dependencies on any game engine, making it suitable for a wide range of applications.

## Table of Contents

- [Key Concepts](#key-concepts)
    - [Entity](#entity)
    - [Component](#component)
    - [System](#system)
    - [EcsWorld](#ecsworld)
    - [EcsPool](#ecspool)
    - [EcsFilter](#ecsfilter)
    - [EcsSystems](#ecssystems)
- [Integration with Game Engines](#integration-with-game-engines)
    - [Unity](#unity)
    - [Custom Engines](#custom-engines)
- [Examples](#examples)
    - [Basic Setup](#basic-setup)
    - [Advanced Usage](#advanced-usage)
- [License](#license)

## Key Concepts

### Entity

An entity is essentially a container for components, represented as an `int`. It does not hold any data itself but acts as an identifier for accessing components.

```csharp
// Creating a new entity in the world.
int entity = world.NewEntity();

// Deleting an entity, automatically removing all its components first.
world.DelEntity(entity);

// Copying components from one entity to another.
world.CopyEntity(srcEntity, dstEntity);
```

### Component

Components are plain data containers used to store the state of an entity. They should not contain any logic.

```csharp
struct Component1 {
    public int Id;
    public string Name;
}
```

### System

Systems contain the logic to process entities that have specific components. They can implement various interfaces to hook into different parts of the ECS lifecycle.

```csharp
class UserSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsDestroySystem, IEcsPostDestroySystem {
    public void PreInit(IEcsSystems systems) { }
    public void Init(IEcsSystems systems) { }
    public void Run(IEcsSystems systems) { }
    public void PostRun(IEcsSystems systems) { }
    public void Destroy(IEcsSystems systems) { }
    public void PostDestroy(IEcsSystems systems) { }
}
```

### EcsWorld

`EcsWorld` is the central container for all entities, component pools, and filters.

### EcsPool

`EcsPool<T>` manages components of a specific type, providing APIs for adding, getting, and removing components from entities.

```csharp
EcsPool<Component1> pool = world.GetPool<Component1>();
ref Component1 c1 = ref pool.Add(entity);
bool exists = pool.Has(entity);
ref Component1 c1 = ref pool.Get(entity);
pool.Del(entity);
```

### EcsFilter

Filters are used to efficiently query entities that match specific component criteria.

```csharp
EcsFilter filter = world.Filter<Component1>().Exc<Component2>().End();
foreach (int entity in filter) {
    // Process entities
}
```

### EcsSystems

`EcsSystems` is a container for systems, managing their execution order and lifecycle.

```csharp
EcsSystems systems = new EcsSystems(world);
systems.Add(new UserSystem()).Init();
```

## Integration with Game Engines

### Unity

LeoEcsLite is compatible with Unity, offering asmdef files for modular compilation and integration.

### Custom Engines

For custom game engines, LeoEcsLite requires C# 7.3 or later.

## Examples

### Basic Setup

This example demonstrates how to set up a basic ECS world, create entities, add components, and process them with a simple system.

```csharp
// Initialize the world and systems
var world = new EcsWorld();
var systems = new EcsSystems(world);
systems
    .Add(new MovementSystem())
    .Init();

// Game loop
while (gameIsRunning) {
    systems.Run();
}

// Cleanup
systems.Destroy();
world.Destroy();
```

### Advanced Usage

#### Implementing a Health System

This hypothetical example demonstrates how to implement a health system that processes entities with health components, applying damage and removing entities with no health left.

```csharp
struct HealthComponent {
    public int Value;
}

class DamageSystem : IEcsRunSystem {
    private EcsFilter _filter;
    private EcsPool<HealthComponent> _healthPool;

    public void Run(EcsSystems systems) {
        var world = systems.GetWorld();
        if (_filter == null) {
            _filter = world.Filter<HealthComponent>().End();
            _healthPool = world.GetPool<HealthComponent>();
        }

        foreach (var entity in _filter) {
            ref var health = ref _healthPool.Get(entity);
            health.Value -= 10; // Apply damage
            if (health.Value <= 0) {
                world.DelEntity(entity); // Remove entity if health is depleted
            }
        }
    }
}
```

## License

LeoEcsLite is released under dual licenses. For more details, see the [LICENSE.md](https://github.com/Leopotam/ecslite/blob/master/LICENSE.md) file in the repository.

This documentation, enriched with code examples and comments, should provide a solid foundation for understanding and utilizing LeoEcsLite in your projects. For further exploration and examples, visit the [LeoEcsLite GitHub repository](https://github.com/Leopotam/ecslite).