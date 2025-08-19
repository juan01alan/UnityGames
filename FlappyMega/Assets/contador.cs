using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;

public class contador : MonoBehaviour
{
    public int score = 1; // The amount to increase the score when the player collides with this object
    public Collider2D Collider2D;
    public TextMeshProUGUI scoreText; // Reference to the UI Text component for displaying the score
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>(); // Find the UI Text GameObject and get its Text component
        Collider2D = GetComponent<Collider2D>(); // Get the Collider2D component attached to this GameObject
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the colliding object has the tag "Player"
        {
            AudioSource audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
            audioSource.Play();
            if (int.Parse(scoreText.text) + score < 999)
            {
                scoreText.text = (int.Parse(scoreText.text) + score).ToString(); // Increment the score and update the UI Text
            }
            else
            {
                scoreText.text = "MEGA SCORE"; // Cap the score at 999
            }
            // Here you can add code to handle the collision, such as increasing score or triggering an event
        }
    }
}
