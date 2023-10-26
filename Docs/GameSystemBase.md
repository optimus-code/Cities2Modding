### `GameSystemBase` Class Documentation

**Namespace**: `Game`

**Inherits**: `COSystemBase`

**Assembly**: Game

---

#### Description:
The `GameSystemBase` class serves as an abstract base class to manage various game-related systems, such as handling game loading, state changes, and focus events.

---

#### Fields:

- `private LoadGameSystem m_LoadGameSystem`: An instance of the `LoadGameSystem` which manages game-loading functionalities.

---

#### Constructor:

- `GameSystemBase()`: Constructor for the `GameSystemBase` class.

---

#### Methods:

- `OnCreate()`: Overrides the base `OnCreate` method. Sets up event listeners for game preload, game loading completion, save game loading, and application focus changes.

- `OnDestroy()`: Overrides the base `OnDestroy` method. Removes previously registered event listeners.

- `GameLoadingComplete(Purpose purpose, GameMode mode)`: Handles the completion of game loading and invokes the virtual `OnGameLoadingComplete` method.

- `GameLoaded(Context serializationContext)`: Handles when the game is loaded and invokes the virtual `OnGameLoaded` method.

- `GamePreload(Purpose purpose, GameMode mode)`: Handles game preloading and invokes the virtual `OnGamePreload` method.

- `FocusChanged(bool hasfocus)`: An event callback that handles focus change events and invokes the virtual `OnFocusChanged` method.

- `OnGamePreload(Purpose purpose, GameMode mode)`: A virtual method intended to be overridden by derived classes to handle game preloading events.

- `OnGameLoaded(Context serializationContext)`: A virtual method intended to be overridden by derived classes to handle game loaded events.

- `OnGameLoadingComplete(Purpose purpose, GameMode mode)`: A virtual method intended to be overridden by derived classes to handle game loading completion events.

- `OnFocusChanged(bool hasFocus)`: A virtual method intended to be overridden by derived classes to handle focus change events.

- `GetUpdateInterval(SystemUpdatePhase phase)`: Returns an integer representing the update interval for a given phase. Defaults to 1.

- `GetUpdateOffset(SystemUpdatePhase phase)`: Returns an integer representing the update offset for a given phase. Defaults to -1.

- `ResetDependency()`: Resets the system's job dependency.

---

#### Remarks:

This class serves as an abstraction for game-related systems, and its main purpose is to manage the lifecycle of game states, from preloading to loading completion. It also manages application focus changes. When overriding its methods, ensure that super-calls (`base.MethodName()`) are made where appropriate.

Note: As this is decompiled code, there might be missing context, and certain details could have been lost in the decompilation process. Ensure to thoroughly test and review any modifications made based on this documentation.

---

#### Example implementation:
```
using Unity.Entities;
using UnityEngine.Scripting;

namespace ExampleMod
{
    public class ExampleSystem : GameSystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
        }
    }
}
```