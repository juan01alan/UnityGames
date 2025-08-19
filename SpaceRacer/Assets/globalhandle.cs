using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class globalhandle : MonoBehaviour
{
    // This is where you create the global variable that can be accessed from anywhere.
    public bool usePostProcessB = false;
    public bool useLightB = false;
    public List<GameObject> lightsC;
    public GameObject PostProcess;
    public void setUsePostP()
    {
        usePostProcessB = !usePostProcessB;
    }
    public void setLightUse()
    {
        useLightB = !useLightB;
    }
    void OnDisable()
    {
        if (usePostProcessB)
        {
            PlayerPrefs.SetFloat("usePostProcess", 1);
        }
        else
        {
            PlayerPrefs.SetFloat("usePostProcess", 0);
        }
        if (useLightB)
        {
            PlayerPrefs.SetFloat("useLight", 1);
        }
        else
        {
            PlayerPrefs.SetFloat("useLight", 0);
        }
    }
    void OnEnable()
    {
        float useLight = PlayerPrefs.GetFloat("useLight");
        float usePostProcess = PlayerPrefs.GetFloat("usePostProcess");
        if (useLight == 1)
        {
            useLightB = true;
        }
        else
        {
            useLightB = false;
        }
        if (usePostProcess == 1)
        {
            usePostProcessB = true;
        }
        else
        {
            usePostProcessB = false;
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {

            if (!useLightB)
            {
                foreach (var item in lightsC)
                {
                    Destroy(item);
                }
            }
            if (!usePostProcessB)
            {
                Destroy(PostProcess);
            }
        }
    }
    private void Start()
    {
    }
}
