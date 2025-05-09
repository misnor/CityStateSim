# CityStateSim

City state simulator (along the same vein as Dwarf Fortress / Songs of Syx) with C#, MonoGame, and DefaultECS. This repository follows clean architectural principles and testable patterns to ensure extensibility as features grow.

---

## 🎯 High-Level Overview

- **Domain-Driven Core** (`CityStateSim.Core`):  
  - Pure data components, domain interfaces, and events.  
  - No external framework dependencies.

- **Infrastructure** (`CityStateSim.Infrastructure`):  
  - Implements Core interfaces (ECS world factory, event buses, JSON config loader).  
  - Provides DI extension methods.  
  - Contains adapters for DefaultECS messaging and persistence stubs.

- **Gameplay** (`CityStateSim.Gameplay`):  
  - Data-driven services (map generation, job logic, command definitions).  

- **UI Host** (`CityStateSim.UI`):  
  - MonoGame entry point (`Program.cs` & `MainGame`).  
  - Renders ECS world, handles input → commands.  
  - Wires DI, logging, event buses, and simulation runner.

- **Tests** (`*.Tests` projects):  
  - Unit tests for Infrastructure (world factory, event buses, config).  
  - Integration tests for DI wiring and headless simulation.

---

## 🔑 Key Patterns & Practices

1. **Clean/Onion Architecture**  
   Layers with strict inward dependencies: UI → Gameplay → Infrastructure → Core.

2. **Entity-Component-System (ECS)**  
   - Core components (`PositionComponent`, `TileTypeComponent`) as record structs.  
   - DefaultECS world created via `IWorldFactory` in Infrastructure.  
   - `IEcsEventBus` adapter wraps DefaultECS messaging.

3. **Dependency Injection**  
   - `Microsoft.Extensions.DependencyInjection` driven by extension methods in Infrastructure.  

4. **Event-Driven Decoupling**  
   - **ECS bus** for in-world events (e.g. `JobCreated`, `TickOccurred`).  
   - **App bus** for UI/app events (e.g. `WindowResized`, `PauseCommandIssued`).

5. **Data-Driven Configuration**  
   - JSON config (`tiles.json`) drives map generation, enabling tuning without code changes.  

7. **Command Pattern for Input**  
   - Encapsulate player actions (`PauseCommand`, `SpeedCommand`) as discrete command objects.  
   - Input system maps keys/mouse to command executions via app event bus.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- MonoGame DesktopGL (via NuGet in UI project)  

### Building & Running

```bash
# Restore & build solution
dotnet restore
dotnet build

# Run tests
dotnet test --no-build

# Launch the game
cd CityStateSim.UI
dotnet run
