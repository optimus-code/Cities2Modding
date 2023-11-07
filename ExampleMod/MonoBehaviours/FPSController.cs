using Cinemachine;
using ExampleMod.Systems;
using Game.Rendering;
using Game.Simulation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExampleMod.MonoBehaviours
{
    /// <summary>
    /// A basic FPS controller toggled via CTRL + F.
    /// </summary>
    /// <remarks>
    /// (Is smoother and slower/more realistic as an FPS controller than Photo mode. This
    /// could be expanded much further as a proper mod.)
    /// </remarks>
    public class FPSController : MonoBehaviour
    {
        private bool IsActive
        {
            get;
            set;
        }

        private CinemachineVirtualCamera virtualCamera;
        private Transform childTransform;
        private TerrainSystem terrainSystem;
        private CameraUpdateSystem cameraUpdateSystem;
        private ExampleUISystem exampleUISystem;

        private List<InputAction> temporaryActions = new List<InputAction>( );
        private Vector2 mousePosition;
        private Vector2 movementAxes;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Quaternion targetChildRotation;

        public float sensitivity = 5f; // Adjust sensitivity as needed
        public float movementSpeed = 0.1f;
        public float runSpeed = 0.2f;

        public float bobFrequency = 1.1f; // How fast the bobbing occurs
        public float bobVerticalAmplitude = 0.06f; // How far the camera moves up and down
        public float bobbingSpeed = 10f; // How fast the bobbing transitions to the target position
        private float bobTimer = 0.0f;

        public float swayFrequency = 0.3f; // The speed of the sway
        public float swayAmplitude = 0.0005f; // The magnitude of the sway
        private float swayTimer = 0.0f;

        public float breathingFrequency = 0.5f; // The speed of the breathing effect
        public float breathingAmplitude = 0.001f; // The magnitude of the breathing pitch oscillation
        private float breathingTimer = 0.0f;

        private float yaw = 0f;
        private float pitch = 0f;

        private void Start( )
        {
            CreateVirtualCamera( );
            SetupShortcut( );
        }

        private void Update( )
        {
            if ( terrainSystem == null || !IsActive )
                return;

            ApplyRotation( );

            transform.rotation = Quaternion.Lerp( transform.rotation, targetRotation, 6f * Time.deltaTime );
            childTransform.localRotation = Quaternion.Lerp( childTransform.localRotation, targetChildRotation, 6f * Time.deltaTime );

            ApplyPosition( );

            ApplyHeadBob( );
            ApplyBreathingEffect( );
            ApplySwayEffect( );

            transform.position = Vector3.Lerp( transform.position, targetPosition, 10f * Time.deltaTime );
        }

        private void ApplyRotation( )
        {
            // mousePosition is assumed to be the delta movement
            float mouseX = mousePosition.x * sensitivity * Time.deltaTime;
            float mouseY = mousePosition.y * sensitivity * Time.deltaTime;

            // Update yaw based on mouse input
            yaw += mouseX;
            // Update pitch based on mouse input, and clamp it
            pitch -= mouseY;
            pitch = Mathf.Clamp( pitch, -90f, 90f );

            // Get the current yaw rotation by applying the yaw to the transform's up vector
            Quaternion yawRotation = Quaternion.AngleAxis( yaw, Vector3.up );

            // Get the current pitch rotation by applying the pitch only to the local right vector
            Quaternion pitchRotation = Quaternion.AngleAxis( pitch, Vector3.right );

            // Combine the rotations
            targetRotation = yawRotation;
            targetChildRotation = pitchRotation;
        }

        private void ApplyPosition( )
        {
            var groundY = GetGroundY( );

            var right = new Vector3( childTransform.right.x, 0f, childTransform.right.z );
            var forward = new Vector3( childTransform.forward.x, 0f, childTransform.forward.z );

            // Calculate the direction vector without applying the movement speed yet
            var direction = Vector3.zero;

            // Add to the direction based on the input axes
            if ( movementAxes.x > 0 )
                direction += right;
            else if ( movementAxes.x < 0 )
                direction -= right;

            if ( movementAxes.y > 0 )
                direction += forward;
            else if ( movementAxes.y < 0 )
                direction -= forward;

            // Normalize the direction vector if there's movement to maintain a constant speed
            if ( direction.magnitude > 0 )
                direction.Normalize( );

            // Now apply the movement speed to the direction
            targetPosition += direction * movementSpeed;
            targetPosition.y = groundY;
        }

        private void ApplyHeadBob( )
        {
            // Only apply head bob if the character is moving
            if ( movementAxes.x != 0 || movementAxes.y != 0 )
            {
                // Increment the timer with the bobbing speed
                bobTimer += Time.deltaTime * bobbingSpeed;

                // Calculate the new Y position using a sine wave
                float bobOffset = Mathf.Sin( bobTimer * bobFrequency ) * bobVerticalAmplitude;

                // Apply the offset to the targetPosition's Y component
                targetPosition.y += bobOffset;
            }
            else
            {
                // Reset the bob timer when the character stops moving
                bobTimer = 0;
            }
        }

        private void ApplyBreathingEffect( )
        {
            // Only apply the breathing effect if the character is not moving
            if ( movementAxes.x == 0 && movementAxes.y == 0 )
            {
                breathingTimer += Time.deltaTime;

                // Calculate the breathing offset using a sine wave for a smooth, natural effect
                float breathingOffset = Mathf.Sin( breathingTimer * breathingFrequency ) * breathingAmplitude;

                // Apply the offset to the targetPosition's Y component for vertical breathing movement
                targetPosition.y += breathingOffset;

                // Calculate the breathing pitch offset, which should be very subtle
                float pitchOffset = Mathf.Sin( breathingTimer * breathingFrequency ) * breathingAmplitude;

                // Modify the targetChildRotation for the pitch (x-axis rotation)
                Vector3 childEulerAngles = targetChildRotation.eulerAngles;
                childEulerAngles.x += pitchOffset;
                targetChildRotation = Quaternion.Euler( childEulerAngles );
            }
            else
            {
                // Reset the breathing timer when the character starts moving
                breathingTimer = 0;
            }
        }

        private void ApplySwayEffect( )
        {
            // Only apply the sway effect if the character is not moving
            if ( movementAxes.x == 0 && movementAxes.y == 0 )
            {
                swayTimer += Time.deltaTime;

                // Calculate the sway offsets using sine for vertical and cosine for horizontal movement
                float swayOffsetX = Mathf.Cos( swayTimer * swayFrequency ) * swayAmplitude;
                float swayOffsetY = Mathf.Sin( swayTimer * swayFrequency ) * swayAmplitude;

                // Apply the sway offsets to the targetPosition's X and Y components
                targetPosition.x += swayOffsetX;
                targetPosition.y += swayOffsetY;
            }
            else
            {
                // Reset the sway timer when the character starts moving
                swayTimer = 0;
            }
        }

        public void AssignSystems( TerrainSystem ts, CameraUpdateSystem cu, ExampleUISystem eui )
        {
            terrainSystem = ts;
            cameraUpdateSystem = cu;
            exampleUISystem = eui;
        }

        private void CreateVirtualCamera( )
        {
            var childObject = new GameObject( "FPSController_Camera" );
            childObject.transform.parent = transform;
            childObject.transform.position = transform.position;

            virtualCamera = childObject.AddComponent<CinemachineVirtualCamera>( );
            virtualCamera.Priority = 0;
            childTransform = virtualCamera.transform;

            transform.position = cameraUpdateSystem.activeCamera.transform.position;
            targetPosition = transform.position;

            targetRotation = transform.rotation;
            targetChildRotation = childTransform.localRotation;
        }

        private void SetupShortcut( )
        {
            var inputAction = new InputAction( "ToggleFPSController" );
            inputAction.AddCompositeBinding( "ButtonWithOneModifier" )
                .With( "Modifier", "<Keyboard>/CTRL" )
                .With( "Button", "<Keyboard>/f" );
            inputAction.performed += ( a ) => Toggle( );
            inputAction.Enable( );

            // Create the input action
            var movementAction = new InputAction( "FPSController_Movement", binding: "<Gamepad>/leftStick" );

            // Add composite bindings for WASD
            movementAction.AddCompositeBinding( "Dpad" )
                .With( "Up", "<Keyboard>/w" )       // W key for up
                .With( "Down", "<Keyboard>/s" )     // S key for down
                .With( "Left", "<Keyboard>/a" )     // A key for left
                .With( "Right", "<Keyboard>/d" );   // D key for right

            movementAction.performed += ctx => movementAxes = ctx.ReadValue<Vector2>( );
            movementAction.canceled += ctx => movementAxes = Vector2.zero;

            temporaryActions.Add( movementAction );

            var mousePositionAction = new InputAction( "FPSController_MousePosition", binding: "<Mouse>/delta" );

            // To read the mouse position
            mousePositionAction.performed += ctx => mousePosition = ctx.ReadValue<Vector2>( );
            mousePositionAction.canceled += ctx => mousePosition = Vector3.zero;

            temporaryActions.Add( mousePositionAction );
        }

        private void EnableTemporaryShortcuts( )
        {
            foreach ( var action in temporaryActions )
            {
                action.Enable( );
            }
        }

        private void DisableTemporaryShortcuts( )
        {
            foreach ( var action in temporaryActions )
            {
                action.Disable( );
            }
        }

        private void Toggle( )
        {
            IsActive = !IsActive;

            virtualCamera.Priority = IsActive ? 9999 : 0;

            exampleUISystem.ToggleUI( IsActive );

            // Update base position of FPS camera
            if ( IsActive )
            {
                transform.position = cameraUpdateSystem.activeCamera.transform.position;
                targetPosition = transform.position;
                EnableTemporaryShortcuts( );
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                DisableTemporaryShortcuts( );
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private float GetGroundY( )
        {
            if ( terrainSystem == null )
                return 0f;

            var heightData = terrainSystem.GetHeightData( true );
            return TerrainUtils.SampleHeight( ref heightData, new Unity.Mathematics.float3( transform.position ) ) + 1.7f; // Offset it a little
        }
    }
}
