using System.Collections.Generic;

namespace Game.Services.Analytics
{
	/// <summary>
	/// This class provides the necessary behaviour to manage the analytics endpoints of the app's $$ transaction calls or moments
	/// </summary>
	public class AnalyticsEconomy : AnalyticsBase
	{
		public AnalyticsEconomy(IAnalyticsService analyticsService) : base(analyticsService)
		{
		}

		/// <summary>
		/// Logs when the user purchases a product
		/// </summary>
		/// <remarks>
		/// In Unity IAP package
		/// - itemId = product.definition.id
		/// - itemName = product.metadata.localizedTitle
		/// </remarks>
		public void Purchase(string transactionID, string itemId, string itemName, float price, float netIncomeModifier)
		{
			var data = new Dictionary<string, object>
			{
				{"currency", "USD"},
				{"transaction_id", transactionID},
				{"price", price},
				{"dollar_gross", price},
				{"dollar_net", price * netIncomeModifier},
				{"item_id", itemId},
				{"item_name", itemName}
			};
			
			LogEvent(AnalyticsEvents.Purchase, data);
		}
	}
}
