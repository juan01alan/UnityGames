using System.Collections;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class fly : MonoBehaviour
{
    public Rigidbody2D rb2d; // Rigidbody2D component for physics interactions
    public float speed = 5f; // Speed of the object
    public bool isDead = false; // Flag to check if the object is dead

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.actions.FindAction("Jump").WasPerformedThisFrame() && !isDead)
        {
            AudioSource audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
            audioSource.Play();
            rb2d.linearVelocity = new Vector2(0, speed); // Set the velocity to move upwards
        }
        if (transform.position.y > 5.6f)
        {
            death(this.gameObject);
        }
    }

    public void death(GameObject gameObject)
    {
        this.isDead = true; // Set the isDead flag to true to prevent further actions
        Animator animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
        animator.SetTrigger("Death"); // Trigger the death animation
        this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Stop the Rigidbody2D's velocity
        this.gameObject.GetComponent<Rigidbody2D>().simulated = false; // Disable the Rigidbody2D to stop physics interactions
        this.gameObject.GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent further collisions
        StartCoroutine(GameOver()); // Start the coroutine to wait and reload the scene
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("terrain")) // If the colliding object is an obstacle
        {
            death(this.gameObject); // Call the death method with the collided object
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f); // Wait for 2 seconds
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver"); // Load the GameOver scene
    }
}
