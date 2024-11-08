using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(healAmount); // Curar a quantidade especificada
            Destroy(gameObject); // Destruir o item de cura após a colisão
        }
    }
}
