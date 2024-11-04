using System;

namespace Game.Data
{
	/// <summary>
	/// Contains all the data in the scope of the Game's App
	/// </summary>
	[Serializable]
	public class AppData
	{
		public enum QualityLevel
		{
			High,
			Medium,
			Low
		}

		public string DisplayName;
		public string PlayerId;
		public DateTime FirstLoginTime;
		public DateTime LastLoginTime;
		public DateTime LoginTime;
		public int SessionCount;
		public string Environment;
		public string DeviceId;
		public DateTime GameReviewDate;

		public bool HapticEnabled = true;
		public int FpsTarget = 30;
		public QualityLevel GraphicQuality = QualityLevel.Medium;

		public bool IsFirstSession => SessionCount <= 1;

		/// <summary>
		/// Copies base values for when user logs in to a new environment.
		/// We want to maintain a few settings across environments, those settings
		/// should be added to this copy method.
		/// </summary>
		public AppData CopyForNewEnvironment()
		{
			return new AppData
			{
				HapticEnabled = this.HapticEnabled,
				FpsTarget = this.FpsTarget,
				GraphicQuality = this.GraphicQuality,
			};
		}
	}
}