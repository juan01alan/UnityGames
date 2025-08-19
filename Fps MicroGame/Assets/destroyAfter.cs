using UnityEngine;

public class destroyAfter : MonoBehaviour
{
    public float destroyTime = 2.0f; // Time after which the object will be destroyed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
