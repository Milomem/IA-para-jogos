using System.Collections.Generic;
using UnityEngine;

public class Pathfinding2D : MonoBehaviour
{
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector2 destination)
    {
        Vector2 direction = (destination - rb2D.position).normalized;
        rb2D.MovePosition(rb2D.position + direction * Time.fixedDeltaTime);
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 goal, Dictionary<Vector2, string> terrainMap)
    {
        var openSet = new List<Vector2> { start };
        var cameFrom = new Dictionary<Vector2, Vector2>();
        var gScore = new Dictionary<Vector2, float> { [start] = 0 };
        var fScore = new Dictionary<Vector2, float> { [start] = Heuristic(start, goal) };

        while (openSet.Count > 0)
        {
            var current = GetLowestFScore(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            foreach (var neighbor in GetNeighbors(current))
            {
                float tentativeGScore = gScore[current] + TerrainCost.GetCost(terrainMap[neighbor]);
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    private Vector2 GetLowestFScore(List<Vector2> openSet, Dictionary<Vector2, float> fScore)
    {
        float lowestScore = float.MaxValue;
        Vector2 lowestNode = openSet[0];
        foreach (var node in openSet)
        {
            if (fScore.ContainsKey(node) && fScore[node] < lowestScore)
            {
                lowestScore = fScore[node];
                lowestNode = node;
            }
        }
        return lowestNode;
    }

    private List<Vector2> GetNeighbors(Vector2 node)
    {
        // Implementar lógica para obter vizinhos em um grid 2D
        return new List<Vector2>
        {
            node + Vector2.up,
            node + Vector2.down,
            node + Vector2.left,
            node + Vector2.right
        };
    }

    private float Heuristic(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }

    private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        var path = new List<Vector2> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}