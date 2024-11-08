using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 3;
    public int columns = 3;
    public float cellSize = 10f; // Tamanho de cada célula da malha
    public GameObject combinedPrefab; // Prefab combinado para célula e background

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 cellPosition = new Vector2(col * (cellSize + 4), row * (cellSize - 1));
                // Instanciar o prefab combinado na posição da célula
                GameObject combinedInstance = Instantiate(combinedPrefab, new Vector3(cellPosition.x, cellPosition.y, 0), Quaternion.identity);
                
                // Inicializa o BackgroundManager
                BackgroundManager backgroundManager = combinedInstance.GetComponent<BackgroundManager>();
                if (backgroundManager != null)
                {
                    // Não é mais necessário inicializar com o player
                }
            }
        }
    }
}