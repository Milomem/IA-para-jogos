using System.Collections.Generic;
using UnityEngine;

public class FindWaterAction : Action
{
    private Alce2D alce;

    public FindWaterAction(Alce2D alce) : base("Find Water", new Dictionary<string, bool> { { "Thirsty", true } }, new Dictionary<string, bool> { { "HasWater", true } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        Debug.Log("Procurando água...");
        Vector2 waterLocation = FindNearestWater();
        var path = alce.pathfinding.FindPath(alce.transform.position, waterLocation, alce.terrainMap);
        if (path != null && path.Count > 1)
        {
            alce.pathfinding.MoveTo(path[1]);
        }
    }

    private Vector2 FindNearestWater()
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();
        frontier.Enqueue(alce.transform.position);

        while (frontier.Count > 0)
        {
            Vector2 current = frontier.Dequeue();
            if (IsWaterLocation(current))
            {
                return current;
            }

            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && alce.terrainMap.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return alce.transform.position; // Retorna a posição atual se não encontrar água
    }

    private bool IsWaterLocation(Vector2 position)
    {
        // Verificar se a posição tem a tag "water"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("water"))
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector2> GetNeighbors(Vector2 position)
    {
        // Implementar lógica para obter vizinhos em um grid 2D
        return new List<Vector2>
        {
            position + Vector2.up,
            position + Vector2.down,
            position + Vector2.left,
            position + Vector2.right
        };
    }
}