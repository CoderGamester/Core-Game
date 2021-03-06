using System;
using Ids;

namespace Data
{
	/// <summary>
	/// Contains all the data in the scope of the Game's App
	/// </summary>
	[Serializable]
	public class AppData
	{
		public DateTime FirstLoginTime;
		public DateTime LastLoginTime;
		public DateTime LoginTime;
		public ulong LoginCount;
		public UniqueId UniqueIdCounter = UniqueId.Invalid;
		
		/// <summary>
		/// Requests the info if this is the first time the player open the game in it's lifetime
		/// </summary>
		public bool IsFirstSession => LoginCount <= 1;
	}
}