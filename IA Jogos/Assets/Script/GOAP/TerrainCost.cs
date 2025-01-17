using System.Collections.Generic;
using UnityEngine;

public static class TerrainCost
{
    public static Dictionary<string, int> costs = new Dictionary<string, int>
    {
        { "Plains", 1 },
        { "Forest", 3 },
        { "Mountain", 5 },
        { "Water", 10 }
    };

    public static int GetCost(string terrainType)
    {
        return costs.ContainsKey(terrainType) ? costs[terrainType] : int.MaxValue;
    }
}