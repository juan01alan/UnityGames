using UnityEngine;

public class collider : MonoBehaviour
{
    public playerScript playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

    }
    private void OnTriggerEnter(Collider other)
    {
        //check exact hit position
        
        playerScript.ColliderAction(other, this.gameObject);// Call the method from playerScript when a collision occurs
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
