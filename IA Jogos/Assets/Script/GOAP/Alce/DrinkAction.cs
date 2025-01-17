using System.Collections.Generic;
using UnityEngine;

public class DrinkAction : Action
{
    private Alce2D alce;

    public DrinkAction(Alce2D alce) : base("Drink", new Dictionary<string, bool> { { "HasWater", true } }, new Dictionary<string, bool> { { "Thirsty", false } })
    {
        this.alce = alce;
    }

    public override void Execute()
    {
        // Implementar lógica para beber
        Debug.Log("Bebendo...");
        // Aqui você pode adicionar a lógica para consumir a água e satisfazer a sede
        alce.state["Thirsty"] = false;
        alce.state["HasWater"] = false;
    }
}