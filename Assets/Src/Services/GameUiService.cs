using System.Threading.Tasks;
using GameLovers.UiService;
using Game.Ids;
using Game.Presenters;

namespace Game.Services
{
	/// <inheritdoc />
	/// <remarks>
	/// Game custom implementation of the <see cref="IUiService"/>
	/// </remarks>
	public interface IGameUiService : IUiService
	{
		/// <summary>
		/// Loads asynchronously a set of <see cref="UiPresenter"/> defined by the given <paramref name="uiSetId"/>.
		/// It will update the <see cref="LoadingScreenPresenter"/> value based on the current loading status to a max
		/// value defined by the given <paramref name="loadingCap"/>
		/// </summary>
		Task LoadGameUiSet(UiSetId uiSetId, float loadingCap);
	}

	/// <inheritdoc cref="IGameUiService"/>
	public interface IGameUiServiceInit : IGameUiService, IUiServiceInit
	{
	}
	
	/// <inheritdoc cref="IGameUiService"/>
	public class GameUiService : UiService, IGameUiServiceInit
	{
		public GameUiService(IUiAssetLoader assetLoader) : base(assetLoader)
		{
		}
		
		/// <inheritdoc />
		public async Task LoadGameUiSet(UiSetId uiSetId, float loadingCap)
		{
			var loadingScreen = GetUi<LoadingScreenPresenter>();
			var tasks = LoadUiSetAsync((int) uiSetId);
			var initialLoadingPercentage = loadingScreen.LoadingPercentage;
			var loadingBuffer = tasks.Length / loadingCap - initialLoadingPercentage;
			var loadedUiCount = 0f;

			// Load all initial uis
			foreach (var taskTemplate in tasks)
			{
				await await taskTemplate;

				loadedUiCount++;

				loadingScreen.SetLoadingPercentage(initialLoadingPercentage + loadedUiCount / loadingBuffer);
			}

			loadingScreen.SetLoadingPercentage(loadingCap);
		}
	}
}