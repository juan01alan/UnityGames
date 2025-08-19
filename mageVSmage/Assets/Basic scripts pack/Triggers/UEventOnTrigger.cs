using UnityEngine;
using UnityEngine.Events;

namespace Basic.Triggers
{
    public class UEventOnTrigger : MonoBehaviour
    {
        [Header("Disable after enter to prevent repeating. \nExclude all layers except player in collider")]
        [Tooltip("This blocks OnTriggerExit")][SerializeField] private bool _DisableScriptAfterEnter;
        [Tooltip("This blocks OnTriggerExit")][SerializeField] private bool _DisableGameObjectAfterEnter;

        public UnityEvent _OnTriggerEnter;
        public UnityEvent _OnTriggerExit;
        public UnityEvent _OnTriggerStay;

        private void OnTriggerEnter(Collider other)
        {
            _OnTriggerEnter.Invoke();
            CheckDisables();
        }

        private void OnTriggerExit(Collider other)
        {
            _OnTriggerExit.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            _OnTriggerStay.Invoke();
        }

        private void CheckDisables()
        {
            if (_DisableScriptAfterEnter)
                this.enabled = false;
            if (_DisableGameObjectAfterEnter)
                gameObject.SetActive(false);
        }

    }

}
