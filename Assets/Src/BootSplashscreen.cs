using Game.Utils;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	/// <summary>
	/// The first entry object that shows the splash screen and boots the <see cref="Main"/> scene reference
	/// </summary>
	public class BootSplashScreen : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;

		private IEnumerator Start()
		{
			var asyncOperation = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN, LoadSceneMode.Additive);

			if (_audioSource.clip != null)
			{
				_audioSource.Play();
			}

#if UNITY_PRO_LICENSE
			UnityEngine.Rendering.SplashScreen.Begin();
			UnityEngine.Rendering.SplashScreen.Draw();
			
			while (!UnityEngine.Rendering.SplashScreen.isFinished)
			{
				UnityEngine.Rendering.SplashScreen.Draw();
				
				yield return null;
			}
#endif

			while (!asyncOperation.isDone)
			{
				yield return null;
			}

			SceneManager.MergeScenes(SceneManager.GetSceneByName(Constants.Scenes.BOOT), SceneManager.GetSceneByName(Constants.Scenes.MAIN));
			FindObjectsOfType<AudioListener>().All(audioSource => audioSource.enabled = true);
			Destroy(gameObject);
		}
	}
}