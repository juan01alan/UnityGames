using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class enemy : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the enemy
    public bool SeePlayer = false; // Flag to check if the enemy sees the player
    public GameObject Player; // Reference to the player transform
    public float SightRange = 50.0f; // Range within which the enemy can see the player
    public float AttackRange = 20.0f; // Range within which the enemy can attack the player
    public float AttackCooldown = 1.0f; // Time between attacks
    public float Life = 50.0f; // Health of the enemy
    public bool isMoving = false; // Flag to check if the enemy is currently moving
    public float Damage = 15.0f; // Damage the enemy can inflict
    public NavMeshAgent m_NavAgent; // Reference to the NavMeshAgent component for movement
    public float MovementRadius = 50.0f; // Radius within which the enemy can move randomly
    public Vector3 bulletVec; // Vector for bullet direction, if needed
    public float LookSpeed = 10.0f; // Speed at which the enemy rotates to look at the player
    public float curBulletsMax = 50; // Maximum bullets the enemy can have
    public float CurBullets = 50; // Current bullets the enemy has
    public float ReloadTime = 2.0f; // Time it takes to reload
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform bulletLoc; // Location where the bullet will be instantiated
    public float bulletSpeed = 20.0f; // Speed of the bullet
    public Vector3 directionB; // Direction in which the bullet will be fired
    private bool isFiring = false; // Flag to check if the enemy is currently firing
    private float lastUsedTime;
    public bool isReloading;
    public float LastMovedTime;
    public float WaitBetweenMov = 4f;
    public Transform PlayerLocToEnemy;
    public GameObject Capsule;
    public Renderer r;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); // Find the player by tag
        GameObject playerLoc = GameObject.FindGameObjectWithTag("PlayerLoc");
        PlayerLocToEnemy = playerLoc.transform;
    }
    public void Move(Vector3 direction)
    {
        if (m_NavAgent != null)
        {
            LastMovedTime = Time.time;

            m_NavAgent.SetDestination(transform.position + direction);
            transform.LookAt(transform.position + direction); 
        }
        else
        {
            Debug.LogWarning("NavMeshAgent component is not assigned to the enemy.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != this.gameObject && collision.gameObject.tag != "Player")
        {
            m_NavAgent.ResetPath();
            r.material.color = Color.green;
            isMoving = true;
            //get random navmesh point and move
            Vector3 randomDirection = Random.insideUnitSphere * MovementRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, MovementRadius, 1))
            {
                finalPosition = hit.position;
                Move(finalPosition);
            }
        }
    }
    private void AttackPlayer()
    {
        if (Player != null && Vector3.Distance(transform.position, Player.transform.position) <= AttackRange && Time.time > lastUsedTime + AttackCooldown)
        {
            if (!isFiring && CurBullets > 0)
            {

                CurBullets--;
                lastUsedTime = Time.time;
                isFiring = true;
                m_NavAgent.isStopped = true; // Stop moving after attacking
                //rotate towards player
                transform.LookAt(transform);
                GameObject bullet = Instantiate(bulletPrefab, bulletLoc.position, bulletLoc.rotation);
                bullet.GetComponent<bullet>().Damage = Damage; // Set the bullet's damage
                bullet.GetComponent<bullet>().selfAttached = gameObject.gameObject; // Set the bullet's damage
                directionB = transform.forward; // Get the forward direction of the player
                bullet.GetComponent<bullet>().Fire(directionB, bulletSpeed); // Fire the bullet with a speed of 20.0f
                                                                             //

                isFiring = false;
                m_NavAgent.isStopped = false; // Stop moving after attacking

            }
        }
    }
    protected bool pathComplete()
    {

        if (Vector3.Distance(m_NavAgent.destination, m_NavAgent.transform.position) <= m_NavAgent.stoppingDistance)
        {
            if (!m_NavAgent.hasPath || m_NavAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }
    public void TakeDamage(float damageAmount)
    {
        Life -= damageAmount;
        if (Life <= 0)
        {
            playerFPS playerFPS = Player.GetComponent<playerFPS>();
            playerFPS.ReceivePoint(1f);
            Debug.Log("Enemy is dead");
            // Here you can add logic for enemy death, like playing an animation or dropping loot
            Destroy(gameObject); // Destroy the enemy game object
        }
    }
    private IEnumerator reload() {
        yield return new WaitForSeconds(2f);
        CurBullets = curBulletsMax;
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_NavAgent != null)
        {
            Debug.Log(m_NavAgent.pathStatus);
            if (m_NavAgent.pathStatus.Equals("PathComplete"))
            {
                isMoving = false; 
            } else if (m_NavAgent.pathStatus.Equals("PathPartial"))
            {

            }
            else
            {
                isMoving = false;
            }
            if (CurBullets <= 0 && !isReloading)
            {
                isReloading = true;
                reload();
            }
            if (SeePlayer)
            {
                transform.LookAt(Player.transform);
            }
            if (!isMoving && Time.time > LastMovedTime + WaitBetweenMov)
            {
                //if player is inside sight range, seePlayer
                if (Vector3.Distance(transform.position, Player.transform.position) <= SightRange && Vector3.Distance(transform.position, Player.transform.position) >= AttackRange)
                {
                    r.material.color = Color.yellow;
                    SeePlayer = true;
                    Move(PlayerLocToEnemy.position);
                }
                //player is inside attack range
                if (Vector3.Distance(transform.position, Player.transform.position) <= AttackRange && !isFiring && !isReloading)
                {
                    r.material.color = Color.red;
                    SeePlayer = true;
                    AttackPlayer();
                }
                if (Vector3.Distance(transform.position, Player.transform.position) >= SightRange)
                {
                    SeePlayer = false;
                }
                if (!SeePlayer && !isFiring)
                {
                    r.material.color = Color.green;
                    isMoving = true;
                    //get random navmesh point and move
                    Vector3 randomDirection = Random.insideUnitSphere * MovementRadius;
                    randomDirection += transform.position;
                    NavMeshHit hit;
                    Vector3 finalPosition = Vector3.zero;
                    if (NavMesh.SamplePosition(randomDirection, out hit, MovementRadius, 1))
                    {
                        finalPosition = hit.position;
                        Move(finalPosition);
                    }
                }
            }
            else
            { //Resetar isMoving se o path for concluido ou o tempo tiver longo demais
                if (Time.time > LastMovedTime + WaitBetweenMov)
                {
                    isMoving = false;
                }
            }
        }
    }
}
