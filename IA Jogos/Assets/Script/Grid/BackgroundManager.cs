using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    public List<GameObject> enemies; // Lista de inimigos neste background
    public Transform player;         // Referência ao Player

    private void Start()
    {
        // Encontra o player na cena
        player = GameObject.FindWithTag("Player").transform;
        // Desativa a lógica dos inimigos no início
        ToggleEnemyLogic(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o Player entrou no background
        if (other.CompareTag("Player"))
        {
            // Ativa a lógica dos inimigos quando o Player entra no background
            ToggleEnemyLogic(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Verifica se o Player saiu do background
        if (other != null && other.gameObject != null && other.CompareTag("Player"))
        {
            // Desativa a lógica dos inimigos quando o Player sai do background
            ToggleEnemyLogic(false);
        }
    }

    // Adiciona um inimigo à lista de inimigos
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    // Remove um inimigo da lista de inimigos
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    // Função para ativar ou desativar a lógica dos inimigos
    void ToggleEnemyLogic(bool state)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                EnemyFollow enemyFollow = enemy.GetComponent<EnemyFollow>();
                if (enemyFollow != null)
                {
                    enemyFollow.enabled = state; // Ativa ou desativa o componente EnemyFollow
                }
            }
        }
    }
}
