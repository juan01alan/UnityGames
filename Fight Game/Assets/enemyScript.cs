using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

//create enum for states IA
public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public class enemyScript : MonoBehaviour
{
    public TextMeshPro textHealth;
    public EnemyState enemyState;

    [Header("GamePlay Variables")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float mana = 100f;
    public float maxMana = 100f;
    public float manaRegenCooldown = 5f;
    public float LastManaRegenTime = 0f;
    public float LifeRegenCooldown = 5f;
    public float LastLifeRegenTime = 0f;
    public float staminaRegenCooldown = 5f;
    public float LastStaminaRegenTime = 0f;
    public float LastSprintTime = 0f;
    public float sprintCooldown = 1f;

    [Header("IA things")]
    public GameObject Player;
    public float SeeDistance = 10f;
    public float AttackDistance = 2f;
    public float speed = 6f;
    public float MaxSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public bool isGrounded;
    public bool isJumping;
    public bool isInAir;
    public Animator anim;
    public Rigidbody rb;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public float InitialSpeed = 6f;
    public NavMeshAgent navMeshAgent;
    public float PatrolCD = 2f;
    public float LastPatrolTime = 0f;
    public float ChaseCD = 2f;
    public float LastChaseTime = 0f;
    public float AttackCD = 2f;
    public float LastAttackTime = 0f;
    public float Radius = 10f;

    [Header("Combat")]
    public float cooldownTime = 0.4f;
    //public float nextFireTime = 0f;
    public float Damage = 10f;
    public static int noOfClicks = 0;
    public float lastAttackedTime = 0;
    public float maxComboDelay = 1;
    public List<BoxCollider> attackHitboxes = new List<BoxCollider>();
    public GameObject[] hitEffects;
    public List<GameObject> playerInRange = new List<GameObject>();
    public GameObject[] hitPoints;
    public bool isAttacking = false;
    public float nextFireTime;
    public bool Died;
    public bool invulneravel;

    [Header("IA movement new System")]
    public LayerMask whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange = 20f;

    //Attacking
    public float timeBetweenAttacks = 0.4f;
    public bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool isHitting = false;
    public bool isAvoiding = false;

    public Vector3 VecPlayer { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent.SetDestination(transform.position);
        Player = GameObject.FindGameObjectWithTag("Player");
        playerScript playerScript = Player.GetComponent<playerScript>();
        maxHealth = playerScript.maxHealth-(10/100*playerScript.maxHealth);
        maxMana = playerScript.maxMana-(10/100*playerScript.maxMana);
        maxStamina = playerScript.maxStamina-(10/100*playerScript.maxStamina);
        Damage = playerScript.Damage-(10/100*playerScript.Damage);
        Invoke(nameof(ResetEnemySpawn), 3f);
    }
    void ResetEnemySpawn()
    {

        Player = GameObject.FindGameObjectWithTag("Player");
        playerScript playerScript = Player.GetComponent<playerScript>();
        playerScript.DeathHandler = false;
    }
    private void Patroling()
    {
        navMeshAgent.speed = speed;
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);


        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //check closiest location in navigation
        navMeshAgent.SetDestination(walkPoint);

    }
    private void SearchWalkPoint()
    {

        walkPointSet = true;
    }

    private void Avoid()
    {
        if (!isAvoiding)
        {
            float randomZ = Random.Range(-20f, 20f);
            float randomX = Random.Range(-20f, 20f);
            isAvoiding = true;
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            //check closiest location in navigation
            navMeshAgent.SetDestination(walkPoint);

            navMeshAgent.speed = speed;
        }

    }
    private void ChasePlayer()
    {
        if (isHitting)
        {
            float randomZ = Random.Range(-20f, 20f);
            float randomX = Random.Range(-20f, 20f);
            VecPlayer = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        }
        else
        {
            VecPlayer = Player.transform.position;
        }
            navMeshAgent.speed = speed;
        if (!navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(VecPlayer);
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        navMeshAgent.SetDestination(transform.position);

        transform.LookAt(Player.transform);

        if (!alreadyAttacked)
        {
            ///End of attack code
            Attack();
        }
    }
    private void ResetHit()
    {
        isHitting = false;
    }
    IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
        isAttacking = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    void Attack()
    {

            Debug.Log("Attacking Player");
            lastAttackedTime = Time.time;            
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

            //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
            noOfClicks++;

            isAttacking = true;
            if (noOfClicks == 1)
            {
                anim.SetBool("hit1", true);
            alreadyAttacked = true; isAvoiding = false;

            lastAttackedTime = Time.time;
            StartCoroutine(resetAttack());
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);


        if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
            {
                anim.SetBool("hit1", false);
                anim.SetBool("hit2", true);
            lastAttackedTime = Time.time;
            alreadyAttacked = true; isAvoiding = false;

            StartCoroutine(resetAttack());
        }
            if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
            {
                anim.SetBool("hit2", false);
                anim.SetBool("hit3", true);
            lastAttackedTime = Time.time;
            alreadyAttacked = true; isAvoiding = false;

            StartCoroutine(resetAttack());
        }
        


    }
    public void ColliderAction(Collider other, GameObject Member)
    {
        //RaycastHit hit;
        //Ray ray = new Ray(Member.transform.position, (other.ClosestPoint(Member.transform.position) - Member.transform.position).normalized);
        //if (Physics.Raycast(ray, out hit))
       // {
        //}
        //Vector3 hitPointPosition = hit.point;
        //Check if the collider is an enemy
        if (other.CompareTag("Player"))
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
                    playerScript playerScript = other.GetComponent<playerScript>();
                    playerScript.TakeDamage(Damage, this.gameObject);
                    audioSource.Play();

                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
                {
                    GameObject HITselect = hitEffects[Random.Range(0, hitEffects.Length)];

                    //get hit point in Enemy
                    GameObject effect = Instantiate(HITselect, Member.transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    playerScript playerScript = other.GetComponent<playerScript>();
                    playerScript.TakeDamage(Damage, this.gameObject);
                    audioSource.Play();

                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
                {

                    GameObject HITselect = hitEffects[Random.Range(0, hitEffects.Length)];

                    //get hit point in Enemy
                    GameObject effect = Instantiate(HITselect, Member.transform.position, Quaternion.identity);
                    Destroy(effect, 1f);
                    playerScript playerScript = other.GetComponent<playerScript>();
                    playerScript.TakeDamage(Damage, this.gameObject);
                    audioSource.Play();
                }

            }
        }
    }

    public void TakeDamage(float damage)
    {

        alreadyAttacked = true;
        alreadyAttacked = false;
        if (!isHitting)
        {
            isAvoiding = false;
            isHitting = true;
            health -= damage;
            alreadyAttacked = true;
            alreadyAttacked = false;
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", false);
            Debug.Log("take damage: " + damage);
            transform.LookAt(Player.transform);
            StartCoroutine(resetAttack());
            Invoke(nameof(ResetHit), timeBetweenAttacks);

            if (health <= 0)
            {
                Die();
            }
            else
            {
                anim.SetTrigger("Hit");
            }
        }
    }
    public void Die()
    {
        // Disable player controls
        rb.linearVelocity = Vector3.zero;
        Died = true;
        rb.isKinematic = true;
        navMeshAgent.enabled = false;
        rb.useGravity = false;
        playerScript playerScript = Player.GetComponent<playerScript>();
        playerScript.HandleEnemyDeath(this.gameObject);
        //useanim rootmotion to make the death animation play correctly
        anim.applyRootMotion = true;
        foreach (BoxCollider hitbox in attackHitboxes)
        {
            hitbox.enabled = false;
        }
        anim.SetTrigger("Die");
        Destroy(gameObject, 3f);
        this.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!Died)
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (navMeshAgent.velocity.magnitude >= 0.1f && !isAttacking && !isHitting)
            {
                anim.SetBool("Run", true);
            }
            else if(!isAttacking && !isHitting)
            {
                isAvoiding = false;
                anim.SetBool("Run", false);
            }
            if (Time.time >= LastPatrolTime + PatrolCD )
            {
                if (!playerInSightRange && !playerInAttackRange) Patroling(); 
                LastPatrolTime = Time.time;

            }
            if (Time.time >= LastChaseTime + ChaseCD ) 
            {
                LastChaseTime = Time.time;
            }
            if (playerInSightRange && !playerInAttackRange && !isAvoiding) ChasePlayer();
            if (playerInAttackRange && playerInSightRange && !isAvoiding) AttackPlayer();

            /*if (Time.time >= lastAttackedTime + AttackCD)
            {
            }*/

            textHealth.text = health.ToString("F0") + "/" + maxHealth.ToString("F0");
            //rotate text health object to player
            if (Player != null && textHealth != null)
            {
                Vector3 direction = Player.transform.position - textHealth.transform.position;
                direction.y = 0; // Mantém o texto na horizontal
                if (direction.sqrMagnitude > 0.001f)
                {
                    textHealth.transform.rotation = Quaternion.LookRotation(direction);
                }
            }
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

            //fim regenthings
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                isHitting = false;
                alreadyAttacked = true;
                alreadyAttacked = false;
                anim.ResetTrigger("Hit");
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
            {
                isAttacking = false;
                alreadyAttacked = true;
                alreadyAttacked = false;
                anim.SetBool("hit1", false);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
            {
                isAttacking = false;
                alreadyAttacked = true;
                alreadyAttacked = false;
                anim.SetBool("hit2", false);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
            {
                isAttacking = false;
                alreadyAttacked = true;
                alreadyAttacked = false;
                anim.SetBool("hit3", false);
                noOfClicks = 0;
            }


            if (Time.time - lastAttackedTime > maxComboDelay)
            {
                noOfClicks = 0;
                isAttacking = false;
            }
            //cooldown time
            if (Time.time > nextFireTime)
            {
                //Attack();
            }

            //aqui eu verifico se o enemy estta no chaoo, se ele esta pulando e se ele esta no ar
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.8f, groundLayer);
            isJumping = rb.linearVelocity.y > 0.1f;
            isInAir = !isGrounded && rb.linearVelocity.y > 0.1f;

            //setando o estado do inimigo baseado na distancia do player
        }
    
    }
}
