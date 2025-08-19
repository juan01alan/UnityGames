
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AttackbleScript : MonoBehaviour
{
    public bool isAttacking;
    public comberScriptEnemy comberScriptEnemy;
    public comberScript comberScript;
    public List<AttackbleScript> attackbleScripts = new List<AttackbleScript>();
    public bool isEnemy = false; // Flag to check if the object is an enemy
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (comberScriptEnemy != null)
        {
            foreach (GameObject item in comberScriptEnemy.AttackbleObjs)
            {
                AttackbleScript curAttackble = item.GetComponent<AttackbleScript>();
                if (curAttackble != null)
                {
                    attackbleScripts.Add(curAttackble);
                }
            }
        }
        if (comberScript != null)
        {

            foreach (GameObject item in comberScript.AttackbleObjs)
            {
                AttackbleScript curAttackble = item.GetComponent<AttackbleScript>();
                if (curAttackble != null)
                {
                    attackbleScripts.Add(curAttackble);
                }
            }
        }
        isAttacking = false;
    }

    public void Attack()
    {
        isAttacking = true;
    }
    public void StopAttack()
    {
        isAttacking = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isAttacking && !isEnemy)
        {
            enemyAdmin enemyAdmin = other.GetComponent<enemyAdmin>();
            if (enemyAdmin != null)
            {
                enemyAdmin.TakeDamage(20f);
                foreach (AttackbleScript item in attackbleScripts)
                {
                    item.StopAttack();   
                }
                Debug.Log("Enemy has been attacked!");
            }
            else
            {
                Debug.LogWarning("enemyAdmin component not found on the collided object.");
            }
        }
        if (other.CompareTag("Player") && isAttacking && isEnemy)
        {
            spellcaster spellcaster = other.GetComponent<spellcaster>();
            if (spellcaster != null)
            {
                spellcaster.setHealth(spellcaster.health - 10f);
                foreach (AttackbleScript item in attackbleScripts)
                {
                    item.StopAttack();
                }
                Debug.Log("Player has been attacked!");
            }
            else
            {
                Debug.LogWarning("spellcaster component not found on the collided object.");
            }

        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && isAttacking)
        {
            Debug.Log("Enemy has been attacked!");
            // Add logic for what happens when the enemy is attacked
            EnemyAiHandle enemyAiHandle = collision.gameObject.GetComponent<EnemyAiHandle>();
            if (enemyAiHandle != null)
            {
                enemyAiHandle.TakeDamage();
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
        
    }
}
