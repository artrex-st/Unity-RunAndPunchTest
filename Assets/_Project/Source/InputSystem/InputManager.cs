using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem {
    public readonly struct InputMoveEvent : IEvent
    {
        public readonly Vector2 MoveDirection;
        public readonly double Duration;
        public readonly InputActionPhase CurrentPhase;

        public InputMoveEvent(Vector2 moveDirection, double duration, InputActionPhase currentPhase)
        {
            MoveDirection = moveDirection;
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

            _inputsActions.Player.Move.started += Move;
            _inputsActions.Player.Move.performed += Move;
            _inputsActions.Player.Move.canceled += Move;

            _inputsActions.Player.Hit.started += HitStarted;
        }

        private void Move(InputAction.CallbackContext context)
        {
            new InputMoveEvent(context.ReadValue<Vector2>(), context.duration, context.phase).Invoke();
        }

        private void HitStarted(InputAction.CallbackContext context)
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
            _inputsActions.Player.Move.Dispose();
            _inputsActions.Player.Hit.Dispose();
        }
    }
}
