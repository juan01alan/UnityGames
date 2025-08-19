using Basic.PublicVariables;
using jcsilva.AISystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jcsilva.CharacterController {

    public class EnemyController : MonoBehaviour {

        [Header("References")]
        [SerializeField] AIStateMachine stateMachine;
        public enemyAdmin enemAdm;
        public Animator anim;

        [Header("Enemy Settings")]
        [SerializeField] float fireRate = 1f;
        [SerializeField] float spell1Rate = 10f;
        [SerializeField] float spell2Rate = 15f;
        [SerializeField] float spell3Rate = 25f;
        [SerializeField] float elapsedTime;
        [SerializeField] float elapsedSpellTime;
        [SerializeField] int SelectedSpell = 1; // Assuming this is a float value to select the spell
        [SerializeField] public float spell1ManaCost = 20f; // Assuming a mana cost for the first spell
        [SerializeField] public float spell2ManaCost = 30f; // Assuming a mana cost for the second spell
        [SerializeField] public float spell3ManaCost = 50f; // Assuming a mana cost for the third spell
        [SerializeField] float manaRegenRate = 5f; // Cooldown for manaRegen
        public comberScriptEnemy comberScript;
        public bool isSpeller = false; // Assuming this is a boolean to check if the enemy can cast spells
        public bool canShootSpell1 = true;
        public bool canShootSpell2 = true;
        public bool canShootSpell3 = true;
        public float manaElapsedTime = 0f; // Timer for mana regeneration
        public float Mana  = 100f; // Assuming Mana is a float value representing the enemy's mana
        public float maxMana = 100f; // Assuming maxMana is a float value representing the maximum mana the enemy can have

        private bool canShoot;

        private void Awake() {
            if (stateMachine == null) {
                stateMachine = GetComponent<AIStateMachine>();
            }
            if (!isSpeller)
            {
                canShootSpell1 = false;
                canShootSpell2 = false;
                canShootSpell3 = false;
            }
        }

        private void OnEnable() {
            stateMachine.EventAIEnableAttack += Shoot;
            stateMachine.EventAIDisableAttack += CantShoot;
        }

        private void OnDisable() {
            stateMachine.EventAIEnableAttack -= Shoot;
            stateMachine.EventAIDisableAttack -= CantShoot;
        }

        public void UseMana(float amount)
        {
            Mana -= amount;
            if (Mana < 0)
            {
                Mana = 0; // Prevent mana from going below zero
            }
        }
        // Update is called once per frame
        void Update() {
            if (isSpeller)
            {
                if (Mana >= spell3ManaCost && SelectedSpell == 3)
                {

                    if (canShootSpell3)
                    {
                        //vai ter que corrigir bug pra não ficar spamando o spell, ajustar sistema de cooldown e pra nao spawnar só a primeira spell
                        if (elapsedSpellTime > spell3Rate)
                        {
                            IsShootingSpell3();
                            elapsedSpellTime = 0f;
                        }
                        else
                        {
                            elapsedSpellTime += Time.deltaTime;
                        }
                    }
                    else if (!canShootSpell3 && elapsedSpellTime > 0f)
                    {
                        if (elapsedSpellTime > spell3Rate)
                        {
                            elapsedSpellTime = 0f;
                        }
                        else
                        {
                            elapsedSpellTime += Time.deltaTime;
                        }
                    }
                }
                if (Mana >= spell2ManaCost && SelectedSpell == 2)
                {

                    if (canShootSpell2)
                    {
                        //vai ter que corrigir bug pra não ficar spamando o spell, ajustar sistema de cooldown e pra nao spawnar só a primeira spell
                        if (elapsedSpellTime > spell2Rate)
                        {
                            IsShootingSpell2();
                            elapsedSpellTime = 0f;
                        }
                        else
                        {
                            elapsedSpellTime += Time.deltaTime;
                        }
                    }
                    else if (!canShootSpell2 && elapsedSpellTime > 0f)
                    {
                        if (elapsedSpellTime > spell2Rate)
                        {
                            elapsedSpellTime = 0f;
                        }
                        else
                        {
                            elapsedSpellTime += Time.deltaTime;
                        }
                    }
                }
                if (Mana >= spell1ManaCost && SelectedSpell == 1)
                {

                    if (canShootSpell1)
                    {
                        //vai ter que corrigir bug pra não ficar spamando o spell, ajustar sistema de cooldown e pra nao spawnar só a primeira spell
                        if (elapsedSpellTime > spell1Rate)
                        {
                            IsShootingSpell1();
                            elapsedSpellTime = 0f;
                        }
                        else
                        {
                            elapsedSpellTime += Time.deltaTime;
                        }
                    }
                    else if (!canShootSpell1 && elapsedSpellTime > 0f)
                    {
                        if (elapsedSpellTime > spell1Rate)
                        {
                            elapsedSpellTime = 0f;
                        }
                        else
                        {
                            elapsedSpellTime += Time.deltaTime;
                        }
                    }
                }
                if (Mana <= maxMana)
                {
                    manaElapsedTime += Time.deltaTime;
                    if (manaElapsedTime >= manaRegenRate)
                    {

                        Mana += 10f;
                        if (Mana > maxMana)
                        {
                            Mana = maxMana;
                            
                        }
                        if (enemAdm.health < enemAdm.maxHealth)
                        {
                            enemAdm.AddHealth(5f);  

                        }
                        manaElapsedTime = 0f;
                    }
                }
                if (canShoot && !canShootSpell1 && !canShootSpell2 && !canShootSpell3)
                {
                    if (elapsedTime > fireRate)
                    {
                        IsShooting();
                        elapsedTime = 0f;
                    }
                    else
                    {
                        elapsedTime += Time.deltaTime;
                    }
                }
                else if (!canShoot && elapsedTime > 0f)
                {
                    if (elapsedTime > fireRate)
                    {
                        elapsedTime = 0f;
                    }
                    else
                    {
                        elapsedTime += Time.deltaTime;
                    }
                }
            }
            else
            {
                if (canShoot)
                {
                    if (elapsedTime > fireRate)
                    {
                        IsShooting();
                        elapsedTime = 0f;
                    }
                    else
                    {
                        elapsedTime += Time.deltaTime;
                    }
                }
                else if (!canShoot && elapsedTime > 0f)
                {
                    if (elapsedTime > fireRate)
                    {
                        elapsedTime = 0f;
                    }
                    else
                    {
                        elapsedTime += Time.deltaTime;
                    }
                }

            }
            
        }

        private void IsShootingSpell1()
        {
            if (comberScript != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 direction = player.transform.position;
                    direction.y = 0; // Keep the direction horizontal
                    transform.LookAt(direction); // Make the enemy look at the player
                }
                SelectedSpell = Random.Range(1, 4); // Randomly select a spell between 1 and 3
                SelectedSpell = Mathf.Clamp(SelectedSpell, 1, 3); // Ensure the selected spell is within the valid range
                Mana -= spell1ManaCost; // Deduct mana for the spell
                anim.SetBool("Walk", false);
                comberScript.Spell1();
            }
            else
            {
                Debug.LogWarning("ComberScriptEnemy is not assigned in EnemyController.");
            }

        }
        private void IsShootingSpell2()
        {
            if (comberScript != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 direction = player.transform.position;
                    direction.y = 0; // Keep the direction horizontal
                    transform.LookAt(direction); // Make the enemy look at the player
                }
                SelectedSpell = Random.Range(1, 4); // Randomly select a spell between 1 and 3
                SelectedSpell = Mathf.Clamp(SelectedSpell, 1, 3); // Ensure the selected spell is within the valid range
                Mana -= spell2ManaCost; // Deduct mana for the spell
                anim.SetBool("Walk", false);
                comberScript.Spell2();
            }
            else
            {
                Debug.LogWarning("ComberScriptEnemy is not assigned in EnemyController.");
            }

        }
        private void IsShootingSpell3()
        {
            if (comberScript != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 direction = player.transform.position;
                    direction.y = 0; // Keep the direction horizontal
                    transform.LookAt(direction); // Make the enemy look at the player
                }
                SelectedSpell = Random.Range(1, 4); // Randomly select a spell between 1 and 3
                SelectedSpell = Mathf.Clamp(SelectedSpell, 1, 3); // Ensure the selected spell is within the valid range
                Mana -= spell3ManaCost; // Deduct mana for the spell
                anim.SetBool("Walk", false);
                comberScript.Spell3();
            }
            else
            {
                Debug.LogWarning("ComberScriptEnemy is not assigned in EnemyController.");
            }

        }

        private void Shoot() {
            canShoot = true;
        }

        private void CantShoot() {
            canShoot = false;
        }

        private void IsShooting() {
            if (comberScript != null)
            {
                anim.SetBool("Walk", false);
                comberScript.Attack();
            }
            else
            {
                Debug.LogWarning("ComberScriptEnemy is not assigned in EnemyController.");
            }
        }
    }
}
