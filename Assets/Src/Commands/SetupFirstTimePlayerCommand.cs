﻿using Game.Logic;
using GameLovers.Services;

namespace Game.Commands
{
	/// <summary>
	/// This command is responsible to handle the logic when the game is opened for the first time and needs to setup the player data
	/// </summary>
	public struct SetupFirstTimePlayerCommand : IGameCommand<IGameLogicLocator>
	{
		/// <inheritdoc />
		public void Execute(IGameLogicLocator gameLogic, IMessageBrokerService messageBroker)
		{
			// Setup Game data in logic
		}
	}
}
