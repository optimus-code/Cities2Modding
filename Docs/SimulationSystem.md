# SimulationSystem Documentation

## Introduction:
The `SimulationSystem` class resides under the `Game.Simulation` namespace and plays a central role in managing and updating the state of the game's simulation. This class provides mechanisms for updating, loading, and serializing the simulation, as well as controlling the speed of the simulation.

## Main Features:

### 1. **Constants and Properties**:
- `PENDING_FRAMES_SPEED_FACTOR`: Defines the speed factor for pending frames.
- `LOADING_COUNT`: Represents the total count of loading operations.
- `kLoadingTask`: A string representing the loading task name.
- `frameIndex`: Index of the current frame being processed.
- `frameTime`: Time taken for the current frame.
- `selectedSpeed`: Defines the desired simulation speed. It can be adjusted unless the system is in loading state.
- `smoothSpeed`: Provides a smoothed speed value.
- `loadingProgress`: Represents the progress of the current loading task.
- `frameDuration`: Duration of a frame in the simulation.
- `performancePreference`: Sets or gets the performance preference, which can be FrameRate, Balanced, or SimulationSpeed.

### 2. **Lifecycle Methods**:
- `OnCreate()`: Initializes the system, sets the default speed, and creates other necessary systems.
- `OnDestroy()`: Ensures any pending operations are completed before destroying the system.

### 3. **Serialization**:
- `Serialize<TWriter>()`: Serializes the frame index.
- `Deserialize<TReader>()`: Deserializes the frame index and resets the frame time and timer.
- `SetDefaults()`: Resets the frame index and frame time to their default values.

### 4. **Loading**:
- `OnGamePreload()`: Prepares the system for a new game or loads an existing one.
- `UpdateLoadingProgress()`: Updates the progress of the loading task.

### 5. **Update Loop**:
- `OnUpdate()`: Central update method. If the game is in a loading state, it updates the loading progress. Otherwise, it processes the game's simulation steps based on the selected speed. This includes updating various systems at different phases, namely PreSimulation, EditorSimulation, GameSimulation, and PostSimulation.

### 6. **Performance Preference**:
This class provides an enum `PerformancePreference` that indicates the desired performance mode. The available modes are:
- `FrameRate`: Prioritizes frame rate.
- `Balanced`: Provides a balance between frame rate and simulation speed.
- `SimulationSpeed`: Prioritizes simulation speed.

### 7. **SimulationEndTimeJob**:
A struct that stops and frees the stopwatch when the job is executed.

## Dependencies:
The class uses several other classes and systems for its functionality:
- `UpdateSystem`
- `ToolSystem`
- `PathfindResultSystem`
- `EndFrameBarrier`

It also integrates closely with Unity's `Job` system to handle parallel operations efficiently, especially with the use of `JobHandle`.