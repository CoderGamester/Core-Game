using GameLovers.ConfigsProvider;
using GameLovers.Services;
using Game.Logic.Server;
using Game.Logic.Client;
using Game.Services;

namespace Game.Logic
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
	public interface IGameDataProviderLocator
	{
		/// <inheritdoc cref="IAppDataProvider"/>
		IAppDataProvider AppDataProvider { get; }
		/// <inheritdoc cref="IRngDataProvider"/>
		IRngDataProvider RngDataProvider { get; }
		/// <inheritdoc cref="IEntityFactoryDataProvider"/>
		IEntityFactoryDataProvider EntityFactoryDataProvider { get; }
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
	public interface IGameLogicLocator : IGameDataProviderLocator
	{
		/// <inheritdoc cref="IAppLogic"/>
		IAppLogic AppLogic { get; }
		/// <inheritdoc cref="IRngLogic"/>
		IRngLogic RngLogic { get; }
		/// <inheritdoc cref="IEntityFactoryLogic"/>
		IEntityFactoryLogic EntityFactoryLogic { get; }
		/// <inheritdoc cref="IGameIdLogic"/>
		IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc cref="ICurrencyLogic"/>
		ICurrencyLogic CurrencyLogic { get; }
	}

	/// <summary>
	/// This interface provides the contract to initialize the Game Logic
	/// </summary>
	public interface IGameLogicLocatorInit : IGameLogicLocator
	{
		/// <summary>
		/// Initializes the Game Logic state to it's default initial values
		/// </summary>
		void Init(IDataService dataService, IGameServicesLocator gameServices);
	}

	/// <inheritdoc cref="IGameLogicLocator"/>
	public class GameLogicLocator : IGameLogicLocatorInit
	{
		/// <inheritdoc />
		public IAppDataProvider AppDataProvider => AppLogic;
		/// <inheritdoc />
		public IRngDataProvider RngDataProvider => RngLogic;
		/// <inheritdoc />
		public IEntityFactoryDataProvider EntityFactoryDataProvider => EntityFactoryLogic;
		/// <inheritdoc />
		public IGameIdDataProvider GameIdDataProvider => GameIdLogic;
		/// <inheritdoc />
		public ICurrencyDataProvider CurrencyDataProvider => CurrencyLogic;
		
		/// <inheritdoc />
		public IAppLogic AppLogic { get; }
		/// <inheritdoc />
		public IRngLogic RngLogic { get; private set; }
		/// <inheritdoc />
		public IEntityFactoryLogic EntityFactoryLogic { get; }
		/// <inheritdoc />
		public IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc />
		public ICurrencyLogic CurrencyLogic { get; }

		public GameLogicLocator(IInstaller installer)
		{
			var configsProvider = installer.Resolve<IConfigsProvider>();
			var dataService = installer.Resolve<IDataService>();
			var timeService = installer.Resolve<ITimeService>();

			AppLogic = new AppLogic(configsProvider, dataService, timeService);
			EntityFactoryLogic = new EntityFactoryLogic(this, configsProvider, dataService, timeService);
			CurrencyLogic = new CurrencyLogic(configsProvider, dataService, timeService);
			GameIdLogic = new GameIdLogic(configsProvider, dataService, timeService);
		}

		/// <inheritdoc />
		public void Init(IDataService dataService, IGameServicesLocator gameServices)
		{
			// IMPORTANT: Order of execution is very important in this method

			RngLogic = new RngLogic(dataService);

			// ReSharper disable PossibleNullReferenceException
			(CurrencyLogic as IGameLogicInitializer).Init();
			(GameIdLogic as IGameLogicInitializer).Init();
		}
	}
}