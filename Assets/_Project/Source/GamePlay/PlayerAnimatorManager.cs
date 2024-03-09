using InputSystem;
using System;
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

    public readonly struct ResponsePunchAnimationEvent : IEvent
    {
        public readonly int InstanceId;

        public ResponsePunchAnimationEvent(int speed)
        {
            InstanceId = speed;
        }
    }

    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviour
    {
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Punch = Animator.StringToHash("Punch");
        private bool _canPunch = true;

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
            new ResponsePunchAnimationEvent().AddListener(HandlerResponsePunchAnimationEvent);
            new RequestInputPressEvent().AddListener(HandlerRequestInputPressEvent);

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

        private void HandlerRequestInputPressEvent(RequestInputPressEvent e)
        {
            if (_canPunch)
            {
                Animator.SetTrigger(Punch);
                _canPunch = false;
            }
        }

        private void HandlerResponsePunchAnimationEvent(ResponsePunchAnimationEvent e)
        {
            if (e.InstanceId == transform.parent.GetInstanceID())
            {
                _canPunch = true;
            }
        }

        private void Dispose()
        {
            new ResponseGameStateUpdateEvent().RemoveListener(HandlerRequestNewGameStateEvent);
            new RequestMoveAnimationEvent().RemoveListener(HandlerRequestMoveAnimationEvent);
            new ResponsePunchAnimationEvent().RemoveListener(HandlerResponsePunchAnimationEvent);
            new RequestInputPressEvent().RemoveListener(HandlerRequestInputPressEvent);
        }
    }
}
