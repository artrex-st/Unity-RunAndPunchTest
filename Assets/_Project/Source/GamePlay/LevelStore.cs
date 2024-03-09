using System;
using TMPro;
using UnityEngine;
using Utility;

namespace GamePlay
{
    public readonly struct RequestLevelUpEvent : IEvent { }
    public readonly struct ResponseBuyLevelUpEvent : IEvent
    {
        public readonly int BodyMoney;

        public ResponseBuyLevelUpEvent(int bodyMoney)
        {
            BodyMoney = bodyMoney;
        }
    }

    [RequireComponent(typeof(Collider))]
    public class LevelStore : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textLevel;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                new RequestLevelUpEvent().Invoke();
            }
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize()
        {
            new ResponseBuyLevelUpEvent().AddListener(HandlerResponseBuyLevelUpEvent);
        }

        private void HandlerResponseBuyLevelUpEvent(ResponseBuyLevelUpEvent e)
        {
            _textLevel.text = $"Next level cost: {e.BodyMoney:N2}";
        }

        private void Dispose()
        {
            new ResponseBuyLevelUpEvent().RemoveListener(HandlerResponseBuyLevelUpEvent);
        }
    }
}
