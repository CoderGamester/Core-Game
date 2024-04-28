using Game.Ids;
using UnityEngine;

namespace Game.MonoComponent
{
	/// <summary>
	/// Simple Mono Component containing the object's <seealso cref="UniqueId"/>
	/// </summary>
	public class EntityMonoComponent : MonoBehaviour
	{
		/// <summary>
		/// This entity's <seealso cref="UniqueId"/>
		/// </summary>
		public UniqueId UniqueId;
	}
}