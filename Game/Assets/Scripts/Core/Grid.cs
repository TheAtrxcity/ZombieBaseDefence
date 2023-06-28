using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Grid
{
    private int _width;
    private int _height;
    private int _cellSize;

    private int[,] _grid;

    private Vector2Int _gridOriginPosition;

    //private List<GameObject> _debugWalls;
    private GameObject[,] _debugWalls;

    public Grid(int width, int height, int cellSize, Vector2Int origin)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridOriginPosition = origin;

        _grid = new int[_width, _height];

        _debugWalls = new GameObject[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Debug.DrawLine((Vector2)GetWorldPosition(x, y), (Vector2)GetWorldPosition(x + 1, y), Color.red, 100f);
                Debug.DrawLine((Vector2)GetWorldPosition(x, y), (Vector2)GetWorldPosition(x, y + 1), Color.red, 100f);
            }

            Debug.DrawLine((Vector2)GetWorldPosition(0, _height), (Vector2)GetWorldPosition(_width, _height), Color.red, 100f);
            Debug.DrawLine((Vector2)GetWorldPosition(_width, 0), (Vector2)GetWorldPosition(_width, _height), Color.red, 100f);
        }
    }

    private Vector2Int GetWorldPosition(int x, int y) => new Vector2Int(x, y) * _cellSize + _gridOriginPosition;

    private Vector2Int GetCell(Vector2 worldPosition)
    {
        Vector2Int vec = new()
        {
            x = Mathf.FloorToInt((worldPosition - _gridOriginPosition).x / _cellSize),
            y = Mathf.FloorToInt((worldPosition - _gridOriginPosition).y / _cellSize)
        };

        return vec;
    }

    public void SetCellValue(Vector2 position, int value)
    {
        Vector2Int cell = GetCell(position);

        if (InGridBounds(cell))
        {
            _grid[cell.x, cell.y] = value;
        }
    }

    public int GetCellValue(Vector2 position)
    {
        Vector2Int cell = GetCell(position);

        if (InGridBounds(cell))
        {
            return _grid[cell.x, cell.y];
        }

        return -1;
    }

    public void AddDebugWall(Vector2 position)
    {
        Vector2Int cell = GetCell(position);

        if (InGridBounds(cell))
        {
            GameObject wall = new("Wall");
            wall.transform.position = (Vector2)GetWorldPosition(cell.x, cell.y) + new Vector2(_cellSize, _cellSize) * 0.5f;

            Texture2D texture = new(32, 32);
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    texture.SetPixel(i, j, Color.white);
                }
            }

            Rect rect = new(Vector2.zero, new Vector2(32, 32));

            wall.AddComponent<SpriteRenderer>();
            wall.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, Vector2.zero);
            wall.transform.localScale = new Vector2(5, 5);

            SetCellValue(position, 1);

            _debugWalls[cell.x, cell.y] = wall;
        }
    }

    public bool AddWall(GameObject wallObject, Vector2 position)
    {
        Vector2Int cell = GetCell(position);
        
        if (InGridBounds(cell) && GetCellValue(position) < 1)
        {
            Vector2 centredPosition = (Vector2)GetWorldPosition(cell.x, cell.y) +
                new Vector2(_cellSize, _cellSize) * 0.5f;

            wallObject.transform.position = centredPosition;
            wallObject.transform.localScale = new Vector2(5, 5);

            SetCellValue(position, 1);
            _debugWalls[cell.x, cell.y] = wallObject;

            return true;
        }

        return false;
    }

    public GameObject RemoveWall(Vector2 position)
    {
        Vector2Int cell = GetCell(position);

        if (InGridBounds(cell) && GetCellValue(position) > 0)
        {
            SetCellValue(position, 0);
            return _debugWalls[cell.x, cell.y];
        }

        return null;
    }

    private bool InGridBounds(Vector2Int cellVector)
    {
        if (cellVector.x >= 0 && cellVector.y >= 0 && cellVector.x < _width && cellVector.y < _height)
        {
            return true;
        }

        return false;
    }
}
