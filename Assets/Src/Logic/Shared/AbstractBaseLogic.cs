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
		protected readonly IDataProvider DataProvider;
		protected readonly ITimeService TimeService;

		protected TData Data => DataProvider.GetData<TData>();

		private AbstractBaseLogic() { }

		public AbstractBaseLogic(IConfigsProvider configsProvider, IDataProvider dataProvider, ITimeService timeService)
		{
			ConfigsProvider = configsProvider;
			DataProvider = dataProvider;
			TimeService = timeService;
		}
	}
}