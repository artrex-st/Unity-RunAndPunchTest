using UnityEngine;

namespace GamePlay {
    public readonly struct RequestMoveAnimationEvent : IEvent
    {
        public readonly float Speed;

        public RequestMoveAnimationEvent(float speed)
        {
            Speed = speed;
        }
    }

    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviour
    {
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        //private bool _isGameRunning;

        private Animator Animator => GetComponent<Animator>();

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
            new ResponseGameStateUpdateEvent().AddListener(HandlerRequestNewGameStateEvent);
            new RequestMoveAnimationEvent().AddListener(HandlerRequestMoveAnimationEvent);
        }

        private void HandlerRequestNewGameStateEvent(ResponseGameStateUpdateEvent e)
        {
            Animator.enabled = e.CurrentGameState.Equals(GameStates.GameRunning);
        }

        private void HandlerRequestMoveAnimationEvent(RequestMoveAnimationEvent e)
        {
            Animator.SetFloat(Move, e.Speed);
            Animator.SetBool(IsMoving, e.Speed != 0);
        }

        private void Dispose()
        {
            new ResponseGameStateUpdateEvent().RemoveListener(HandlerRequestNewGameStateEvent);
            new RequestMoveAnimationEvent().RemoveListener(HandlerRequestMoveAnimationEvent);
        }
    }
}
