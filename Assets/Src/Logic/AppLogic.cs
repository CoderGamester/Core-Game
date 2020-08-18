using System;
using Data;

namespace Logic
{
	/// <summary>
	/// This logic provides the necessary behaviour to manage the game's app
	/// </summary>
	public interface IAppDataProvider
	{
		/// <summary>
		/// Requests the information if the current game session is the first time the player is playing the game or not
		/// </summary>
		bool IsFirstSession { get; }
		
		/// <summary>
		/// Requests the information if the game was or not yet reviewed
		/// </summary>
		bool IsGameReviewed { get; }
	}
	
	/// <inheritdoc />
	public interface IAppLogic : IAppDataProvider
	{
		/// <summary>
		/// Marks the game as already reviewed
		/// </summary>
		void MarkGameAsReviewed();
	}
	
	/// <inheritdoc />
	public class AppLogic : IAppLogic
	{
		private readonly IGameLogic _gameLogic;
		private readonly AppData _data;

		/// <inheritdoc />
		public bool IsFirstSession => _data.IsFirstSession;

		/// <inheritdoc />
		public bool IsGameReviewed => _data.GameReviewDate > new DateTime(2020, 1, 1);

		private AppLogic() {}

		public AppLogic(IGameLogic gameLogic, AppData appData)
		{
			_gameLogic = gameLogic;
			_data = appData;
		}
		
		/// <inheritdoc />
		public void MarkGameAsReviewed()
		{
			if (IsGameReviewed)
			{
				throw new LogicException("The game was already reviewed and cannot be reviewed again");
			}
			
			_data.GameReviewDate = _gameLogic.TimeService.DateTimeUtcNow;
		}
	}
}