using System.Collections;
using UnityEngine;

public class delayGmM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicp(); // Call the musicp method to start the coroutine
    }
    private IEnumerator musicp()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        AudioSource audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        audioSource.Play(); // Play the audio
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
