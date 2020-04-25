using Commands;
using Logic;

namespace Services
{
	/// <summary>
	/// This service provides the possibility to execute a <see cref="IGameCommand"/>
	/// </summary>
	public interface ICommandService
	{
		/// <summary>
		/// Executes the <paramref name="command"/>
		/// The command execution is done atomically
		/// </summary>
		void ExecuteCommand<T>(T command) where T : IGameCommand;
	}
	
	/// <inheritdoc />
	public class CommandService : ICommandService
	{
		private readonly IGameLogic _gameLogic;
		
		public CommandService(IGameLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}
		
		/// <inheritdoc />
		public void ExecuteCommand<T>(T command) where T : IGameCommand
		{
			command.Execute(_gameLogic);
		}
	}
}