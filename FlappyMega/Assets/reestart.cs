using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class reestart : MonoBehaviour
{
    public Button Button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button.onClick.AddListener(RestartGame); // Adiciona o m�todo Reestart ao evento de clique do bot�o
    }

    // Update is called once per frame
    void Update()
    {
        // Nenhuma l�gica necess�ria aqui para o bot�o UI.
    }
  
    public void RestartGame()
    {
        AudioSource audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        audioSource.Play();
        SceneManager.LoadScene("SampleScene"); // Substitua "SampleScene" pelo nome da sua cena
    }
}
