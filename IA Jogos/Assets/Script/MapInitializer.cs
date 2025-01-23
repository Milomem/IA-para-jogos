using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapInitializer : MonoBehaviour
{
    public Tilemap terrainTilemap;
    public TileBase plainsTile;
    public TileBase forestTile;
    public TileBase mountainTile;
    public TileBase waterTile;

    private Dictionary<Vector2, string> terrainMap;

    void Start()
    {
        InitializeTerrainMap();
    }

    private void InitializeTerrainMap()
    {
        terrainMap = new Dictionary<Vector2, string>();

        foreach (var pos in terrainTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (!terrainTilemap.HasTile(localPlace)) continue;

            TileBase tile = terrainTilemap.GetTile(localPlace);
            Vector2 worldPos = terrainTilemap.CellToWorld(localPlace);

            if (tile == plainsTile)
            {
                terrainMap[worldPos] = "Plains";
            }
            else if (tile == forestTile)
            {
                terrainMap[worldPos] = "Forest";
            }
            else if (tile == mountainTile)
            {
                terrainMap[worldPos] = "Mountain";
            }
            else if (tile == waterTile)
            {
                terrainMap[worldPos] = "Water";
            }
        }

        // Passar o mapa de terrenos para o script Alce2D
        Alce2D alce = FindObjectOfType<Alce2D>();
        if (alce != null)
        {
            alce.terrainMap = terrainMap;
        }
    }
}