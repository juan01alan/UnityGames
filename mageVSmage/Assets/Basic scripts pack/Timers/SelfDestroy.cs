using UnityEngine;

namespace Basic.Timers
{
    public class SelfDestroy : MonoBehaviour
    {
        [Tooltip("This one works after Thread.Sleep()")][SerializeField] float _timer = 0f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Destroy(gameObject, _timer);
        }

    }
}
