using System.Collections.Generic;
using UnityEngine;

public class FindFoodAction : Action
{
    private Alce2D alce;

    public FindFoodAction(Alce2D alce) : base("Find Food", new Dictionary<string, bool> { { "Hungry", true } }, new Dictionary<string, bool> { { "HasFood", true } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        Debug.Log("Procurando comida...");
        Vector2 foodLocation = FindNearestFood();
        var path = alce.pathfinding.FindPath(alce.transform.position, foodLocation, alce.terrainMap);
        if (path != null && path.Count > 1)
        {
            alce.pathfinding.MoveTo(path[1]);
        }
    }

    private Vector2 FindNearestFood()
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();
        frontier.Enqueue(alce.transform.position);

        while (frontier.Count > 0)
        {
            Vector2 current = frontier.Dequeue();
            if (IsFoodLocation(current))
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

        return alce.transform.position; // Retorna a posição atual se não encontrar comida
    }

    private bool IsFoodLocation(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("food"))
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector2> GetNeighbors(Vector2 position)
    {
        return new List<Vector2>
        {
            position + Vector2.up,
            position + Vector2.down,
            position + Vector2.left,
            position + Vector2.right
        };
    }
}