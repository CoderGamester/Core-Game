using GameLovers;
using GameLovers.Services;

namespace Game.Logic.Shared
{
	/// <summary>
	/// This logic provides the necessary behaviour to peak the next Random Number Generator
	/// </summary>
	public interface IRngDataProvider
	{
		/// <inheritdoc cref="IRngService.Data"/>
		public IRngData Data { get; }

		/// <inheritdoc cref="IRngService.Counter"/>
		int Counter { get; }

		/// <inheritdoc cref="IRngService.Peek"/>
		int Peek { get; }

		/// <inheritdoc cref="IRngService.Peekfloat"/>
		floatP Peekfloat { get; }

		/// <inheritdoc cref="IRngService.PeekRange(int, int, bool)"/>
		int PeekRange(int min, int max, bool maxInclusive = false);

		/// <inheritdoc cref="IRngService.PeekRange(float, float, bool)"/>
		floatP PeekRange(floatP min, floatP max, bool maxInclusive = true);
	}

	/// <inheritdoc cref="IRngService"/>
	public interface IRngLogic : IRngService, IRngDataProvider { }

	/// <inheritdoc cref="RngService"/>
	public class RngLogic : RngService, IRngLogic
	{
		public RngLogic(IDataProvider dataProvider) : base(dataProvider.GetData<RngData>())
		{
		}

	}
}
