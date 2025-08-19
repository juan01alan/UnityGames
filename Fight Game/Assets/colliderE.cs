using UnityEngine;

public class colliderE : MonoBehaviour
{
    public enemyScript EnemScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        //check exact hit position

        EnemScript.ColliderAction(other, this.gameObject);// Call the method from playerScript when a collision occurs
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
