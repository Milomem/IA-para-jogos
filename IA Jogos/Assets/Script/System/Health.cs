using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Aqui você pode adicionar lógica para atualizar a saúde, se necessário
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        // Lógica para quando o objeto morre
        if (gameObject.CompareTag("Player"))
        {
            // Reiniciar o jogo se for o jogador
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Destruir o objeto se não for o jogador
            if (gameObject.GetComponent<Health>() != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        // Verificar se o objeto foi destruído
        if (gameObject != null)
        {
            // Lógica adicional, se necessário
        }
    }

}
