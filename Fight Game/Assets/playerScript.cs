using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Random = UnityEngine.Random;

public class playerScript : MonoBehaviour
{
    public Camera playerCamera;
    public Transform camera;
    public Animator anim;
    public Rigidbody rb;
    public float speed = 6f;
    public LayerMask groundLayer;
    public float InitialSpeed = 6f;
    public float TurnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public float jumpForce = 5f;
    public bool isGrounded;
    public bool isJumping;
    public bool isInAir;

    [Header("GamePlay Variables")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float mana = 100f;
    public float maxMana = 100f;
    public float experience = 0f;
    public float maxExperience = 100f;
    public int level = 1;
    public int maxLevel = 100;
    public float manaRegenCooldown = 5f;
    public float LastManaRegenTime = 0f;
    public float LifeRegenCooldown = 5f;
    public float LastLifeRegenTime = 0f;
    public float staminaRegenCooldown = 5f;
    public float LastStaminaRegenTime = 0f;
    public float LastSprintTime = 0f;
    public float sprintCooldown = 1f;

    [Header("Combat")]
    public float cooldownTime = 1f;
    public float nextFireTime = 0f;
    public static int noOfClicks = 0;
    public float Damage = 10f;
    public float lastClickedTime = 0;
    public float maxComboDelay = 1;
    public LayerMask enemyLayer;
    public List<BoxCollider> attackHitboxes = new List<BoxCollider>();
    public GameObject[] hitEffects;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject[] hitPoints;
    public bool isAttacking = false;
    public bool Died;
    public bool invulneravel;
    public float especialCD = 1f;
    public float LastEspecialTime;
    public GameObject SpawnEspecialPoint;
    public GameObject PowerPrefabC;

    public bool isHitting = false;
    [Header("Roguelike")]
    public float EnemyDeaths = 0f;
    public float enemysToSpawn = 5f;
    public float spawnRange = 50f;
    public float SpawnLife;
    public float SpawnStamina;
    public float SpawnMana;
    public List<GameObject> SpawnEnemyPos;
    public GameObject enemyPrefab;

    [Header("UIthings")]
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI staminaText;
    public CameraShake camShake;

    public bool DeathHandler = true;

    void Start()
    {
        hitPoints = GameObject.FindGameObjectsWithTag("HitPoint");
        speed = InitialSpeed;
        GameObject selectPointSpawn = SpawnEnemyPos[Random.Range(0, SpawnEnemyPos.Count - 1)];
        GameObject enemy = Instantiate(enemyPrefab, selectPointSpawn.transform.position, selectPointSpawn.transform.rotation);
    }
    public void HandleEnemyDeath(GameObject enemy)
    {
        if (!DeathHandler)
        {
            DeathHandler = true;
            EnemyDeaths++;
            enemyScript enemyScript = enemy.GetComponent<enemyScript>();
            experience += 40;
            GameObject selectPointSpawn = SpawnEnemyPos[Random.Range(0, SpawnEnemyPos.Count - 1)];
            GameObject enemyInst = Instantiate(enemyPrefab, selectPointSpawn.transform.position, selectPointSpawn.transform.rotation);
        }
    }
    public void ResetAttack()
    {
        isAttacking = false;
    }

    private void ResetHit()
    {
        isHitting = false;
    }
    public void TakeDamage(float damage, GameObject other)
    {

        if (!isHitting)
        {

            isHitting = true;
            health -= damage;
            Debug.Log("take damage: " + damage);
            transform.LookAt(other.transform);
            Invoke(nameof(ResetAttack), cooldownTime);
            Invoke(nameof(ResetHit), cooldownTime);
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", false);
            if (health <= 0)
            {
                Die();
            }
            else
            {
                anim.SetTrigger("Hit");
                camShake.Shake(4f);
            }
        }
    }
    public void Die()
    {
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        Died = true;
        rb.useGravity = false;
        //useanim rootmotion to make the death animation play correctly
        anim.applyRootMotion = true;
        foreach (BoxCollider hitbox in attackHitboxes)
        {
            hitbox.enabled = false;
        }
        anim.SetTrigger("Die");
        this.enabled = false;
    }
    public void ColliderAction(Collider other, GameObject Member)
    {
        //RaycastHit hit;
        //Ray ray = new Ray(Member.transform.position, (other.ClosestPoint(Member.transform.position) - Member.transform.position).normalized);
        //if (Physics.Raycast(ray, out hit))
        //{
        //}
        //Vector3 hitPointPosition = hit.point;   
        // Check if the collider is an enemy
        if (other.CompareTag("enemy"))
        {
            AudioSource audioSource = Member.GetComponent<AudioSource>();
            if (isAttacking && !isHitting)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
                {

                    GameObject HITselect = hitEffects[Random.Range(0, hitEffects.Length)];

                    //get hit point in Enemy
                    GameObject effect = Instantiate(HITselect, Member.transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    // Apply damage to the enemy
                    enemyScript enemy = other.GetComponent<enemyScript>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                    }
                    audioSource.Play();
                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
                {

                    GameObject HITselect = hitEffects[Random.Range(0, hitEffects.Length)];

                    //get hit point in Enemy
                    GameObject effect = Instantiate(HITselect, Member.transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    // Apply damage to the enemy
                    enemyScript enemy = other.GetComponent<enemyScript>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                    }
                    audioSource.Play();
                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
                {

                    GameObject HITselect = hitEffects[Random.Range(0, hitEffects.Length)];

                    //get hit point in Enemy
                    GameObject effect = Instantiate(HITselect, Member.transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    // Apply damage to the enemy
                    enemyScript enemy = other.GetComponent<enemyScript>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                    }
                    audioSource.Play();
                }


            }
            // If it is, add it to the list of enemies in range
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnClick()
    {
        if (!isHitting)
        {
            //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
            lastClickedTime = Time.time;
            nextFireTime = Time.time + cooldownTime;
            noOfClicks++;
            camShake.Shake(4f);
            if (noOfClicks == 1)
            {
                anim.SetBool("hit1", true);
                isAttacking = true;
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

            if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
            {
                anim.SetBool("hit1", false);
                anim.SetBool("hit2", true);
            }
            if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
            {
                anim.SetBool("hit2", false);
                anim.SetBool("hit3", true);
            }

        }
    }

    private void FixedUpdate()
    {
        if (!Died)
        {

            // FixedUpdate is not used in this script, but it's a good practice to keep it for physics-related updates.
            //Movimento vai aqui

            //peguei valores do input system novo
            Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
            //fiz duas variaveis para horizontal e vertical desse Vector 2 (x,y)
            float horizontal = input.x;
            float vertical = input.y;
            //fiz um vector 3 com esses valores normalizados para ser a direcao do movimento
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            //se a direcao for maior que 0.1, ou seja, se o player estiver se movendo
            if (direction.magnitude >= 0.1f)
            {
                //aqui eu pego o angulo da direaoo do movimento em relacao ao eixo y da c�mera
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
                //aqui eu fa�o uma suavizacao do angulo de rotacao do player
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
                //aqui eu aplico a rotacao do player e movimento
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                //aqui eu aplico o movimento do player
                    //se o player estiver se movendo, eu seto a animacao de correr para true
                    anim.SetBool("Run", true);
                    rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.deltaTime);
                
            }
            else
            {
                //se o player nao estiver se movendo, eu seto a animacao de correr para false
                anim.SetBool("Run", false);
            }
        }

    }
    public void EspecialSpawn()
    {
        if (!isHitting && !isAttacking)
        {
            mana -= 20f;
            Transform transform = SpawnEspecialPoint.transform;
            GameObject PowerPrefab = Instantiate(PowerPrefabC, transform.position, transform.rotation);

            camShake.Shake(6f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!Died)
        {

            //regenthings

            if (Time.time > LastManaRegenTime + manaRegenCooldown && mana < maxMana)
            {
                mana += 1f * Time.deltaTime;
                LastManaRegenTime = Time.time;
                mana = Mathf.Clamp(mana, 0, maxMana);
            }
            if (Time.time > LastLifeRegenTime + LifeRegenCooldown && health < maxHealth)
            {
                health += 1f * Time.deltaTime;
                LastLifeRegenTime = Time.time;
                health = Mathf.Clamp(health, 0, maxHealth);
            }
            if (Time.time > LastStaminaRegenTime + staminaRegenCooldown && stamina < maxStamina)
            {
                stamina += 1f * Time.deltaTime;
                LastStaminaRegenTime = Time.time;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
            float healthP = Mathf.RoundToInt(health);
            float manaP = Mathf.RoundToInt(mana);
            float staminaP = Mathf.RoundToInt(stamina);
            lifeText.text = "Vida: " + healthP + "/" + maxHealth;
            manaText.text = "Mana: " + manaP + "/" + maxMana;
            staminaText.text = "Stamina: " + staminaP + "/" + maxStamina;

            //fim regenthings

            //experience gain

            if (experience >= maxExperience)
            {
                level++;
                Damage += 5f; // Increase damage per level
                maxHealth += 20f; // Increase max health per level
                maxMana += 10f; // Increase max mana per level
                health = maxHealth; // Restore health to max health on level up
                mana = maxMana; // Restore mana to max mana on level up
                stamina = maxStamina; // Restore stamina to max stamina on level up
                experience = 0f;
                maxExperience += 25
                    ; // Increase the max experience needed for the next level
                if (level > maxLevel)
                {
                    level = maxLevel; // Cap the level at maxLevel
                }
            }
            //end experience gain

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                isHitting = false;
                anim.ResetTrigger("Hit");
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
            {
                anim.SetBool("hit1", false);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
            {
                anim.SetBool("hit2", false);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
            {
                anim.SetBool("hit3", false);
                noOfClicks = 0;
            }


            if (Time.time - lastClickedTime > maxComboDelay)
            {
                noOfClicks = 0;
                isAttacking = false;
            }

            //cooldown time
            if (Time.time > nextFireTime)
            {
                if (InputSystem.actions.FindAction("Attack").WasPerformedThisFrame())
                {
                    OnClick();

                }
            }
            if (Time.time > LastEspecialTime + especialCD)
            {
                if (InputSystem.actions.FindAction("Especial").WasPerformedThisFrame())
                {
                    LastEspecialTime = Time.time;
                    anim.SetTrigger("Especial");
                }
            }
            else
            {
                anim.ResetTrigger("Especial");
            }
                //pegando variaveis de estado do player
                //aqui eu verifico se o player estta no chaoo, se ele esta pulando e se ele esta no ar
                isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.4f, groundLayer);
            isJumping = InputSystem.actions.FindAction("Jump").ReadValue<float>() > 0.1f;
            isInAir = !isGrounded && rb.linearVelocity.y > 0.1f;


            //sprint função
            if (InputSystem.actions.FindAction("Sprint").ReadValue<float>() > 0.1f)
            {
                if (stamina > 5f)
                {
                    // Aumenta a velocidade ao sprintar de forma gradual
                    speed = Mathf.Lerp(speed, InitialSpeed * 3, Time.deltaTime * 5f);
                    //aumentar fov da camera gradualmente para efeito de sprint
                    playerCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 90f, Time.deltaTime * 5f);
                }
                if (Time.time > LastSprintTime + sprintCooldown)
                {
                    if (stamina >5f)
                    {
                        stamina -= 5;
                    }
                    LastSprintTime = Time.time;

                }
            }
            else
            {
                // Volta para a velocidade normal de forma gradual
                speed = Mathf.Lerp(speed, InitialSpeed, Time.deltaTime * 2f);
                //volta o fov da camera para o normal
                playerCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, Time.deltaTime * 2f);
            }

            if (isGrounded && InputSystem.actions.FindAction("Jump").ReadValue<float>() > 0.1f && !isInAir)
            {
                // Se o player estiver no chao e pressionar o bottao de pulo, aplica uma forca de impulso para cima
                anim.SetBool("Jump", true);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                // Se o player nao estiver no chao, reseta a animação de pulo
                anim.SetBool("Jump", false);
            }

        
    }
    }
}
