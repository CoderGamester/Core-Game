using GameLovers.Services;

namespace Game.Messages
{
	public struct ApplicationQuitMessage : IMessage { }
	public struct ApplicationPausedMessage : IMessage { public bool IsPaused; }
	public struct ApplicationComplianceAcceptedMessage : IMessage { public int Age; }
}