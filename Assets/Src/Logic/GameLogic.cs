using GameLovers.GoogleSheetImporter;
using GameLovers.Services;

namespace Logic
{
	/// <summary>
	/// This interface marks the Game Logic as one that needs to initialize it's internal state
	/// </summary>
	public interface IGameLogicInitializer
	{
		/// <summary>
		/// Initializes the Game Logic state to it's default initial values
		/// </summary>
		void Init();
	}
	
	/// <summary>
	/// Provides access to all game's data.
	/// This interface provides the data with view only permissions
	/// </summary>
	public interface IGameDataProvider
	{
		/// <inheritdoc cref="IConfigsProvider"/>
		IConfigsProvider ConfigsProvider { get; }
		
		/// <inheritdoc cref="IAppDataProvider"/>
		IAppDataProvider AppDataProvider { get; }
		/// <inheritdoc cref="IGameIdDataProvider"/>
		IGameIdDataProvider GameIdDataProvider { get; }
		/// <inheritdoc cref="ICurrencyDataProvider"/>
		ICurrencyDataProvider CurrencyDataProvider { get; }
	}

	/// <summary>
	/// Provides access to all game's logic
	/// This interface shouldn't be exposed to the views or controllers
	/// To interact with the logic, execute a <see cref="Commands.IGameCommand"/> via the <see cref="ICommandService"/>
	/// </summary>
	public interface IGameLogic : IGameDataProvider
	{
		/// <inheritdoc cref="IMessageBrokerService"/>
		IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc cref="ITimeService"/>
		ITimeService TimeService { get; }
		
		/// <inheritdoc cref="IAppLogic"/>
		IAppLogic AppLogic { get; }
		/// <inheritdoc cref="IGameIdLogic"/>
		IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc cref="ICurrencyLogic"/>
		ICurrencyLogic CurrencyLogic { get; }
	}

	/// <inheritdoc cref="IGameLogic"/>
	public interface IGameLogicInit : IGameLogic, IGameLogicInitializer
	{
	}

	/// <inheritdoc cref="IGameLogic"/>
	public class GameLogic : IGameLogicInit
	{
		/// <inheritdoc />
		public IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc />
		public ITimeService TimeService { get; }

		/// <inheritdoc />
		public IConfigsProvider ConfigsProvider { get; }

		/// <inheritdoc />
		public IAppDataProvider AppDataProvider => AppLogic;
		/// <inheritdoc />
		public IGameIdDataProvider GameIdDataProvider => GameIdLogic;
		/// <inheritdoc />
		public ICurrencyDataProvider CurrencyDataProvider => CurrencyLogic;
		
		/// <inheritdoc />
		public IAppLogic AppLogic { get; }
		/// <inheritdoc />
		public IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc />
		public ICurrencyLogic CurrencyLogic { get; }

		public GameLogic(IMessageBrokerService messageBroker, ITimeService timeService, IDataProvider dataProvider,
		                 IConfigsProvider configsProvider)
		{
			MessageBrokerService = messageBroker;
			TimeService = timeService;
			ConfigsProvider = configsProvider;
			
			AppLogic = new AppLogic(this, dataProvider);
			CurrencyLogic = new CurrencyLogic(this, dataProvider);
			GameIdLogic = new GameIdLogic(this, dataProvider);
		}

		/// <inheritdoc />
		public void Init()
		{
			// ReSharper disable PossibleNullReferenceException
			
			(CurrencyLogic as IGameLogicInitializer).Init();
			(GameIdLogic as IGameLogicInitializer).Init();
		}
	}
}