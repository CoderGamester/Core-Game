using System.Collections.Generic;
using Freya;
using Game.Commands;
using Game.Services;
using Game.Utils;
using GameLovers.Services;
using GameLovers.UiService;
using GameLovers.UiService.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Presenters
{
    /// <summary>
    /// This Presenter handles the Compliance Screen by:
    /// - Presenting the terms of service and policy compliance for the player to read
    /// - Allowing the user to accept the terms of service and policy and start the game
    /// - Set the player's age
    /// </summary>
    public class ComplianceScreenPresenter : UiPresenter
    {
        [SerializeField] private Button _acceptButton;
        [SerializeField] private TextMeshProUGUI _ageText;
        [SerializeField] private InteractableTextView[] _interactableTextViews;
        [SerializeField] private Slider _ageSlider;

        private readonly Dictionary<string, string> _links = new Dictionary<string, string>()
        {
            { "Terms", Constants.Settings.Terms_Link },
            { "Policy", Constants.Settings.Policy_Link },
        };

        private int _age;
        private IGameServicesLocator _services;

        private void Awake()
        {
            _services = MainInstaller.Resolve<IGameServicesLocator>();

            _ageSlider.onValueChanged.AddListener(OnAgeSliderValueChanged);
            _acceptButton.onClick.AddListener(AcceptButtonClicked);
            OnAgeSliderValueChanged(0);

            foreach (var view in _interactableTextViews)
            {
                view.OnLinkedInfoClicked.AddListener(OnLinkedInfoClicked);
            }
        }

        private void OnAgeSliderValueChanged(float newValue)
        {
            _age = Mathf.Lerp(0, Constants.Settings.Age_Max_Value, newValue).RoundToInt();
            _ageText.text = _age.ToString();
            _acceptButton.interactable = _age > Constants.Settings.Age_Min_Value;

            if (_age == Constants.Settings.Age_Max_Value)
            {
                _ageText.text += "+";
            }
        }

        private void AcceptButtonClicked()
        {
            _services.CommandService.ExecuteCommand(new AcceptComplianceCommand(_age));
        }

        private void OnLinkedInfoClicked(TMP_LinkInfo info)
        {
            Application.OpenURL(_links[info.GetLinkID()]);
        }
    }
}