using System.Collections.Generic;
using UnityEngine;

public class EatAction : Action
{
    private Alce2D alce;

    public EatAction(Alce2D alce) : base("Eat", new Dictionary<string, bool> { { "HasFood", true } }, new Dictionary<string, bool> { { "Hungry", false } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        // Implementar lógica para comer
        Debug.Log("Comendo...");
        // Aqui você pode adicionar a lógica para consumir a comida e satisfazer a fome
        alce.state["Hungry"] = false;
        alce.state["HasFood"] = false;
    }
}