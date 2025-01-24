using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    private List<Action> actions;

    public GOAPPlanner(List<Action> actions)
    {
        this.actions = actions;
    }

    public List<Action> Plan(Dictionary<string, bool> startState, Goal goal)
    {
        var plan = new List<Action>();
        var state = new Dictionary<string, bool>(startState);
        int maxIterations = 100; // Limite de iterações para evitar loops infinitos
        int iterations = 0;

        while (!goal.IsSatisfied(state))
        {
            if (iterations >= maxIterations)
            {
                Debug.LogError("GOAPPlanner: Excedido o número máximo de iterações. Verifique as condições do objetivo e as ações disponíveis.");
                return null;
            }

            Action applicableAction = null;
            foreach (var action in actions)
            {
                if (action.IsApplicable(state))
                {
                    applicableAction = action;
                    break;
                }
            }

            if (applicableAction == null)
            {
                Debug.LogError("GOAPPlanner: Nenhuma ação aplicável encontrada. Verifique as condições do objetivo e as ações disponíveis.");
                return null; // Nenhuma ação aplicável encontrada
            }

            Debug.Log("Aplicando ação: " + applicableAction.Name);
            state = applicableAction.Apply(state);
            plan.Add(applicableAction);
            iterations++;

            // Log do estado atual após aplicar a ação
            Debug.Log("Estado atual após aplicar a ação:");
            foreach (var kvp in state)
            {
                Debug.Log(kvp.Key + ": " + kvp.Value);
            }
        }

        return plan;
    }
}