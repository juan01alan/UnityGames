using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class playerFPS : MonoBehaviour
{
    public float speed;
    public bool isFiring = false;
    public Transform bulletLoc;
    public float Aceleration = 10.0f; // Acceleration for the player movement
    public GameObject bulletPrefab;
    public float LookSpeed = 10.0f;
    public float currentSpeed = 0.0f; // Current speed of the player
    public float Life = 100.0f;
    public float CurBulletsMax = 50f;
    public GameObject[] SpawnLocs;
    public float MovedSeconds = 0f;
    public float CurBullets = 50;
    public float ReloadTime = 2.0f;
    public GameObject EnemyPrefab;
    public float Points;
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject HitEffect;
    public float CurRoom = 1f;
    public float Damage = 10.0f;
    public float bulletSpeed = 20.0f; // Speed of the bullet
    public Vector3 directionB; // Direction in which the bullet will be fired
    public Renderer armaR;
    public AudioSource audioSource; // Reference to the AudioSource component for firing sound
    public AudioClip fire;
    public AudioClip reloadA;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI.text = "Points: " + (Points > 999 ? "MEGAPOINTs" : Points);

        for (int i = 0; i < 5; i++)
        {
            GameObject selectedSpawn = SpawnLocs[UnityEngine.Random.Range(1, SpawnLocs.Length)];
            GameObject Enemy = Instantiate(EnemyPrefab, selectedSpawn.transform.position, selectedSpawn.transform.rotation);
        }
    }
    public void ReceivePoint(float point)
    {
        Points+=point; 
        textMeshProUGUI.text = "Points: " + (Points > 999 ? "MEGAPOINTs" : Points);

        if (Points > 4 * CurRoom)
        {
            CurRoom++;
            SpawnEnemys();
        }
    }
    public void SpawnEnemys()
    {
        Life = 110;
        CurBullets = CurBulletsMax;
        textMeshProUGUI.text = "Points: " + (Points > 999 ? "MEGAPOINTs" : Points);
        Debug.Log("Spawning new enemies for room " + CurRoom);
        // Destroy all existing enemies in the scene
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }
        // Spawn new enemies
        for (int i = 0; i < 5; i++)
        {
            GameObject selectedSpawn = SpawnLocs[UnityEngine.Random.Range(1, SpawnLocs.Length)];
            GameObject Enemy = Instantiate(EnemyPrefab, selectedSpawn.transform.position, selectedSpawn.transform.rotation);
        }
    }

    private IEnumerator Delay(float DelayTime)
    {
        yield return new WaitForSeconds(ReloadTime);
    }
    IEnumerator Reload(float DelayTime)
    {
        yield return new WaitForSeconds(2);
        CurBullets = CurBulletsMax;
        Debug.Log("Reloaded...");
        isFiring = false;
    }

    public void TakeDamage(float damageAmount)
    {
        Life -= damageAmount;
        if (Life <= 0)
        {
            Debug.Log("Player is dead");
            // Here you can add logic for player death, like respawning or ending the game
            SceneManager.LoadScene(0); // Reloads the current scene
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Movement with net Input System
        Vector2 move = InputSystem.actions["Move"].ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(0, 0, move.y);
        float unMoveTime = 0.0f;
        float MoveTime = 0f;
        float curAceleration = 0.0f;
        if (move.y != 0)
        {
            //increase the moveTime every second if the player is moving
            MoveTime = Time.time;
            if (MoveTime - unMoveTime > 1f && MovedSeconds <= 140)
            {

                MovedSeconds += 1f;
            }
            if (MovedSeconds >=120)
            {
                MovedSeconds = 20;
            }
            curAceleration = MovedSeconds/ 100 * Aceleration; // Calculate the current acceleration based on the moveTime
        } else
        {
            MovedSeconds = 0f;
            curAceleration = 0f;
            MoveTime = 0.0f;
            unMoveTime = Time.time;
        }
        currentSpeed = speed + curAceleration;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= currentSpeed * Time.deltaTime;
        transform.position += moveDirection;
        transform.Rotate(0, move.x * LookSpeed * Time.deltaTime, 0);

        if (InputSystem.actions["Reload"].WasPerformedThisFrame())
        {
            isFiring = true;
            StartCoroutine(Reload(ReloadTime));
            if (audioSource != null)
            {
                audioSource.PlayOneShot(reloadA, 0.7f); // Play the firing sound
            }
        }
        if (InputSystem.actions["Fire"].WasPerformedThisFrame())
        {
            if (!isFiring && CurBullets > 0)
            {
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(fire,1f); // Play the firing sound
                }

                CurBullets--;
                isFiring = true;
                GameObject bullet = Instantiate(bulletPrefab, bulletLoc.position, bulletLoc.rotation);
                GameObject hitEffect = Instantiate(HitEffect, bulletLoc.position, bulletLoc.rotation);
                bullet.GetComponent<bullet>().Damage = Damage; // Set the bullet's damage
                bullet.GetComponent<bullet>().selfAttached = gameObject.gameObject; // Set the bullet's damage
                directionB = transform.forward; // Get the forward direction of the player
                bullet.GetComponent<bullet>().Fire(directionB, bulletSpeed); // Fire the bullet with a speed of 20.0f
                //
                isFiring = false;
            }
        }
        if (CurBullets <= 0)
        {
            armaR.material.color = Color.red;
        }
        else
        {
            armaR.material.color = Color.yellow;
        }

    }
}
