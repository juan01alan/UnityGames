using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class powerManage : MonoBehaviour
{
    public GameObject player;
    public GameObject transformToLook;
    public float TpStart;
    public float TeleportCur;
    public LayerMask hitLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    public void Teleport(float tpDistance)
    {
        // Logic for teleportation
        if (player != null)
        {
            TpStart = tpDistance;
            TeleportCur = tpDistance;
            TryTp();
        }
    }
    public void TryTp()
    {
        Vector3 origin = player.transform.position; // Start the ray from this object's position
        Vector3 direction = transformToLook.transform.forward; // Cast the ray forward from this object

        RaycastHit hit; // Stores information about the hit

        // Perform the raycast
        if (Physics.Raycast(origin, direction, out hit, TeleportCur, hitLayer))
        {
            // If the ray hits something
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);
            // You can access hit.collider, hit.point, hit.normal, etc.
        }
        else
        {
            player.transform.position += transformToLook.transform.forward * TeleportCur; // Teleport to a new location
            // If the ray didn't hit anything within the specified length
            Debug.Log("Ray did not hit anything.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
