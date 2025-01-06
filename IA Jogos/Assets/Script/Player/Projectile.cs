using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 10; // Adicionar variável de dano

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calcular a direção do mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        // Mover o projétil na direção do mouse
        rb.linearVelocity = direction * speed;

        // Destruir o projétil após um tempo de vida
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se o objeto colidido possui o componente Health
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            // Causar dano ao objeto
            health.TakeDamage(damage);
        }

        // Destruir o projétil ao colidir com outro objeto
        Destroy(gameObject);
    }
}
