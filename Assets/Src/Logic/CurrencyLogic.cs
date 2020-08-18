using System;
using Data;
using Events;
using GameLovers;
using Ids;

namespace Logic
{
	/// <summary>
	/// This logic provides the necessary behaviour to manage the player's currency
	/// </summary>
	public interface ICurrencyDataProvider
	{
		/// <summary>
		/// Requests the player's current <seealso cref="GameId.SoftCurrency"/> amount
		/// </summary>
		int SoftCurrencyAmount { get; }
		/// <summary>
		/// Requests the player's current <seealso cref="GameId.HardCurrency"/> amount
		/// </summary>
		int HardCurrencyAmount { get; }
		/// <summary>
		/// Requests the player's <seealso cref="GameIdGroup.Currency"/> <see cref="IObservableDictionary{TKey,TValue}"/>
		/// </summary>
		IObservableDictionaryReader<GameId, int> Currencies { get; }
	}

	/// <inheritdoc />
	public interface ICurrencyLogic : ICurrencyDataProvider
	{
		/// <summary>
		/// Adds the given <paramref name="amount"/> to the current <paramref name="currency"/> wallet amount
		/// </summary>
		/// <exception cref="LogicException">
		/// Thrown when the given <paramref name="currency"/> is not part of the <seealso cref="GameIdGroup.Currency"/> group
		/// </exception>
		void AddCurrency(GameId currency, int amount);
		
		/// <summary>
		/// Deducts the given <paramref name="amount"/> from the current <paramref name="currency"/> wallet amount
		/// </summary>
		/// <exception cref="LogicException">
		/// Thrown when the given <paramref name="currency"/> is not part of the <seealso cref="GameIdGroup.Currency"/> group
		/// or if the given <paramref name="amount"/> is higher than the current amount in the player's wallet
		/// </exception>
		void DeductCurrency(GameId currency, int amount);
	}
	
	/// <inheritdoc />
	public class CurrencyLogic : ICurrencyLogic
	{
		private readonly IGameLogic _gameLogic;
		private readonly IObservableDictionary<GameId, int> _currencies;

		/// <inheritdoc />
		public int SoftCurrencyAmount => _currencies[GameId.SoftCurrency];
		/// <inheritdoc />
		public int HardCurrencyAmount => _currencies[GameId.HardCurrency];
		/// <inheritdoc />
		public IObservableDictionaryReader<GameId, int> Currencies => _currencies;

		private CurrencyLogic() {}

		public CurrencyLogic(IGameLogic gameLogic, PlayerData playerData)
		{
			_gameLogic = gameLogic;
			_currencies = new ObservableDictionary<GameId, int>(playerData.Currencies);
		}

		/// <inheritdoc />
		public void AddCurrency(GameId currency, int amount)
		{
			if (!currency.IsInGroup(GameIdGroup.Currency))
			{
				throw new LogicException($"The given game Id {currency} is not of {GameIdGroup.Currency} type");
			}
			
			var oldAmount = _currencies[currency];
			var newAmount = oldAmount + amount;
			
			_currencies[currency] = newAmount;

			PublishCurrencyEvent(currency, oldAmount, newAmount);
		}

		/// <inheritdoc />
		public void DeductCurrency(GameId currency, int amount)
		{
			if (!currency.IsInGroup(GameIdGroup.Currency))
			{
				throw new LogicException($"The given game Id {currency} is not of {GameIdGroup.Currency} type");
			}
			
			var oldAmount = _currencies[currency];
			var newAmount = oldAmount + amount;
			
			if (oldAmount - amount < 0)
			{
				throw new LogicException($"The player needs {amount.ToString()} of {currency} type and only has " +
				                                    $"{oldAmount.ToString()}");
			}
			
			_currencies[currency] = newAmount;

			PublishCurrencyEvent(currency, oldAmount, newAmount);
		}

		private void PublishCurrencyEvent(GameId currency, int oldAmount, int newAmount)
		{
			_gameLogic.MessageBrokerService.Publish(new CurrencyValueChangedEvent
			{
				Currency = currency,
				OldValue = oldAmount,
				NewValue = newAmount
			});
		}
	}
}