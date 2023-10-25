### `UISystemBase` Class Documentation

#### Namespace:
`Game.UI`

#### Description:
The `UISystemBase` class provides a foundational structure for UI systems within the game. It offers capabilities to manage UI bindings, their updates, and the lifecycle of UI-related resources.

#### Dependencies:
- Colossal.Logging
- Colossal.Serialization.Entities
- Colossal.UI.Binding
- Game.SceneFlow
- System.Collections.Generic
- UnityEngine.Scripting

#### Inherits:
- `GameSystemBase`

#### Fields:

- **`log`**: A static logging instance for logging UI-related information.

- **`m_Bindings`**: A private list containing the UI bindings.

- **`m_UpdateBindings`**: A private list containing the bindings that require updates.

#### Properties:

---

##### `gameMode`

**Type**: `GameMode`  
**Description**: Represents the supported game mode(s) for this UI system. The default value is `GameMode.All`.

---

#### Methods:

---

##### `OnCreate()`

**Description**: This method initializes the UI system's resources and is called upon creation. It initializes the binding lists.

---

##### `OnDestroy()`

**Description**: This method handles the destruction phase for the UI system. It ensures that all the bindings are properly removed.

---

##### `OnUpdate()`

**Description**: Called during the update phase, it ensures that all bindings requiring updates get processed.

---

##### `AddBinding(IBinding binding)`

**Parameters**:  
- `IBinding binding`: The UI binding to be added.

**Description**: Adds a UI binding to the system and registers it with the game manager's user interface.

---

##### `AddUpdateBinding(IUpdateBinding binding)`

**Parameters**:  
- `IUpdateBinding binding`: The UI binding that requires updates.

**Description**: Adds a UI binding to the system that requires updates, and also registers it with the game manager's user interface.

---

##### `OnGamePreload(Purpose purpose, GameMode mode)`

**Parameters**:  
- `Purpose purpose`: The purpose for which the game is preloading.
  
- `GameMode mode`: The current game mode.

**Description**: Prepares the UI system for game preloading. Sets the `Enabled` property based on the compatibility of the current game mode with the system's `gameMode`.

---

#### Remarks:

- The class has a static logger `log` which is configured to log UI-related information.
- The class maintains two lists: `m_Bindings` for bindings and `m_UpdateBindings` for bindings that need updates.
- Binding management is crucial in UI systems to ensure efficient updates, rendering, and resource management.