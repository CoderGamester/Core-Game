using Game.Logic;
using GameLovers.Services;

namespace Game.Commands
{
	/// <summary>
	/// This command is responsible to handle the logic when the game is restarted
	/// </summary>
	public struct RestartGameCommand : IGameCommand<IGameLogicLocator>
	{
		/// <inheritdoc />
		/// <inheritdoc />
		public void Execute(IGameLogicLocator gameLogic, IMessageBrokerService messageBroker)
		{
			// Restart the Game data
		}
	}
}