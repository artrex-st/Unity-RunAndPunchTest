using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem {
    public readonly struct InputXEvent : IEvent
    {
        public readonly float MoveRotation;
        public readonly double Duration;
        public readonly InputActionPhase CurrentPhase;

        public InputXEvent(float moveRotation, double duration, InputActionPhase currentPhase)
        {
            MoveRotation = moveRotation;
            Duration = duration;
            CurrentPhase = currentPhase;
        }
    }

    public readonly struct InputYEvent : IEvent
    {
        public readonly float MoveForward;
        public readonly double Duration;
        public readonly InputActionPhase CurrentPhase;

        public InputYEvent(float moveForward, double duration, InputActionPhase currentPhase)
        {
            MoveForward = moveForward;
            Duration = duration;
            CurrentPhase = currentPhase;
        }
    }

    public readonly struct RequestInputPressEvent : IEvent
    {
        public readonly double Duration;
        public readonly InputActionPhase CurrentPhase;

        public RequestInputPressEvent(double duration, InputActionPhase currentPhase)
        {
            Duration = duration;
            CurrentPhase = currentPhase;
        }
    }

    public class InputManager : MonoBehaviour
    {
        private InputActions _inputsActions;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize()
        {
            _inputsActions = new InputActions();
            _inputsActions.Player.Enable();

            _inputsActions.Player.Axis_X.started += MoveX;
            _inputsActions.Player.Axis_X.performed += MoveX;
            _inputsActions.Player.Axis_X.canceled += MoveX;

            _inputsActions.Player.Axis_Y.started += RotateY;
            _inputsActions.Player.Axis_Y.performed += RotateY;
            _inputsActions.Player.Axis_Y.canceled += RotateY;

            _inputsActions.Player.Press.started += PressStarted;
        }

        private void MoveX(InputAction.CallbackContext context)
        {
            new InputXEvent(context.ReadValue<float>(), context.duration, context.phase).Invoke();
        }

        private void RotateY(InputAction.CallbackContext context)
        {
            new InputYEvent(context.ReadValue<float>(), context.duration, context.phase).Invoke();
        }

        private void PressStarted(InputAction.CallbackContext context)
        {
            new RequestInputPressEvent(context.duration, context.phase).Invoke();
        }

        private static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }

        private void Dispose()
        {
            _inputsActions.Player.Axis_X.Dispose();
            _inputsActions.Player.Axis_Y.Dispose();
            _inputsActions.Player.Press.Dispose();
        }
    }
}
