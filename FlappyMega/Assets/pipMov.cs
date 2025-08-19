using UnityEngine;
using UnityEngine.SceneManagement;

public class pipMov : MonoBehaviour
{
    public float minRange = -4f; // Minimum range for random spawn position
    public float maxRange = -2f; // Maximum range for random spawn position

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*Vector3 newPosition = transform.position;
        newPosition.y = Random.Range(minRange, maxRange);
        transform.position = newPosition;*/
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the colliding object has the tag "Player"
        {
            AudioSource audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
            audioSource.Play();
            fly flyScript = collision.gameObject.GetComponent<fly>(); // Get the fly script from the Player object
            flyScript.death(collision.gameObject); // Pass the required GameObject parameter to the death method
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
