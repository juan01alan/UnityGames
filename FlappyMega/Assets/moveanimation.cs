using UnityEngine;

public class moveanimation : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float resetPositionX = 10f;
    [SerializeField] private float startPositionX = -10f;

    void Update()
    {
        // Move o sprite para a direita
        transform.position += Vector3.right * speed * Time.deltaTime;


        // Se passar do ponto de reset, volta para o início
        if (transform.position.x > resetPositionX)
        {
            Vector3 pos = transform.position;
            pos.x = startPositionX;
            transform.position = pos;
        }
    }
}
