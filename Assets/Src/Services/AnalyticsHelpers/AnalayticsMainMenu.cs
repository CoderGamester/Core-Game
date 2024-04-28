using System.Collections.Generic;

namespace Game.Services.Analytics
{
	/// <summary>
	/// This class provides the necessary behaviour to manage the analytics endpoints for the game's main menu
	/// </summary>
	public class AnalyticsMainMenu : AnalyticsBase
	{
		public AnalyticsMainMenu(IAnalyticsService analyticsService) : base(analyticsService)
		{
		}

		/// <summary>
		/// Logs the event when the player enters the main menu
		/// </summary>
		public void MainMenuEnter()
		{
			var data = new Dictionary<string, object>
			{
			};

			LogEvent(AnalyticsEvents.MainMenuEnter, data);
		}

		/// <summary>
		/// Logs the event when the player leaves the main menu
		/// </summary>
		public void MainMenuExit()
		{
			var data = new Dictionary<string, object>
			{
			};

			LogEvent(AnalyticsEvents.MainMenuExit, data);
		}
	}
}
