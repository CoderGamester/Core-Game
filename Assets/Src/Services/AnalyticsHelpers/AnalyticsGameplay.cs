using System.Collections.Generic;
using UnityEngine;

namespace Game.Services.Analytics
{
    /// <summary>
    /// This class provides the necessary behaviour to manage the analytics endpoints for the game's gameplay levels
    /// </summary>
    public class AnalyticsGameplay : AnalyticsBase
    {
        private float _levelStartTime;

        public AnalyticsGameplay(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }
		
        /// <summary>
        /// Logs the event when it's game over
        /// </summary>
		public void GameOver()
		{
            LogEvent(AnalyticsEvents.GameOver);
		}

        /// <summary>
        /// Logs the event when the player enters a new level
        /// </summary>
        public void LevelStart(int level)
        {
            _levelStartTime = Time.realtimeSinceStartup;
            
            var data = new Dictionary<string, object>
            {
                {"level", level},
            };
            
            LogEvent(AnalyticsEvents.LevelStart, data);
        }
        
        /// <summary>
        /// Logs the event when the player exits the current level
        /// </summary>
        public void LevelComplete(int level)
        {
            var data = new Dictionary<string, object>
            {
                {"level", level},
                {"completion_time", Time.realtimeSinceStartup - _levelStartTime},
            };
            
            LogEvent(AnalyticsEvents.LevelComplete, data);
        }
    }
}
