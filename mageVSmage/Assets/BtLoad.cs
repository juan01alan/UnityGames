using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtLoad : MonoBehaviour
{
    public Button button;
    public int indexScene = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*button.onClick.AddListener(() =>
        {
            OnButtonClick();
        });*/
    }
    public void OnButtonClick()
    {
        SceneManager.LoadScene(indexScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
