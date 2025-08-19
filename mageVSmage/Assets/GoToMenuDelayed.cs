using UnityEngine;

public class GoToMenuDelayed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start a coroutine to wait for 5 seconds before going to the menu
        StartCoroutine(GoToMenuAfterDelay(5f));


    }
    private System.Collections.IEnumerator GoToMenuAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        
        // Load the menu scene (assuming the menu scene is named "Menu")
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
