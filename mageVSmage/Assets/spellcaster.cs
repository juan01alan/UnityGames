using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using System.Collections;
using TMPro;

public class spellcaster : MonoBehaviour
{
    public float health = 100f;
    public float mana = 100f;
    public float maxMana = 100f;
    public Animator animator;
    public Sprite spell1;
    public Sprite spell2;
    public Sprite spell3;
    public string Spell1Name;
    public string Spell2Name;
    public string Spell3Name;
    public Image spell1mg;
    public Image spell2mg;
    public Image spell3mg;
    public float spell1Cost = 10f;
    public float spell2Cost = 20f;
    public float spell3Cost = 30f;
    public float spell1Cooldown = 5f;
    public float spell2Cooldown = 10f;
    public float spell3Cooldown = 15f;
    public float spell1LastUsed = 0f;
    public float spell2LastUsed = 0f;
    public Rigidbody rb;
    public float spell3LastUsed = 0f;
    public bool Ivulnerable = false;
    public TextMeshProUGUI healthText; // Reference to the health text UI
    public TextMeshProUGUI manaText; // Reference to the health text UI
    public GameObject ivulEffect;
    public float backForce = 15f;
    public Transform transformToLook;
    public bool died = false; // Flag to check if the player has died
    public GameObject deathScreen; // Reference to the death screen UI

    [Header("Scripts to disable before death")]
    public MonoBehaviour[] scriptsToDisable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spell1mg == null)
        {
            spell1mg = GameObject.FindGameObjectWithTag("Spell1mg").GetComponent<Image>();
            spell2mg = GameObject.FindGameObjectWithTag("Spell2mg").GetComponent<Image>();
            spell3mg = GameObject.FindGameObjectWithTag("Spell3mg").GetComponent<Image>();
            healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
            manaText = GameObject.FindGameObjectWithTag("ManaText").GetComponent<TextMeshProUGUI>();
        }
        healthText.text = "Health: " + health.ToString("F0"); // Update health text UI
        manaText.text = "Mana: " + mana.ToString("F0"); // Update mana text UI
        spell1mg.sprite = spell1;
        spell2mg.sprite = spell2;
        spell3mg.sprite = spell3;

    }
    public void ResetIvul()
    {
        StartCoroutine(resetInvulnerable());
    }
    public IEnumerator RESETBACKFORCE()
    {
        yield return new WaitForSeconds(0.4f);
        rb.linearVelocity = Vector3.zero;
    }
    public IEnumerator resetInvulnerable()
    {
        StartCoroutine(RESETBACKFORCE());
        yield return new WaitForSeconds(1f);
        Ivulnerable = false;
    }
    public void setMana(float manas)
    {
        mana = manas;
        mana = Mathf.Clamp(mana, 0f, maxMana);
        manaText.text = "Mana: " + mana.ToString("F0"); // Update mana text UI
    }
    public void setHealth(float healths)
    {
        if (Ivulnerable)
        {
            if (ivulEffect != null)
            {

                StopCoroutine(resetInvulnerable());
                Ivulnerable = false;
                GameObject gameObject = Instantiate(ivulEffect, transform.position, Quaternion.identity); // Instantiate the invulnerability effect
            }
            return; // If the player is invulnerable, do not change health
        }
        if (died)
        {
            return;
        }
        Ivulnerable = true; // Set invulnerable state when health is changed
        StartCoroutine(resetInvulnerable()); // Reset invulnerability after a delay;
        animator.SetTrigger("hit");
        Vector3 backForceV = transformToLook.forward * -1;
        rb.AddForce(backForceV * backForce, ForceMode.Impulse);
        health = healths;
        health = Mathf.Clamp(health, 0f, 100f);
        healthText.text = "Health: " + health.ToString("F0"); // Update health text UI
        if (health <=0)
        {
            health= 0f;
            Death();
        }
    }
    private void Death()
    {
        died = true; // Set the died flag to true
        healthText.text = "Health: " + health.ToString("F0"); // Update health text UI
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        animator.SetBool("Walk", false);
        animator.SetTrigger("die");
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            script.enabled = false; // Disable all specified scripts
        }
        if (deathScreen != null)
        {
            GameObject gameObject = Instantiate(deathScreen, transform.position, Quaternion.identity); // Instantiate the invulnerability effect
        }
        // Optionally, you can destroy the game object or perform other actions
    }
   private void Spell1()
    {
        if (mana >= spell1Cost && Time.time >= spell1LastUsed + spell1Cooldown)
        {
            spell1mg.enabled = false;
            animator.SetTrigger("Spell1");
            mana -= spell1Cost;
            manaText.text = "Mana: " + mana.ToString("F0"); // Update health text UI
            spell1LastUsed = Time.time;
            // Add logic to cast Spell 1
            Debug.Log("Spell 1 casted: " + Spell1Name);
        }
        else
        {
            Debug.Log("Not enough mana or spell is on cooldown.");
        }

    }
    private void Spell2()
    {
        if (mana >= spell2Cost && Time.time >= spell2LastUsed + spell2Cooldown)
        {
            spell2mg.enabled = false;
            animator.SetTrigger("Spell2");
            mana -= spell2Cost;
            manaText.text = "Mana: " + mana.ToString("F0"); // Update health text UI
            spell2LastUsed = Time.time;
            // Add logic to cast Spell 2
            Debug.Log("Spell 2 casted: " + Spell2Name);
        }
        else
        {
            Debug.Log("Not enough mana or spell is on cooldown.");
        }   

    }
    private void Spell3()
    {
        if (mana >= spell3Cost && Time.time >= spell3LastUsed + spell3Cooldown)
        {
            spell3mg.enabled = false;
            animator.SetTrigger("Spell3");
            mana -= spell3Cost;
            manaText.text = "Mana: " + mana.ToString("F0"); // Update health text UI
            spell3LastUsed = Time.time;
            // Add logic to cast Spell 3
            Debug.Log("Spell 3 casted: " + Spell3Name);
        }
        else
        {
            Debug.Log("Not enough mana or spell is on cooldown.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (mana >= spell1Cost && Time.time >= spell1LastUsed + spell1Cooldown)
        {
            spell1mg.enabled = true;
        }
        if (mana >= spell2Cost && Time.time >= spell2LastUsed + spell2Cooldown)
        {
            spell2mg.enabled = true;
        }
        if (mana >= spell3Cost && Time.time >= spell3LastUsed + spell3Cooldown)
        {
            spell3mg.enabled = true;
        }
        if (transform.position.y <= -8f)
        {
            setHealth(health - 20f);
        }
        if (InputSystem.actions.FindAction("Spell1").WasPerformedThisFrame())
        {
            Spell1();
        }
        if (InputSystem.actions.FindAction("Spell2").WasPerformedThisFrame())
        {
            Spell2();

        }
        if (InputSystem.actions.FindAction("Spell3").WasPerformedThisFrame())
        {
            Spell3();
        }

    }
}
