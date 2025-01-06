using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player; // Referência ao jogador no Inspector
    public float detectionRange = 5f; // Alcance de detecção
    public float patrolSpeed = 2f; // Velocidade de patrulha
    public float chaseSpeed = 5f; // Velocidade de perseguição
    public float moveSpeed = 3f; // Velocidade de movimento
    public Transform healthPack; // Referência ao kit de vida no Inspector
    public float playerLowHealth = 20f; // Valor de saúde baixa do jogador
    public float safeDistance = 10f; // Distância segura para fugir do inimigo
    public float maxRunDistance = 15f; // Distância máxima que o NPC pode percorrer ao fugir
    private Vector3 initialPosition; // Posição inicial do NPC
    private bool hasReachedMaxDistance = false; // Flag para verificar se a distância máxima foi atingida

    private Node root; // Raiz da árvore de comportamento
    private Vector3 patrolPoint; // Ponto de patrulha

    private void Start()
    {
        // Construção da árvore de comportamento
        root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskNode(IsPlayerHealthLow),
                new TaskNode(BringHealthToPlayer)
            }),
            new TaskNode(FollowPlayerAndAvoidEnemies)
        });

        initialPosition = transform.position; // Armazena a posição inicial do NPC
    }

    private void Update()
    {
        // Avalia a árvore de comportamento a cada quadro
        root.Evaluate();
    }

    // --------------------------------------------
    // Tasks Implementadas como Métodos
    // --------------------------------------------

    // Tarefa: Verifica se a saúde do jogador está baixa
    private Node.NodeState IsPlayerHealthLow()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null && playerHealth.GetCurrentHealth() <= playerLowHealth)
        {
            return Node.NodeState.Success;
        }
        return Node.NodeState.Failure;
    }

    // Tarefa: Levar vida para o jogador
    private Node.NodeState BringHealthToPlayer()
    {
        GameObject[] healthPacks = GameObject.FindGameObjectsWithTag("Heal");
        if (healthPacks.Length == 0)
        {
            return Node.NodeState.Failure;
        }

        GameObject nearestHealthPack = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject pack in healthPacks)
        {
            float distance = Vector3.Distance(transform.position, pack.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestHealthPack = pack;
            }
        }

        if (nearestHealthPack != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, nearestHealthPack.transform.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nearestHealthPack.transform.position) < 1f)
            {
                Health playerHealth = player.GetComponent<Health>();
                HealingItem healingItem = nearestHealthPack.GetComponent<HealingItem>();
                if (playerHealth != null && healingItem != null)
                {
                    playerHealth.Heal(healingItem.healAmount);
                    Destroy(nearestHealthPack); // Destruir o item de cura após usá-lo
                    return Node.NodeState.Success;
                }
            }
            return Node.NodeState.Running;
        }

        return Node.NodeState.Failure;
    }

    // Tarefa: Verifica se há inimigos perto do jogador
    private Node.NodeState IsEnemyNear()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(player.position, enemy.transform.position) <= detectionRange)
            {
                return Node.NodeState.Success;
            }
        }
        return Node.NodeState.Failure;
    }

    // Tarefa: Corre do inimigo mais próximo
    private Node.NodeState RunToSafeZone()
    {
        if (hasReachedMaxDistance)
        {
            return Node.NodeState.Success;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            return Node.NodeState.Failure;
        }

        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 directionAwayFromEnemy = (transform.position - nearestEnemy.transform.position).normalized;
            Vector3 fleeForce = directionAwayFromEnemy * moveSpeed;
            Vector3 newTargetPosition = transform.position + fleeForce * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newTargetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition) > maxRunDistance)
            {
                Debug.Log("Distância máxima atingida!");
                hasReachedMaxDistance = true;
                return Node.NodeState.Success;
            }

            if (Vector3.Distance(transform.position, nearestEnemy.transform.position) > safeDistance)
            {
                Debug.Log("Fugiu do inimigo!");
                return Node.NodeState.Success;
            }

            return Node.NodeState.Running;
        }

        return Node.NodeState.Failure;
    }

    // Tarefa: Seguir o jogador e desacelerar quando um inimigo se aproxima
    private Node.NodeState FollowPlayerAndAvoidEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        float orbitRadius = 3f; // Raio da órbita ao redor do jogador
        float orbitSpeed = 50f; // Velocidade angular da órbita

        Vector3 offset = new Vector3(Mathf.Sin(Time.time * orbitSpeed), 0, Mathf.Cos(Time.time * orbitSpeed)) * orbitRadius;
        Vector3 targetPosition = player.position + offset;

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        float currentSpeed = moveSpeed;

        if (nearestEnemy != null && Vector3.Distance(player.position, nearestEnemy.transform.position) <= detectionRange)
        {
            currentSpeed = Mathf.Lerp(moveSpeed, 0, (detectionRange - Vector3.Distance(player.position, nearestEnemy.transform.position)) / detectionRange);
        }

        Vector3 seekForce = directionToTarget * currentSpeed;
        Vector3 newTargetPosition = transform.position + seekForce * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, newTargetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            return Node.NodeState.Success;
        }
        return Node.NodeState.Running;
    }

    private void OnDrawGizmos()
    {
        // Desenha uma esfera ao redor do jogador para mostrar o alcance de detecção de inimigos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, detectionRange);
    }
}
