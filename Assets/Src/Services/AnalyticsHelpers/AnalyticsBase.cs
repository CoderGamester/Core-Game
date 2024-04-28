using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game.Services.Analytics
{
	/// <summary>
	/// Analytics base class for all analytics endpoint calls
	/// </summary>
	public abstract class AnalyticsBase
	{
		protected IAnalyticsService _analyticsService;
		
		protected AnalyticsBase(IAnalyticsService analyticsService)
		{
			_analyticsService = analyticsService;
		}

		/// <summary>
		/// Logs an analytics event with the given <paramref name="eventName"/>.
		/// </summary>
		protected void LogEvent(string eventName, Dictionary<string, object> parameters = null)
		{
			try
			{
				/*
				//PlayFab Analytics
				if (PlayFabSettings.staticPlayer.IsClientLoggedIn())
				{
					var request = new WriteClientPlayerEventRequest { EventName = eventName, Body = parameters };
					PlayFabClientAPI.WritePlayerEvent(request, null, null);
				}
				*/

				if (parameters == null || parameters.Count == 0)
				{
					// Unity
					UnityEngine.Analytics.Analytics.CustomEvent(eventName);
					return;
				}

				if (parameters.Count > 10)
				{
					Debug.LogError($"The event {eventName} has {parameters.Count} and the max parameters for unity is 10");
				}

				// Unity
				UnityEngine.Analytics.Analytics.CustomEvent(eventName, parameters);
			}
			catch (Exception e)
			{
				Debug.LogError("Error while sending analytics: " + e.Message);
				Debug.LogException(e);
			}
		}
	}
}