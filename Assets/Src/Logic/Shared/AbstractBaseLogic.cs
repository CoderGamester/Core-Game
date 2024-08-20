using GameLovers.ConfigsProvider;
using GameLovers.Services;

namespace Game.Logic.Shared
{
	/// <summary>
	/// Abstract basic signature for any Logic that is part of the <see cref="IGameLogic"/>
	/// </summary>
	public abstract class AbstractBaseLogic<TData> where TData : class
	{
		protected readonly IConfigsProvider ConfigsProvider;
		protected readonly IDataService DataService;
		protected readonly ITimeService TimeService;

		protected TData Data => DataService.GetData<TData>();

		private AbstractBaseLogic() { }

		public AbstractBaseLogic(IConfigsProvider configsProvider, IDataService dataService, ITimeService timeService)
		{
			ConfigsProvider = configsProvider;
			DataService = dataService;
			TimeService = timeService;
		}
	}
}