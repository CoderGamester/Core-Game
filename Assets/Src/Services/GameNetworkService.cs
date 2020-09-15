using System.Collections.Generic;
using GameLovers.Services;

namespace Services
{
	/// <inheritdoc />
	public class GameNetworkService : NetworkService
	{
		/// <inheritdoc />
		protected override void SendMessageRequest(string name, IDictionary<string, object> payload)
		{
			// TODO:
		}
	}
}