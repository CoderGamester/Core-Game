using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Game.Services
{
	/// <summary>
	/// This service is responsible for providing access to the important GameObjects that are used across the entire game
	/// </summary>
	public interface IWorldObjectReferenceService
	{
		/// <summary>
		/// The InputSystem UI module that is used across the game in all scenes
		/// </summary>
		InputSystemUIInputModule InputSystem { get; }
		
		/// <summary>
		/// The main camera instance that is used across the game in all scenes
		/// </summary>
		Camera MainCamera { get; }
	}
	
	/// <inheritdoc />
	public class WorldObjectReferenceService : IWorldObjectReferenceService
	{
		/// <inheritdoc />
		public InputSystemUIInputModule InputSystem { get; }
		/// <inheritdoc />
		public Camera MainCamera { get; }
		
		public WorldObjectReferenceService(InputSystemUIInputModule inputSystem, Camera mainCamera)
		{
			InputSystem = inputSystem;
			MainCamera = mainCamera;
		}
	}
}