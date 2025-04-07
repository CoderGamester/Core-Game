namespace Game.Services.Analytics
{
    /// <summary>
    /// Static class that defines all the event types names
    /// </summary>
    public static class AnalyticsEvents
    {
        public const string SessionStart = "game_session_start";
        public const string SessionEnd = "game_session_end";
        public const string SessionHeartbeat = "session_heartbeat";
        public const string SessionAdsData = "session_ads_data";
        public const string LoadingStarted = "loading_started";
        public const string LoadingCompleted = "loading_completed";
        public const string PlayerLogin = "player_login";
        public const string PlayerAge = "player_age";
        public const string Error = "error_log";
        public const string Purchase = "purchase";
        public const string MainMenuEnter = "main_menu_enter";
        public const string LevelStart = "level_start";
        public const string LevelComplete = "level_complete";
        public const string GameOver = "game_over";
    }
}