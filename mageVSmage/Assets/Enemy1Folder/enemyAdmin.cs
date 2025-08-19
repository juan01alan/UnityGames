
using Ilumisoft.HealthSystem;
using System.Collections;
using UnityEngine;

public class enemyAdmin : MonoBehaviour
{
    public Health Health; // Reference to the Health component
    public float health = 100f; // Health of the enemy
    public float maxHealth;
    public Animator anim; // Reference to the Animator component
    public GameObject winscreen; // Reference to the win screen UI
    public bool isBoss = false; // Flag to check if the enemy is a boss
    public Rigidbody rb;
    public bool Ivulnerable = false; // Flag to check if the enemy is invulnerable
    public bool died = false; // Flag to check if the enemy has died
    public GameObject ivulEffect;
    public GameObject textdamager;
    public GameObject spTxt1;
    public GameObject spTxt2;
    [Header("Scripts to disable berfore die")]
    public MonoBehaviour[] scriptsToDisable; // Scripts to disable before the enemy dies
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
        Health = GetComponent<Health>(); // Get the Health component attached to the enemy
        Health.MaxHealth = health; // Set the current health to the initial health value
        Health.SetHealth(health); // Set the current health to the initial health value
        Health.CurrentHealth = health; // Set the current health to the initial health value
    }
    public void AddHealth (float healthA)
    {
        health += healthA;
        Health.SetHealth(health); // Set the current health to the initial health value
        Health.CurrentHealth = health; // Set the current health to the initial health value

    }

    public IEnumerator ResetRoll()
    {

        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity = Vector3.zero;
    }
    public IEnumerator resetIvulnerable()
    {
        StartCoroutine(ResetRoll());

        yield return new WaitForSeconds(0.8f);
        Ivulnerable = false; // Reset invulnerability after 1 second
    }
    public void TakeDamage(float amount)
    {
        float RandManager = (int)Random.Range(1, 3);
        RandManager = Mathf.Clamp(RandManager,1,2);
        if (RandManager == 1)
        {

            GameObject gameObject = Instantiate(textdamager, spTxt1.transform.position, spTxt1.transform.rotation);
            textDamager td = gameObject.GetComponent<textDamager>();
            if (td != null)
            {
                td.setDamage(amount);
            }
        }
        else
        {
            GameObject gameObject = Instantiate(textdamager, spTxt2.transform.position, spTxt2.transform.rotation);
            textDamager td = gameObject.GetComponent<textDamager>();
            if (td != null)
            {
                td.setDamage(amount);
            }
        }
        if (Ivulnerable)
        {
            if (ivulEffect != null)
            {
                /*StopCoroutine(resetIvulnerable());
                Ivulnerable = false;*/
                GameObject gameObject = Instantiate(ivulEffect, transform.position, Quaternion.identity); // Instantiate the invulnerability effect
            }
            return;
        }
        if (died)
        {
            return; 
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position;
            direction.y = 0; // Keep the direction horizontal
            transform.LookAt(direction); // Make the enemy look at the player
        }
        Ivulnerable = true;
        int forceImpulse = (int)amount / 3;
        
        if (forceImpulse > 0) {
            Vector3 back = transform.forward * -1;
            rb.AddForce(back * 15, ForceMode.Impulse); 
        }
        anim.SetTrigger("hit"); // Trigger the hit animations
        StartCoroutine(resetIvulnerable()); // Start the coroutine to reset invulnerability
        health -= amount;
        Health.SetHealth(health); // Set the current health to the initial health value
        Health.CurrentHealth = health; // Set the current health to the initial health value
        if (health <= 0)
        {
            Die();
        }
    }   
    private void Die()
    {
        died = true; // Set the died flag to true
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        anim.SetBool("Walk", false);
        // Disable the enemy controller and AI state machine scripts
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            script.enabled = false; // Disable all specified scripts
        }
        anim.SetTrigger("die"); // Trigger the death animation
        if (isBoss)
        {

            if (winscreen != null)
            {
                GameObject gameObject = Instantiate(winscreen, transform.position, Quaternion.identity); // Instantiate the invulnerability effect
            }
        }
        Destroy(gameObject,2); // Destroy the enemy game object
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -8f)
        {
            TakeDamage(20f);
        }

    }
}
