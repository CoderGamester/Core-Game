using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Game.Services
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IWorldObjectReferenceService
	{
		/// <summary>
		/// TODO:
		/// </summary>
		InputSystemUIInputModule InputSystem { get; }
		
		/// <summary>
		/// TODO:
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