using System.Collections.Generic;
using UnityEngine;

public class ExploreAction : Action
{
    private Alce2D alce;

    public ExploreAction(Alce2D alce) : base("Explore", new Dictionary<string, bool> { }, new Dictionary<string, bool> { { "Exploring", true } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        // Implementar lógica para explorar
        Debug.Log("Explorando...");
        // Aqui você pode adicionar a lógica para mover o alce para uma nova posição aleatória
        Vector2 exploreLocation = FindRandomLocation();
        var path = alce.pathfinding.FindPath(alce.transform.position, exploreLocation, alce.terrainMap);
        if (path != null && path.Count > 1)
        {
            alce.pathfinding.MoveTo(path[1]);
        }
    }

    private Vector2 FindRandomLocation()
    {
        // Implementar lógica para encontrar uma localização aleatória
        // Exemplo: retornar uma posição aleatória dentro de um certo raio
        float radius = 10.0f;
        Vector2 randomDirection = Random.insideUnitCircle * radius;
        return (Vector2)alce.transform.position + randomDirection;
    }
}