using Unity.Cinemachine;
using UnityEngine;

public class CinMacCame : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CinemachineCamera = GetComponent<CinemachineCamera>();
        if (CinemachineCamera == null)
        {
            //Debug.LogError("CinemachineCamera component not found on this GameObject.");
        }
        else
        {
            //Debug.Log("CinemachineCamera component successfully assigned.");
            Transform target = GameObject.FindWithTag("Player")?.transform;
            CinemachineCamera.LookAt = target;
            CinemachineCamera.Follow = target;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
