using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game
{
	/// <summary>
	/// The first entry object that shows the splash screen and boots the <see cref="Main"/> scene reference
	/// </summary>
	public class BootSplashscreen : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;

		private IEnumerator Start()
		{
			var asyncOperation = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

			if (_audioSource.clip != null)
			{
				_audioSource.Play();
			}
			
#if UNITY_PRO_LICENSE
			SplashScreen.Begin();
			SplashScreen.Draw();
			
			while (!SplashScreen.isFinished)
			{
				SplashScreen.Draw();
				
				yield return null;
			}
#endif

			while (!asyncOperation.isDone)
			{
				yield return null;
			}

			SceneManager.MergeScenes(SceneManager.GetSceneByName("Boot"), SceneManager.GetSceneByName("Main"));
			FindObjectsOfType<AudioListener>().All(audioSource => audioSource.enabled = true);
			Destroy(gameObject);
		}
	}
}