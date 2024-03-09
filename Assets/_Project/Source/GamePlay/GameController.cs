using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public readonly struct RequestMoneyUiEvent : IEvent
    {
        public readonly int Money;
        public readonly int Level;

        public RequestMoneyUiEvent(int money, int level)
        {
            Money = money;
            Level = level;
        }
    }

    public sealed class GameController : BaseScreen
    {
        [SerializeField] private Button _mainMenuBtn;
        [SerializeField] private TextMeshProUGUI _moneyUi;
        [SerializeField] private ScreenReference _gameMenuRef;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private new void Initialize()
        {
            base.Initialize();
            new RequestMoneyUiEvent().AddListener(HandlerRequestMoneyUiEvent);
            _mainMenuBtn.onClick.AddListener(HandlerGameMenuClick);
            StartGame();
        }

        private async void StartGame()
        {
            //TODO: Finish Loading screen fade effect time using DOTween
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            new RequestGameStateUpdateEvent(GameStates.GameRunning).Invoke();
        }

        private void HandlerGameMenuClick()
        {
            ScreenService.LoadingSceneAdditiveAsync(_gameMenuRef);
            new RequestGameStateUpdateEvent(GameStates.GamePaused).Invoke();
        }

        private void HandlerRequestMoneyUiEvent(RequestMoneyUiEvent e)
        {
            _moneyUi.text = $"Level:{e.Level}, ${e.Money:N2}";
        }

        private new void Dispose()
        {
            base.Dispose();
            new RequestMoneyUiEvent().RemoveListener(HandlerRequestMoneyUiEvent);

        }
    }
}
