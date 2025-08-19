using Ilumisoft.HealthSystem;
using UnityEngine;

public class healthPicker : MonoBehaviour
{
    private bool picked;
    public float Amount = 25f;
    public GameObject toDesactive;
    public GameObject toActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !picked)
        {
            picked = true;
            GameObject player = other.gameObject;
            spellcaster spellcaster = player.GetComponent<spellcaster>();
            if (spellcaster. health < 100f)
            {
                toDesactive.SetActive(false);
                toActive.SetActive(true);
                spellcaster.health += Amount;
                if (spellcaster.health > 100)
                {
                    spellcaster.health = 100;
                }
                spellcaster.healthText.text = "Health: " + spellcaster.health.ToString("F0"); // Update mana text UI
                Destroy(gameObject, 2f);
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
