using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class pipe : MonoBehaviour
{
    [SerializeField] private float speed = 4f;

    public float delimited;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()

    {
        if (transform.position.x < delimited)
        {
            Destroy(
         gameObject); // Destroy the GameObject if it goes beyond a certain x position
        }
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
