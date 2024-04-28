using System;
using GameLovers.UiService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Presenters
{
	/// <summary>
	/// This Presenter handles the loading screen UI by:
	/// - Showing the loading percentage status
	/// </summary>
	public class LoadingScreenPresenter : UiPresenter
	{
		[SerializeField] private Slider _loadingBar;
		[SerializeField] private TextMeshProUGUI _loadingText;

		public float LoadingPercentage => _loadingBar.value;

		/// <summary>
		/// Sets the loading screen to the given <paramref name="percentage"/>
		/// </summary>
		public void SetLoadingPercentage(float percentage)
		{
			_loadingBar.value = percentage;
		}

		/// <summary>
		/// Set's the loading bar text
		/// </summary>
		public void SetBarText(string text)
		{
			_loadingText.text = text;
		}

		protected override void OnOpened()
		{
			_loadingBar.value = 0;
		}
	}
}