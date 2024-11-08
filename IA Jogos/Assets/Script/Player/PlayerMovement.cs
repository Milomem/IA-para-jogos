using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento

    void Update()
    {
        // Movimentação horizontal e vertical
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Movimenta o Player diretamente
        transform.Translate(new Vector2(moveX, moveY));
    }
}
