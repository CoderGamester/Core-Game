using GameLovers.Services;
using Game.Logic;

namespace Game.Commands
{
	/// <summary>
	/// Command interface implementation for this game
	/// </summary>
	public interface IGameCommand : IGameCommand<IGameLogic>
	{
	}
}