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
			LogEvent(AnalyticsEvents.MainMenuEnter);
		}
	}
}
