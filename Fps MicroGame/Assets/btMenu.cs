using UnityEngine;
using UnityEngine.UI;

public class btMenu : MonoBehaviour
{
    public Button bt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bt.onClick.AddListener(LoadScene);
    }
    private void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
