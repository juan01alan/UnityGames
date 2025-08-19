using ADAPT;
using UnityEngine;
using UnityEngine.AI;

public class wolfInvoked : MonoBehaviour
{
    public Animator anim;
    public GameObject enemy;
    public NavMeshAgent agent;
    public LayerMask enemyLayer;
    public GameObject Explode;
    public float radius = 40f;
    public float speed = 12;
    public Transform Destination;
    public GameObject Mesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if (hitColliders.Length > 0)
        {
            foreach (Collider col in hitColliders)
            {
                // You can further process the detected enemies here
                if (col.gameObject.CompareTag("Enemy"))
                {
                    enemy = col.gameObject;
                    Destination = col.gameObject.transform;

                    anim.SetFloat("Vert",1f);
                    anim.SetFloat("State", 1f);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") )
        {
            enemy = other.gameObject;
            agent.SetDestination(enemy.transform.position);
            Mesh.SetActive(false); // Hide the mesh after the explosion
            enemyAdmin adminEnemy = enemy.GetComponent<enemyAdmin>();
            if (adminEnemy != null)
            {
                adminEnemy.TakeDamage(20f); // Deal damage to the enemy
            }
            GameObject explosion = Instantiate(Explode, transform.position, Quaternion.identity);
            anim.SetFloat("Vert", 0f);
            this.enabled = false; // Disable this script after the explosion
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Destination != null)
        {
            // Calculate the direction to the target
            Vector3 direction = (Destination.position - transform.position).normalized;

            // Rotate towards the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 240f * Time.deltaTime);

            // Move forward
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, Destination.position) < 0.1f)
            {
                Destination = null;
                // Reached destination, do something or set a new target
            }
        }

    }
}
