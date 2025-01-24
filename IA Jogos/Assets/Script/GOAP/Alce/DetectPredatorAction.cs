using System.Collections.Generic;
using UnityEngine;

public class DetectPredatorAction : Action
{
    private Alce2D alce;

    public DetectPredatorAction(Alce2D alce) : base("Detect Predator", new Dictionary<string, bool> { }, new Dictionary<string, bool> { { "PredatorNearby", true } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        // Implementar lógica para detectar predadores
        Debug.Log("Detectando predadores...");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(alce.transform.position, 5.0f); // Raio de detecção de 5 unidades
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("predator"))
            {
                alce.state["PredatorNearby"] = true;
                return;
            }
        }
        alce.state["PredatorNearby"] = false;
    }
}