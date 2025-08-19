using System.Collections;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public AudioSource AudioSource; // Reference to the AudioSource component for firing sound
    public AudioClip FireSound; // Sound to play when the bullet is fired
    public SphereCollider SphereCollider;
    public Rigidbody Rigidbody;
    public GameObject EffectPrefab;
    public GameObject selfAttached; // Reference to the bullet GameObject
    public float Damage = 10.0f; // Damage dealt by the bullet
    public Vector3 Direction; // Direction in which the bullet will be fired
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AudioSource = player.GetComponent<AudioSource>() ;
        Destroy(gameObject,2f); // Destroy the bullet after 4 seconds
    }

    public void Fire(Vector3 direction, float speed)
    {
        Direction = direction.normalized; // Normalize the direction vector
        Rigidbody.linearVelocity = Direction * speed; // Set the bullet's velocity
    }
    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayOneShot(FireSound, 0.5f); // Play the firing sound
        //spawn hit particle at position
        GameObject Effect = Instantiate(EffectPrefab, transform.position, transform.rotation);
        if (other.CompareTag("Enemy") && other.gameObject != selfAttached)
        {
            other.GetComponent<enemy>().TakeDamage(Damage); // Assuming enemy has a TakeDamage method
            Debug.Log("Bullet hit an enemy!");
        }

        if (other.CompareTag("Player") && other.gameObject != selfAttached)
        {
            other.GetComponent<playerFPS>().TakeDamage(Damage); // Assuming enemy has a TakeDamage method
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
