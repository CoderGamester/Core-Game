using GameLovers.Services;
using Logic;

namespace Commands
{
	/// <summary>
	/// Command interface implementation for this game
	/// </summary>
	public interface IGameCommand : IGameCommand<IGameLogic>
	{
	}
}