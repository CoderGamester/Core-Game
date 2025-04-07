using System;
using System.Collections.Generic;
using Game.Data;
using GameLovers;
using GameLovers.Services;
using UnityEngine;
using UnityEngine.Analytics;

namespace Game.Services.Analytics
{
	/// <summary>
	/// This class provides the necessary behaviour to manage the analytics endpoints of the app's session calls
	/// </summary>
	public class AnalyticsSession : AnalyticsBase
	{
		private float _loadingStarted;
		private IDataProvider _dataProvider;

		private static bool IsTablet
		{
			get
			{
#if UNITY_IOS
				return SystemInfo.deviceModel.Contains("iPad");
#elif UNITY_ANDROID
				var screenWidth = Screen.width;
				var screenHeight = Screen.height;
				var screenWidthDpi = screenWidth / Screen.dpi;
				var screenHeightDpi = screenHeight / Screen.dpi;
				var diagonalInchesSqrt =Mathf.Pow (screenWidthDpi, 2) + Mathf.Pow (screenHeightDpi, 2);
				var aspectRatio = Mathf.Max(screenWidth, screenHeight) / Mathf.Min(screenWidth, screenHeight);

				// This double checks the physical size device with screen aspect ratio
				return diagonalInchesSqrt > 42f && aspectRatio < 2f;
#else
				return false;
#endif
			}
		}

		private static Dictionary<string, object> StartData => new Dictionary<string, object>
		{
			{ "client_version", VersionServices.VersionInternal },
			{ "platform", Application.platform.ToString() },
			{ "device", SystemInfo.deviceModel },
			{ "tablet", IsTablet },
			{ "os", SystemInfo.operatingSystem },
			{ "language", Application.systemLanguage.ToString() },
#if UNITY_WEBGL && !UNITY_EDITOR
			{ "url_source", new Uri(Application.absoluteURL).Host },
#elif UNITY_IOS
			{ "ios_att_enabled", UnityEngine.iOS.Device.advertisingTrackingEnabled},
#endif
		};

		public AnalyticsSession(IAnalyticsService analyticsService, IDataProvider dataProvider) : base(analyticsService)
		{
			_dataProvider = dataProvider;
		}

		/// <summary>
		/// Sends the mark of starting a game session
		/// </summary>
		public void SessionStart()
		{
			var appData = _dataProvider.GetData<AppData>();
			var loginData = StartData;
			
			loginData.Add("session_count", appData.SessionCount);
			loginData.Add("days_since_install", (DateTime.UtcNow - appData.FirstLoginTime).Days);
			
			LogEvent(AnalyticsEvents.SessionStart, loginData);
		}

		/// <summary>
		/// Sends the mark of ending a game session
		/// </summary>
		public void SessionEnd(string reason)
		{
			var dic = new Dictionary<string, object> 
			{
				{"reason", reason}
			};
			LogEvent(AnalyticsEvents.SessionEnd, dic);
		}

		/// <summary>
		/// Sends a heartbeat analytics event
		/// </summary>
		public void Heartbeat()
		{
			LogEvent(AnalyticsEvents.SessionHeartbeat, null);
		}

		/// <summary>
		/// Logs the data required to configure ads tracking points
		/// </summary>
		public void AdsData()
		{
			// Async call for the AdvertisingId
			var requestAdvertisingIdSuccess = Application.RequestAdvertisingIdentifierAsync((id, enabled, msg) =>
			{
				var dic = new Dictionary<string, object>
				{
					{"client_version", VersionServices.VersionInternal},
					{"advertising_id", id},
					{"boot_time", Time.realtimeSinceStartup},
					{"advertising_tracking_enabled", enabled },
					{"vendor_id", SystemInfo.deviceUniqueIdentifier},
					{"session_id", AnalyticsSessionInfo.sessionId }
				};
				LogEvent(AnalyticsEvents.SessionAdsData, dic);
			});
			
			// If the async call fails we try another way
			if (!requestAdvertisingIdSuccess)
			{
				var dic = new Dictionary<string, object>
				{
					{"client_version", VersionServices.VersionInternal},
#if UNITY_IOS
					{"ios_att_enabled", UnityEngine.iOS.Device.advertisingTrackingEnabled},
#elif UNITY_ANDROID && !UNITY_EDITOR
					{"advertising_id", GetAndroidAdvertiserId()},
#endif
					{"vendor_id", SystemInfo.deviceUniqueIdentifier},
					{"session_id", AnalyticsSessionInfo.sessionId }
				};
				LogEvent(AnalyticsEvents.SessionAdsData, dic);
			}

			_loadingStarted = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Logs the start of loading the given <paramref name="scene"/>
		/// </summary>
		public void LoadingStarted(string scene)
		{
			_loadingStarted = Time.realtimeSinceStartup;

			var data = new Dictionary<string, object>
			{
				{"boot_time", Time.realtimeSinceStartup},
				{"scene", scene},
			};

			LogEvent(AnalyticsEvents.LoadingStarted, data);
		}

		/// <summary>
		/// Logs the end of the given <paramref name="scene"/>
		/// </summary>
		public void LoadingCompleted(string scene)
		{
			var loadingTime = Time.realtimeSinceStartup - _loadingStarted;
			var data = new Dictionary<string, object>
			{
				{"boot_time", Time.realtimeSinceStartup},
				{"loading_time", loadingTime},
				{"scene", scene},
			};

			_loadingStarted = 0;

			LogEvent(AnalyticsEvents.LoadingCompleted, data);
		}

		/// <summary>
		/// Logs the first login Event with the given user <paramref name="id"/>
		/// </summary>
		public void PlayerLogin(string id)
		{
			UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("player_id", id);

			var loginData = StartData;
			
			loginData.Add("player_id", id);
			
			LogEvent(AnalyticsEvents.PlayerLogin, loginData);
		}

		/// <summary>
		/// Logs the player age in aggregated analytics for player segmentation
		/// </summary>
		public void PlayerAge(int age)
		{
			var data = new Dictionary<string, object>
			{
				{"age", age },
			};
			
			LogEvent(AnalyticsEvents.PlayerAge, data);
		}

#if UNITY_ANDROID
		private static string GetAndroidAdvertiserId()
		{
			string advertisingID = "";
			// TODO: Enable when Google SDK is connected or another code supports it
			// Error Unity Error acquiring Android AdvertiserId - java.lang.ClassNotFoundException: com.google.android.gms.ads.identifier.AdvertisingIdClient
			/*try
			{
				AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
				AndroidJavaClass client = new AndroidJavaClass ("com.google.android.gms.ads.identifier.AdvertisingIdClient");
				AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject> ("getAdvertisingIdInfo", currentActivity);
     
				advertisingID = adInfo.Call<string> ("getId").ToString();
			}
			catch (Exception ex)
			{
				Debug.LogError("Error acquiring Android AdvertiserId - "+ex.Message);
			}*/
			return advertisingID;
		}
#endif
	}
}