using GameLovers.Services;

namespace Events
{
	public struct ApplicationPausedEvent : IMessage
	{
		public bool IsPaused;
	}
}