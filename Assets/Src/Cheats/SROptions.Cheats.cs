using System;
using System.ComponentModel;
using GameLovers.Services;
using Game.Ids;
using Game.Logic;
using Game.Logic.Server;
using Game.Services;
using UnityEngine;

public partial class SROptions
{
	[Category("Data")]
	public void ResetAllData()
	{
		PlayerPrefs.DeleteAll();
	}

	[Category("Currency")]
	public void Add100Sc()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProviderLocator>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddCurrency(GameId.SoftCurrency, 100);
	}

	[Category("Currency")]
	public void Add1000Sc()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProviderLocator>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddCurrency(GameId.SoftCurrency, 1000);
	}

	[Category("Currency")]
	public void Add100Hc()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProviderLocator>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddCurrency(GameId.HardCurrency, 100);
	}
		
	[Category("Time")]
	public void Add1Day()
	{
		var timeManipulator = MainInstaller.Resolve<IGameServicesLocator>().TimeService as ITimeManipulator;
		var timeNow = timeManipulator.DateTimeUtcNow;
			
		timeManipulator.AddTime((float) (timeNow.AddDays(1) - timeNow).TotalSeconds);
	}
		
	[Category("Time")]
	public void Add1Hour()
	{
		var timeManipulator = MainInstaller.Resolve<IGameServicesLocator>().TimeService as ITimeManipulator;
		var timeNow = timeManipulator.DateTimeUtcNow;
			
		timeManipulator.AddTime((float) (timeNow.AddHours(1) - timeNow).TotalSeconds);
	}
		
	[Category("Time")]
	public void Add1Minute()
	{
		var timeManipulator = MainInstaller.Resolve<IGameServicesLocator>().TimeService as ITimeManipulator;
		var timeNow = timeManipulator.DateTimeUtcNow;
			
		timeManipulator.AddTime((float) (timeNow.AddHours(1) - timeNow).TotalSeconds);
	}
}