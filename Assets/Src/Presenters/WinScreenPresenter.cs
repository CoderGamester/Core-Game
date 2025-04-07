using Game.Services;
using GameLovers.Services;
using GameLovers.UiService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Presenters
{
    /// <summary>
    /// This Presenter handles the Win Screen UI by:
    /// - Allowing the user to leave the level and return to the main menu
    /// </summary>
    public class WinScreenPresenter : UiPresenter<WinScreenPresenter.PresenterData>
    {
        public struct PresenterData
        {
            public UnityAction OnMenuClicked;
        }
        
        [SerializeField] private Button _menuButton;

        private IGameServicesLocator _services;

        private void Awake()
        {
            _services = MainInstaller.Resolve<IGameServicesLocator>();

            _menuButton.onClick.AddListener(() => Data.OnMenuClicked.Invoke());
        }
    }
}
