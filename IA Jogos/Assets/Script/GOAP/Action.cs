using System.Collections.Generic;

public abstract class Action
{
    public string Name { get; private set; }
    public Dictionary<string, bool> Preconditions { get; private set; }
    public Dictionary<string, bool> Effects { get; private set; }

    public Action(string name, Dictionary<string, bool> preconditions, Dictionary<string, bool> effects)
    {
        Name = name;
        Preconditions = preconditions;
        Effects = effects;
    }

    public bool IsApplicable(Dictionary<string, bool> state)
    {
        foreach (var precondition in Preconditions)
        {
            if (!state.ContainsKey(precondition.Key) || state[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }

    public Dictionary<string, bool> Apply(Dictionary<string, bool> state)
    {
        var newState = new Dictionary<string, bool>(state);
        foreach (var effect in Effects)
        {
            newState[effect.Key] = effect.Value;
        }
        return newState;
    }

    public abstract void Execute();
}