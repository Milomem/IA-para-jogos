using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public enum State
    {
        Idle,
        Following,
        Fleeing,
        SeekingHealth
    }

    public Transform player; // Referência ao Player
    public float speed = 2f; // Velocidade de movimento do inimigo
    public float followRange = 2.5f; // Alcance para seguir o Player (diminuído pela metade)
    public Transform[] patrolPoints; // Pontos de patrulha
    public Transform healthPoint; // Ponto de cura
    public int lowHealthThreshold = 20; // Limite de vida baixa
    private int currentPatrolIndex = 0; // Índice do ponto de patrulha atual
    private State currentState = State.Idle; // Estado atual do inimigo
    private BackgroundManager backgroundManager; // Referência ao BackgroundManager
    public int damage = 10; // Quantidade de dano que o inimigo causa ao player
    private Health health; // Referência ao componente de saúde

    void Start()
    {
        backgroundManager = GetComponentInParent<BackgroundManager>();
        health = GetComponent<Health>();
        if (backgroundManager != null)
        {
            player = backgroundManager.player;
        }
    }

    void Update()
    {
        // Atualiza a referência ao player dinamicamente
        if (backgroundManager != null)
        {
            player = backgroundManager.player;
        }

        switch (currentState)
        {
            case State.Idle:
                Patrol();
                CheckPlayerDistance();
                CheckHealth();
                break;
            case State.Following:
                FollowPlayer();
                CheckHealth();
                break;
            case State.Fleeing:
                Flee();
                break;
            case State.SeekingHealth:
                SeekHealth();
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPatrolPoint.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void CheckPlayerDistance()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= followRange)
        {
            currentState = State.Following;
        }
    }

    void FollowPlayer()
    {
        // Se o inimigo está ativo e deve seguir o Player
        if (player != null)
        {
            // Move o inimigo na direção do Player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Verifica se o player saiu do alcance
            if (Vector2.Distance(transform.position, player.position) > followRange)
            {
                currentState = State.Idle;
            }
        }
    }

    void CheckHealth()
    {
        if (health != null && health.GetCurrentHealth() <= lowHealthThreshold)
        {
            currentState = State.Fleeing;
        }
    }

    void Flee()
    {
        if (player == null) return;

        // Move o inimigo na direção oposta ao Player
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Verifica se o inimigo está a uma distância segura do Player
        if (Vector2.Distance(transform.position, player.position) > followRange * 2)
        {
            currentState = State.SeekingHealth;
        }
    }

    void SeekHealth()
    {
        if (healthPoint == null) return;

        // Move o inimigo na direção do ponto de cura
        Vector2 direction = (healthPoint.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Verifica se o inimigo chegou ao ponto de cura
        if (Vector2.Distance(transform.position, healthPoint.position) < 0.2f)
        {
            // Lógica para curar o inimigo
            health.Heal(health.maxHealth);
            currentState = State.Idle;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o inimigo colidiu com o player
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Desenha o alcance de seguimento do inimigo em verde
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRange);

        // Desenha os pontos de patrulha em azul
        Gizmos.color = Color.blue;
        foreach (Transform patrolPoint in patrolPoints)
        {
            Gizmos.DrawWireSphere(patrolPoint.position, 0.2f);
        }

        // Desenha o ponto de cura em vermelho
        if (healthPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(healthPoint.position, 0.2f);
        }
    }
}
