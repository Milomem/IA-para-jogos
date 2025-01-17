using System.Collections.Generic;

public class Goal
{
    public string Name { get; private set; }
    public Dictionary<string, bool> Conditions { get; private set; }

    public Goal(string name, Dictionary<string, bool> conditions)
    {
        Name = name;
        Conditions = conditions;
    }

    public bool IsSatisfied(Dictionary<string, bool> state)
    {
        foreach (var condition in Conditions)
        {
            if (!state.ContainsKey(condition.Key) || state[condition.Key] != condition.Value)
            {
                return false;
            }
        }
        return true;
    }
}