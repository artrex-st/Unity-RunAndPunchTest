using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem {
    public readonly struct InputMoveEvent : IEvent
    {
        public readonly float MoveDirection;
        public readonly float MoveRotation;
        public readonly double Duration;
        public readonly InputActionPhase CurrentPhase;

        public InputMoveEvent(float moveDirection, float moveRotation, double duration, InputActionPhase currentPhase)
        {
            MoveDirection = moveDirection;
            MoveRotation = moveRotation;
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

            _inputsActions.Player.Move.started += MoveX;
            _inputsActions.Player.Move.performed += MoveX;
            _inputsActions.Player.Move.canceled += MoveX;

            _inputsActions.Player.Hit.started += HitStarted;
        }

        private void MoveX(InputAction.CallbackContext context)
        {
            new InputMoveEvent(context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x, context.duration, context.phase).Invoke();
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
