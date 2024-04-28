using System;
using Game.Services;
using TMPro;
using UnityEngine;

namespace Game.Views
{
	/// <summary>
	/// This View handles the Timer View in the UI:
	/// - Showing the current UTC time status
	/// </summary>
	public class TimerView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _timeText;
		
		private IGameServices _services;

		/// <summary>
		/// Initializes the view to start it's update loop.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Thrown if is called more than one time for the same entity GameObject
		/// </exception>
		public void Init(IGameServices services)
		{
			if (_services != null)
			{
				throw new InvalidOperationException("The Timer View was already initialized");
			}
			
			_services = services;
			
			_services.TickService.SubscribeOnUpdate(UpdateTime, 1, true, true);
		}

		private void UpdateTime(float deltatime)
		{
			_timeText.text = _services.TimeService.DateTimeUtcNow.ToString();
		}
	}
}