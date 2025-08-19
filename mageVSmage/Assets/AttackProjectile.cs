
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public bool Apply = false;
    public bool isPlayer = false;
    public float Velocity = 800f;
    public float Damage = 30f;
    public bool shouldForce = false;
    public GameObject hitEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (Apply)
        {
            return;
        }
        if (other.CompareTag("Enemy") && isPlayer)
        {
            //Debug.Log("Enemy has been attacked!");
            // Add logic for what happens when the enemy is attacked
                Apply = true; // Prevent further application of damage
                enemyAdmin enemyAdmin = other.gameObject.GetComponent<enemyAdmin>();
                if (enemyAdmin != null)
                {
                    GameObject effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
                enemyAdmin.TakeDamage(Damage);
                Apply = true;
                }
                else
                {
                    Debug.LogWarning("EnemyAdmin component not found on the collided object.");
            }
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                if (rb != null && shouldForce)
                    rb.AddForce(transform.forward * Velocity, ForceMode.Impulse);

        }
        if (other.CompareTag("Player") && !isPlayer)
        {
            spellcaster spellCaster = other.gameObject.GetComponent<spellcaster>(); 
            if (spellCaster != null)
            {
                //Debug.Log("Player has been attacked!");
                GameObject effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
                spellCaster.setHealth(spellCaster.health - Damage);
                Apply = true;
            }
            else
            {
                Debug.LogWarning("Spellcaster component not found on the collided object.");
            }
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                if (rb != null && shouldForce)
                    rb.AddForce(transform.forward * Velocity, ForceMode.Impulse);

        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isPlayer)
        {
            Debug.Log("Player has been attacked!");
            // Add logic for what happens when the player is attacked
            PlayerControlHLh playerControl = collision.gameObject.GetComponent<PlayerControlHLh>();
            if (playerControl != null)
            {
                GameObject multiHitEffect = Instantiate(playerControl.multiHitEffect, playerControl.transform.position, Quaternion.identity);
                playerControl.GetHit(30f);
            }
            else
            {
                Debug.LogWarning("EnemyAiHandle component not found on the collided object.");
            }
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        //projectile movement
        transform.position += transform.forward * Velocity * Time.deltaTime;
    }
}
