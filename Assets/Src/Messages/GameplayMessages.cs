using GameLovers.Services;

namespace Game.Messages
{
	public struct OnGameInitMessage : IMessage { }
	public struct OnGameOverMessage : IMessage { }
	public struct OnGameCompleteMessage : IMessage { }
	public struct OnGameRestartMessage : IMessage { }
}
