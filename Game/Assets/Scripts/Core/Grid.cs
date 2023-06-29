using System.Collections.Generic;

using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridCellSize;
    [Space(10f)]
    [SerializeField] private Vector2Int _gridOrigin;

    [Header("Visual Settings")]
    [SerializeField] private GameObject _cellFreePreviewPrefab;
    [SerializeField] private GameObject _cellOccupiedPreviewPrefab;

    [Header("Debug")]
    [SerializeField] private GameObject _debugPrefab;
    [SerializeField] private GameObject _debugPrefabBig;

    private bool[,] _grid;

    private List<GameObject> _gridObjects;

    private Camera _sceneCamera;

    private GameObject _cachedCellFreePreviewObject;
    private GameObject _cachedCellOccupiedPreviewObject;

    private Transform _dynamicTransform;

    private void Awake()
    {
        _sceneCamera = Camera.main;

        _grid = new bool[_gridWidth, _gridHeight];
        _gridObjects = new List<GameObject>();

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Vector2 start = GetCellWorldPosition(x, y);

                Debug.DrawLine(start, (Vector2)GetCellWorldPosition(x + 1, y), Color.blue, 500f);
                Debug.DrawLine(start, (Vector2)GetCellWorldPosition(x, y + 1), Color.blue, 500f);
            }
        }

        Transform previewTransform = GameObject.Find("@Preview").transform;
        _dynamicTransform = GameObject.Find("@Dynamic").transform;
        
        _cachedCellFreePreviewObject = Instantiate(_cellFreePreviewPrefab, Vector2.zero, Quaternion.identity, previewTransform);
        _cachedCellOccupiedPreviewObject = Instantiate(_cellOccupiedPreviewPrefab, Vector2.zero, Quaternion.identity, previewTransform);

        _cachedCellFreePreviewObject.SetActive(false);
        _cachedCellOccupiedPreviewObject.SetActive(false);
    }

    private void Update()
    {
        SpawnDebugObject();

        PreviewCell();
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
                Debug.Log($"Testing -> {x + i}, {y + j}");
                Debug.Log($"Is Occupied -> {IsCellOccupied(x + i, y + j)}");

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
            }
        }
    }
    
    private void SpawnDebugObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);

            (int cellX, int cellY) = GetCellPositionFromWorld(mousePosition);
            
            if (IsInGridBounds(cellX, cellY))
            {
                if (IsCellOccupied(cellX, cellY)) { return; }

                _gridObjects.Add
                    (Instantiate(_debugPrefab, (Vector2)GetCellWorldPositionCentred(cellX, cellY), Quaternion.identity, _dynamicTransform));
                
                SetCellOccupation(cellX, cellY, true);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);

            (int cellX, int cellY) = GetCellPositionFromWorld(mousePosition);

            if (IsInGridBounds(cellX, cellY))
            {
                if (!AreCellsFree(cellX, cellY, 2, 2)) { return; }

                _gridObjects.Add
                    (Instantiate(_debugPrefabBig, (Vector2)GetCellWorldPositionCentred(cellX, cellY), Quaternion.identity, _dynamicTransform));

                SetCellOccupations(cellX, cellY, 2, 2, true);
            }
        }
    }

    private void PreviewCell()
    {
        Vector2 mousePositionWorld = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        (int cellX, int cellY) = GetCellPositionFromWorld(mousePositionWorld);

        if (IsInGridBounds(cellX, cellY))
        {
            if (IsCellOccupied(cellX, cellY))
            {
                _cachedCellFreePreviewObject.SetActive(false);
                _cachedCellOccupiedPreviewObject.SetActive(true);
                _cachedCellOccupiedPreviewObject.transform.position = (Vector2)GetCellWorldPositionCentred(cellX, cellY);
            }
            else if (!IsCellOccupied(cellX, cellY))
            {
                _cachedCellOccupiedPreviewObject.SetActive(false);
                _cachedCellFreePreviewObject.SetActive(true);
                _cachedCellFreePreviewObject.transform.position = (Vector2)GetCellWorldPositionCentred(cellX, cellY);
            }
        }
    }
}
