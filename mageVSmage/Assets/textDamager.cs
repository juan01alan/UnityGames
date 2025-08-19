using TMPro;
using UnityEngine;

public class textDamager : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    public void setDamage (float damage)
    {
        Camera main = Camera.main;
        transform.LookAt( main.transform.position);
        textMeshPro.text = "- " + damage.ToString();
        float Rand = (int)Random.Range(1, 2);
        Rand = Mathf.Clamp(Rand, 1,2);
        if (Rand == 1)
        {
            textMeshPro.color = Color.red;
        }
        else
        {
            textMeshPro.color= Color.yellow;
        }
            Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
