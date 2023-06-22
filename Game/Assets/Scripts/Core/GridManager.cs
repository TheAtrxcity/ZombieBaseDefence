using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Transform _cameraTransform;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3Int(x, y), Quaternion.identity);

                tile.transform.SetParent(GameObject.Find("_Tiles").transform);
                tile.name = $"Tile [{x}, {y}]";
            }
        }

        _cameraTransform.position = new Vector3Int(_gridWidth / 2 - 1, _gridHeight / 2 - 1, -10);
    }
}
