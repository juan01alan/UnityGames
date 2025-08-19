using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jogar : MonoBehaviour
{
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(() => { Play(); });
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
