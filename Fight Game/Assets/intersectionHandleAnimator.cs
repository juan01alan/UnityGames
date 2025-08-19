using UnityEngine;
using UnityEngine.SceneManagement;

public class intersectionHandleAnimator : MonoBehaviour
{
    public playerScript playerScript;
    public enemyScript enemyScript;
    public bool isEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void hitFinished()
    {
    }
    public void Die()
    {
        if (!isEnemy)
        {
            SceneManager.LoadScene(0);

        }
    }
    public void PlayerEspecial()
    {
        if (playerScript != null)
        {
            playerScript.EspecialSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
