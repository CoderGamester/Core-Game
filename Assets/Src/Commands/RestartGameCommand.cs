using Game.Logic;
using GameLovers.Services;

namespace Game.Commands
{
	/// <summary>
	/// This command is responsible to handle the logic when the game is restarted
	/// </summary>
	public struct RestartGameCommand : IGameCommand<IGameLogic>
	{
		/// <inheritdoc />
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic, IMessageBrokerService messageBroker)
		{
			// Restart the Game data
		}
	}
}