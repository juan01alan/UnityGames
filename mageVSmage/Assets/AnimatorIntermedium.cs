using UnityEngine;

public class AnimatorIntermedium : MonoBehaviour
{
    public bool isEnemy;
    public AudioSource audioSource;
    public spellcaster spellCaster;
    public PlayerController playerControl;
    public float Damage = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerControl = player.GetComponent<PlayerControlHLh>();
        }
        else
        {
            //Debug.LogError("Player GameObject with tag 'Player' not found.");
        }*/
    }
    public void Roll()
    {
        playerControl.RollC();
    }
    public void AddMana()
    {
        if (spellCaster != null)
        {
            spellCaster.setMana(spellCaster.mana + 10f); // Assuming you want to add 10 mana
        }
    }
    public void ResetRoll() {
        if (playerControl != null)
        {
            playerControl.resetRoll();
        }
    }
    public void PlaySound(AudioClip cliper)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(cliper);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
