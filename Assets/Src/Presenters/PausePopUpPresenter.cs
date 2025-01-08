using Game.Commands;
using Game.Services;
using GameLovers.Services;
using GameLovers.UiService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Presenters
{
    /// <summary>
    /// This Presenter handles the Pause Pop Up UI by:
    /// - Allowing the user to restart the game
    /// - Allowing the user to return to the main menu and leave the game at the same state
    /// </summary>
    public class PausePopUpPresenter : UiPresenter<PausePopUpPresenter.PresenterData>
    {
        public struct PresenterData
        {
            public UnityAction OnCloseClicked;
            public UnityAction OnReturnMenuClicked;
        }
        
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _closeButton;

        private IGameServicesLocator _services;

        private void Awake()
        {
            _services = MainInstaller.Resolve<IGameServicesLocator>();

            _restartButton.onClick.AddListener(Restart);
            _menuButton.onClick.AddListener(() => Data.OnReturnMenuClicked.Invoke());
            _closeButton.onClick.AddListener(() => Data.OnCloseClicked.Invoke());
        }

        private void Restart()
        {
            _services.CommandService.ExecuteCommand(new RestartGameCommand());
        }
    }
}