using System;

namespace Game.Logic.Server
{
	/// <summary>
	/// Exception to be used in any of the logic's <see cref="GameLogicLocator"/>
	/// </summary>
	[Serializable]
	public class LogicException : Exception
	{
		public LogicException(string message) : base(message)
		{
		}
 
		public LogicException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}