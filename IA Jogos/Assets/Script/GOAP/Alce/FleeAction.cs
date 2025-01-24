using System.Collections.Generic;
using UnityEngine;

public class FleeAction : Action
{
    private Alce2D alce;

    public FleeAction(Alce2D alce) : base("Flee", new Dictionary<string, bool> { { "PredatorNearby", true } }, new Dictionary<string, bool> { { "Safe", true } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        // Implementar lógica para fugir
        Debug.Log("Fugindo...");
        // Aqui você pode adicionar a lógica para fugir de predadores
        Vector2 safeLocation = FindSafeLocation();
        var path = alce.pathfinding.FindPath(alce.transform.position, safeLocation, alce.terrainMap);
        if (path != null && path.Count > 1)
        {
            alce.pathfinding.MoveTo(path[1]);
        }
    }

    private Vector2 FindSafeLocation()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("shelter"))
            {
                return true;
            }
        }
        return false;
    }
}