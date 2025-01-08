using UnityEngine;

namespace Game.ViewControllers
{
    /// <summary>
    /// Base class for all View Controllers in the project.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ViewControllerBase : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        /// <summary>
        /// The RectTransform component attached to this GameObject.
        /// </summary>
        public RectTransform RectTransform => _rectTransform;

        private void OnValidate()
        {
            _rectTransform = _rectTransform != null ? _rectTransform : GetComponent<RectTransform>();

            OnEditorValidate();
        }
        
        /// <summary>
        /// Called only in the Editor. Called in the end of this object MonoBehaviour's OnValidate() -> <see cref="OnValidate"/>
        /// </summary>
        protected virtual void OnEditorValidate() { }
    }
}