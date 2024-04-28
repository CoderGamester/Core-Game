using Game.Services.Analytics;

namespace Game.Services
{
	/// <summary>
	/// Static class that defines all the event types names
	/// </summary>
	public static class AnalyticsEvents
	{
		public static readonly string SessionStart = "session_start";
		public static readonly string SessionEnd = "session_end";
		public static readonly string SessionHeartbeat = "session_heartbeat";
		public static readonly string AdsData = "ads_data";
		public static readonly string LoadingStarted = "loading_started";
		public static readonly string LoadingCompleted = "loading_completed";
		public static readonly string PlayerLogin = "player_login";
		public static readonly string ScreenView = "screen_view";
		public static readonly string ButtonAction = "button_action";
		public static readonly string MainMenuEnter = "main_menu_enter";
		public static readonly string MainMenuExit = "main_menu_exit";
		public static readonly string Error = "error_log";
		public static readonly string Purchase = "purchase";
	}

	/// <summary>
	/// The analytics service is an endpoint in the game to log custom events to Game's analytics console
	/// </summary>
	public interface IAnalyticsService
	{
		/// <inheritdoc cref="AnalyticsSession"/>
		AnalyticsSession SessionCalls { get; }
		/// <inheritdoc cref="AnalyticsEconomy"/>
		AnalyticsEconomy EconomyCalls { get; }
		/// <inheritdoc cref="AnalyticsErrors"/>
		AnalyticsErrors ErrorsCalls { get; }
		/// <inheritdoc cref="AnalyticsUI"/>
		AnalyticsUI UiCalls { get; }
		/// <inheritdoc cref="AnalyticsMainMenu"/>
		AnalyticsMainMenu MainMenuCalls { get; }
		/// <inheritdoc cref="AnalyticsMatch"/>
	}

	/// <inheritdoc />
	public class AnalyticsService : IAnalyticsService
	{
		/// <inheritdoc />
		public AnalyticsSession SessionCalls { get; }
		/// <inheritdoc />
		public AnalyticsEconomy EconomyCalls { get; }
		/// <inheritdoc />
		public AnalyticsErrors ErrorsCalls { get; }
		/// <inheritdoc />
		public AnalyticsUI UiCalls { get; }
		/// <inheritdoc />
		public AnalyticsMainMenu MainMenuCalls { get; }

		public AnalyticsService()
		{
			SessionCalls = new AnalyticsSession(this);
			EconomyCalls = new AnalyticsEconomy(this);
			ErrorsCalls = new AnalyticsErrors(this);
			UiCalls = new AnalyticsUI(this);
			MainMenuCalls = new AnalyticsMainMenu(this);
		}
	}
}
