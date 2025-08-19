using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class opengame : MonoBehaviour
{
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(() => { openGame(); });
    }
    void openGame()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
