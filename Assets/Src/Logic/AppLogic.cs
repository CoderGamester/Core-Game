using System;
using Data;
using GameLovers.Services;

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
		
		/// <summary>
		/// Marks the date when the game was last time reviewed
		/// </summary>
		void MarkGameAsReviewed();
	}
	
	/// <inheritdoc />
	public interface IAppLogic : IAppDataProvider
	{
	}
	
	/// <inheritdoc cref="IAppLogic"/>
	public class AppLogic : AbstractBaseLogic<AppData>, IAppLogic
	{
		private readonly DateTime _defaultZeroTime = new DateTime(2020, 1, 1);

		/// <inheritdoc />
		public bool IsFirstSession => Data.IsFirstSession;

		/// <inheritdoc />
		public bool IsGameReviewed => LocalData.GameReviewDate > _defaultZeroTime;

		private LocalData LocalData => DataProvider.GetData<LocalData>();

		public AppLogic(IGameLogic gameLogic, IDataProvider dataProvider) : base(gameLogic, dataProvider)
		{
		}
		
		/// <inheritdoc />
		public void MarkGameAsReviewed()
		{
			if (IsGameReviewed)
			{
				throw new LogicException("The game was already reviewed and cannot be reviewed again");
			}
			
			LocalData.GameReviewDate = GameLogic.TimeService.DateTimeUtcNow;
		}
	}
}