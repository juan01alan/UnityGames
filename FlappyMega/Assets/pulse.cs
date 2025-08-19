using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class pulse : MonoBehaviour
{
    [SerializeField]
    public GameObject player; // Reference to the fly script to check if the player is dead
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fly fly = player.GetComponent<fly>(); // Corrected: Use GetComponent<T>() to get the fly script component from the player GameObject
        if (InputSystem.actions.FindAction("Jump").WasPerformedThisFrame() && !fly.isDead)
        {
            StartCoroutine(PulseLight());
        }
    }

    // Coroutine to handle the light pulsing effect
    private IEnumerator PulseLight()
    {
        Light2D light2D = GetComponent<Light2D>(); // Get the Light2D component attached to this GameObject
        light2D.intensity = 2f; // Set the intensity to 2
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        light2D.intensity = 1f; // Reset the intensity to 1
    }
}
