using Logic;
using Services;

namespace Commands
{
	/// <summary>
	/// Interface representing the command to be executed in the <see cref="ICommandService"/>
	/// Implement this interface with the proper command logic
	/// </summary>
	/// <remarks>
	/// Follows the Command pattern <see cref="https://en.wikipedia.org/wiki/Command_pattern"/>
	/// </remarks>
	public interface IGameCommand
	{
		/// <summary>
		/// Executes the command logic
		/// </summary>
		void Execute(IGameLogic gameLogic);
	}
}