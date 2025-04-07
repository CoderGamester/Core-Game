using Game.Messages;
using Game.Services.Analytics;
using GameLovers.Services;
using UnityEngine.Device;

namespace Game.Services
{
	/// <summary>
	/// The analytics service is an endpoint in the game to log custom events to Game's analytics console
	/// </summary>
	public interface IAnalyticsService
	{
		/// <inheritdoc cref="AnalyticsSession" />
		AnalyticsSession SessionCalls { get; }
		/// <inheritdoc cref="AnalyticsEconomy" />
		AnalyticsEconomy EconomyCalls { get; }
		/// <inheritdoc cref="AnalyticsErrors" />
		AnalyticsErrors ErrorsCalls { get; }

		/// <inheritdoc cref="AnalyticsMainMenu" />
		AnalyticsMainMenu MainMenuCalls { get; }
		/// <inheritdoc cref="AnalyticsGameplay" />
		AnalyticsGameplay GameplayCalls { get; }

		/// <summary>
		/// Flushes all the queued analytics events
		/// </summary>
		void FlushEvents();
	}

	/// <inheritdoc cref="IAnalyticsService" />
	public class AnalyticsService : IAnalyticsService, IGameServicesInitializer
	{
		/// <inheritdoc />
		public AnalyticsSession SessionCalls { get; }
		/// <inheritdoc />
		public AnalyticsEconomy EconomyCalls { get; }
		/// <inheritdoc />
		public AnalyticsErrors ErrorsCalls { get; }
		/// <inheritdoc />
		public AnalyticsMainMenu MainMenuCalls { get; }
		/// <inheritdoc />
		public AnalyticsGameplay GameplayCalls { get; }

		public AnalyticsService(IMessageBrokerService messageBrokerService, IDataProvider dataProvider)
		{
			SessionCalls = new AnalyticsSession(this, dataProvider);
			EconomyCalls = new AnalyticsEconomy(this);
			ErrorsCalls = new AnalyticsErrors(this);
			MainMenuCalls = new AnalyticsMainMenu(this);
			GameplayCalls = new AnalyticsGameplay(this);
			
			messageBrokerService.Subscribe<ApplicationComplianceAcceptedMessage>(OnApplicationComplianceAcceptedMessage);
		}

		/// <inheritdoc />
		public void Init()
		{
			Unity.Services.Analytics.AnalyticsService.Instance.StartDataCollection();
			SessionCalls.SessionStart();
			// TODO: Use this call when user actively logs in the backend
			//SessionCalls.PlayerLogin(SystemInfo.deviceUniqueIdentifier);
		}
		
		/// <inheritdoc />
		public void FlushEvents()
		{
			Unity.Services.Analytics.AnalyticsService.Instance.Flush();
		}

		private void OnApplicationComplianceAcceptedMessage(ApplicationComplianceAcceptedMessage message)
		{
			SessionCalls.PlayerAge(message.Age);
		}
	}
}
