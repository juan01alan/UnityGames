using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PowerScript : MonoBehaviour
{
    public bool AlreadyAttacked;
    public float Damage;
    public GameObject impactEffect;
    public GameObject player;
    public GameObject impactEffectI;
    public Vector3 forward;
    public bool canRun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerScript playerScript = player.GetComponent<playerScript>(); 
            forward = Vector3.forward;
            Damage = playerScript.Damage * 5;
            canRun = true;
        }
        else
        {
            Debug.LogWarning("Player not found");
            forward = Vector3.forward;
            Damage = 40f;
            canRun = true;
        }
        Destroy(this, 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!AlreadyAttacked)
        {

            if (other.gameObject.tag == "enemy")
            {
                enemyScript enemyScript = other.GetComponent<enemyScript>();
                if (enemyScript != null)
                {
                    impactEffectI = Instantiate(impactEffect, transform.position, transform.rotation);

                    AlreadyAttacked = true;
                    StartCoroutine(SelfDestructHit());
                    enemyScript.TakeDamage(Damage);
                    StartCoroutine(SelfDestruct());
                    Destroy(impactEffectI,1f);
                }
            }
        }
    }

    private IEnumerator SelfDestructHit()
    {
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (canRun)
        {
            transform.Translate(forward * 25f * Time.deltaTime);
        }
    }
}
