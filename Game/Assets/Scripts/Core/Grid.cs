using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridCellSize;
    [Space(10f)]
    [SerializeField] private Vector2Int _gridOrigin;

    public int Width => _gridWidth;
    public int Height => _gridHeight;
    public int CellSize => _gridCellSize;
    public Vector2 GridOrigin => _gridOrigin;

    private bool[,] _grid;

    private Camera _sceneCamera;

    private GameObject _cachedCellFreePreviewObject;
    private GameObject _cachedCellOccupiedPreviewObject;

    private void Awake()
    {
        _sceneCamera = Camera.main;

        _grid = new bool[_gridWidth, _gridHeight];

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Vector2 start = GetCellWorldPosition(x, y);

                Debug.DrawLine(start, (Vector2)GetCellWorldPosition(x + 1, y), Color.blue, 500f);
                Debug.DrawLine(start, (Vector2)GetCellWorldPosition(x, y + 1), Color.blue, 500f);
            }
        }
    }

    private Vector2Int GetCellWorldPosition(int x, int y)
    {
        Vector2Int vec = new Vector2Int
        {
            x = Mathf.FloorToInt(x),
            y = Mathf.FloorToInt(y)
        } * _gridCellSize + _gridOrigin;

        return vec;
    }

    private Vector2Int GetCellWorldPositionCentred(int x, int y)
    {
        Vector2Int worldPosition = GetCellWorldPosition(x, y);
        
        Vector2Int centred = worldPosition + new Vector2Int(_gridCellSize, _gridCellSize) / 2;

        return centred;
    }

    private (int x, int y) GetCellPositionFromWorld(Vector2 worldPosition)
    {
        int cellX = Mathf.FloorToInt((worldPosition - _gridOrigin).x / _gridCellSize);
        int cellY = Mathf.FloorToInt((worldPosition - _gridOrigin).y / _gridCellSize);

        return (cellX, cellY);
    }

    private bool IsInGridBounds(int x, int y) => (x >= 0 && y >= 0 && x < _gridWidth && y < _gridHeight);

    private bool IsCellOccupied(int x, int y) => _grid[x, y];

    private bool AreCellsFree(int x, int y, int width, int height)
    {
        if (x + width > _gridWidth || y + height > _gridHeight)
        {
            Debug.Log("Testing outside the grid is not possible.");
            return false;
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //Debug.Log($"[Debug]:: Testing -> {x + i}, {y + j}");
                //Debug.Log($"[Debug]:: Is Occupied -> {IsCellOccupied(x + i, y + j)}");

                if (IsCellOccupied(x + i, y + j)) { return false; }
            }
        }

        return true;
    }

    private void SetCellOccupation(int x, int y, bool newValue) => _grid[x, y] = newValue;

    private void SetCellOccupations(int x, int y, int width, int height, bool newValue)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _grid[x + i, y + j] = newValue;

                //Debug.Log($[Debug]:: "{x + i}, {y + j} set to occupied");
            }
        }
    }

    public bool AddBuilding(Vector2 worldPosition, int width, int height)
    {
        (int x, int y) = GetCellPositionFromWorld(worldPosition);

        if (IsInGridBounds(x, y))
        {
            if (!AreCellsFree(x, y, width, height)) { Debug.Log("Building failed"); return false; }

            SetCellOccupations(x, y, width, height, true);
            Debug.Log("Building built");
            return true;
        }

        return false;
    }

    public Vector2 GetPlacementPosition(Vector2 worldPosition)
    {
        (int x, int y) = GetCellPositionFromWorld(worldPosition);

        Vector2Int centred = GetCellWorldPositionCentred(x, y);

        return new Vector2(centred.x, centred.y);
    }

    public bool CanBuildHere(Vector2 worldPosition, int width, int height)
    {
        (int x, int y) = GetCellPositionFromWorld(worldPosition);

        if (IsInGridBounds(x, y))
        {
            if (AreCellsFree(x, y, width, height))
            {
                return true;
            }
        }

        return false;
    }
}
