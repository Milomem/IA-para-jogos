using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Referência ao Player
    public Vector3 offset;         // Offset da posição do Player
    public float smoothSpeed = 0.125f; // Velocidade de suavização

    void LateUpdate()
    {
        // Posição desejada da câmera
        Vector3 desiredPosition = player.position + offset;
        
        // Suaviza a transição da posição atual para a posição desejada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }
}
