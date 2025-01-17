using System.Collections.Generic;

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

        while (!goal.IsSatisfied(state))
        {
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
                return null; // No applicable actions found
            }

            state = applicableAction.Apply(state);
            plan.Add(applicableAction);
        }

        return plan;
    }
}