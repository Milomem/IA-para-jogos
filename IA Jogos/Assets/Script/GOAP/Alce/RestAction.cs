using System.Collections.Generic;
using UnityEngine;

public class RestAction : Action
{
    private Alce2D alce;

    public RestAction(Alce2D alce) : base("Rest", new Dictionary<string, bool> { { "HasShelter", true } }, new Dictionary<string, bool> { { "Tired", false } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        // Implementar lógica para descansar
        Debug.Log("Descansando...");
        // Aqui você pode adicionar a lógica para descansar e recuperar energia
        alce.state["Tired"] = false;
        alce.state["HasShelter"] = false;
    }
}