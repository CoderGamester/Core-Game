using System.Collections.Generic;

namespace Game.Services.Analytics
{
	/// <summary>
	/// This class provides the necessary behaviour to manage the analytics endpoints of the app's UI calls
	/// </summary>
	public class AnalyticsUI : AnalyticsBase
	{
		public static readonly string Play = "play_button";

		public AnalyticsUI(IAnalyticsService analyticsService) : base(analyticsService)
		{
		}
		
		/// <summary>
		/// Logs when the user opens a screen
		/// </summary>
		/// <param name="screenName">A name that identifies the screen we opened</param>
		public void ScreenView(string screenName)
		{
			var data = new Dictionary<string, object>
			{
				{"screen_name", screenName}
			};
			
			LogEvent(AnalyticsEvents.ScreenView, data);
		}

		/// <summary>
		/// Logs when the user clicks a button
		/// </summary>
		/// <param name="buttonName">A name that identifies the button we clicked</param>
		public void ButtonAction(string buttonName)
		{
			var data = new Dictionary<string, object>
			{
				{"button", buttonName}
			};
			
			LogEvent(AnalyticsEvents.ButtonAction, data);
		}
	}
}