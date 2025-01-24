using System.Collections.Generic;
using UnityEngine;

public class Alce2D : MonoBehaviour
{
    public Dictionary<string, bool> state;
    private List<Action> actions;
    private GOAPPlanner planner;
    private Goal currentGoal;
    public Pathfinding2D pathfinding;
    public Dictionary<Vector2, string> terrainMap;

    void Start()
    {
        state = new Dictionary<string, bool>
        {
            { "Hungry", true },
            { "Thirsty", true },
            { "Tired", false },
            { "Safe", true },
            { "PredatorNearby", false }
        };

        actions = new List<Action>
        {
            new FindFoodAction(this),
            new EatAction(this),
            new FindWaterAction(this),
            new DrinkAction(this),
            new FindShelterAction(this),
            new RestAction(this),
            new DetectPredatorAction(this),
            new FleeAction(this),
            new ExploreAction(this)
        };

        planner = new GOAPPlanner(actions);
        pathfinding = GetComponent<Pathfinding2D>();

        SetNextGoal();
        Debug.Log("Current Goal: " + currentGoal.Name);
    }

    void Update()
    {
        if (currentGoal == null || currentGoal.IsSatisfied(state))
        {
            SetNextGoal();
            Debug.Log("Current Goal: " + currentGoal.Name);
            foreach (var condition in currentGoal.Conditions)
            {
                Debug.Log("Condition: " + condition.Key + " = " + condition.Value);
            }
        }

        var plan = planner.Plan(state, currentGoal);
        if (plan != null && plan.Count > 0)
        {
            var action = plan[0];
            Debug.Log("Executando ação: " + action.Name);
            action.Execute();
            state = action.Apply(state);
            Debug.Log("Estado após executar a ação:");
            foreach (var kvp in state)
            {
                Debug.Log(kvp.Key + ": " + kvp.Value);
            }
        }
        else
        {
            Debug.LogError("Alce2D: Nenhum plano encontrado para o objetivo atual.");
        }
    }

    private void SetNextGoal()
    {
        if (state["PredatorNearby"])
        {
            currentGoal = new Goal("Flee", new Dictionary<string, bool> { { "Safe", true } });
        }
        else if (state["Hungry"])
        {
            currentGoal = new Goal("Find Food", new Dictionary<string, bool> { { "Hungry", false } });
        }
        else if (state["Thirsty"])
        {
            currentGoal = new Goal("Find Water", new Dictionary<string, bool> { { "Thirsty", false } });
        }
        else if (state["Tired"])
        {
            currentGoal = new Goal("Rest", new Dictionary<string, bool> { { "Tired", false } });
        }
        else
        {
            currentGoal = new Goal("Explore", new Dictionary<string, bool> { { "Exploring", true } });
        }
    }
}