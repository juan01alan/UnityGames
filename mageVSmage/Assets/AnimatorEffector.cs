using System;
using System.Linq;
using UnityEngine;

public class AnimatorEffector : MonoBehaviour
{
    public GameObject SpawnEffect;
    public Transform Transform;
    public Transform transformToLook;
    private bool InstantiateReturn = false;
    public void SetSpawnEffect(GameObject effect)
    {
        InstantiateReturn = false; // Reset the flag to false when setting a new effect
        if (effect != null)
        {
            SpawnEffect = effect;
        }
        else
        {
           // Debug.LogError("Effect GameObject is null.");
        }
    }
    public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        var allKids = fromGameObject.GetComponentsInChildren<Transform>();
        var kid = allKids.FirstOrDefault(k => k.gameObject.name == withName);
        if (kid == null) return null;
        return kid.gameObject;
    }
    public void pawnEffect(String transformS)
    {
        if (InstantiateReturn)
        {
            return;
        }
        GameObject finder = GetChildGameObject(this.gameObject, transformS);
        if (finder != null)
        {
            Transform = finder.transform;
        }
        if (Transform != null && SpawnEffect != null && !InstantiateReturn)
        {
            InstantiateReturn = true; // Set the flag to true to prevent further instantiation
            Vector3 pos = Transform.position;
            GameObject spawnedEffect = Instantiate(SpawnEffect, pos, transformToLook.rotation);
            Destroy(spawnedEffect, 2f); // Destroy the effect after 2 seconds
            Transform = null; // Reset Transform to null after spawning
            spawnedEffect = null; // Reset spawnedEffect to null after spawning
        }
        else
        {
            //Debug.LogError("Effect GameObject is null.");
        }
        if (SpawnEffect != null && Transform == null && !InstantiateReturn)
        {
            InstantiateReturn = true; // Set the flag to true to prevent further instantiation

            GameObject spawnedEffect = Instantiate(SpawnEffect, transform.position, transformToLook.rotation);
            Destroy(spawnedEffect, 2f); // Destroy the effect after 2 seconds
            Transform = null; // Reset Transform to null after spawning
            spawnedEffect = null; // Reset spawnedEffect to null after spawning
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
