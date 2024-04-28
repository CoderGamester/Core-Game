using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services.Analytics
{
	/// <summary>
	/// This class provides the necessary behaviour to manage the analytics endpoints of the app's error or crash moments
	/// </summary>
	public class AnalyticsErrors : AnalyticsBase
	{
		public enum ErrorType
		{
			Session,
			Disconnect,
			Match,
		}

		public AnalyticsErrors(IAnalyticsService analyticsService) : base(analyticsService)
		{
		}

		/// <summary>
		/// Reports an error of predefined <paramref name="type"/>
		/// </summary>
		public void ReportError(ErrorType type, string description)
		{
			var data = new Dictionary<string, object>
			{
				{"type", type.ToString()},
				{"description", description}
			};
			
			LogEvent(AnalyticsEvents.Error, data);
			Debug.LogError(description);
		}

		/// <summary>
		/// Logs a crash with the given <paramref name="exception"/>
		/// </summary>
		public void CrashLog(Exception exception)
		{
			ReportError(AnalyticsErrors.ErrorType.Session, "CrashLog:" + exception.Message);
			Debug.LogException(exception);
		}
	}
}