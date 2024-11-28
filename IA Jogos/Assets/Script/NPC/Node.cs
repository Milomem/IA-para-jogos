using System.Collections.Generic;

public abstract class Node
{
    public enum NodeState { Running, Success, Failure }
    protected NodeState state;

    public abstract NodeState Evaluate();
}

public class Selector : Node
{
    private List<Node> nodes;

    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                case NodeState.Failure:
                    continue;
            }
        }
        state = NodeState.Failure;
        return state;
    }
}

public class Sequence : Node
{
    private List<Node> nodes;

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    state = NodeState.Failure;
                    return state;
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                case NodeState.Success:
                    continue;
            }
        }
        state = NodeState.Success;
        return state;
    }
}

public class TaskNode : Node
{
    private System.Func<NodeState> task;

    public TaskNode(System.Func<NodeState> task)
    {
        this.task = task;
    }

    public override NodeState Evaluate()
    {
        return task();
    }
}
