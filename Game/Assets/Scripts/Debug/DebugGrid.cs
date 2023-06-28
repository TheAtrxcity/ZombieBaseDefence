using UnityEngine;

public class DebugGrid : MonoBehaviour
{
    private Camera _camera;

    private Grid _gridA;
    private Grid _gridB;

    [SerializeField] private GameObject _debugWall;

    private void Start()
    {
        _camera = Camera.main;

        _gridA = new Grid(5, 5, 5, new Vector2Int(-10, -10));
        _gridB = new Grid(2, 5, 5, new Vector2Int(-30, -10));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            GameObject newWall = Instantiate(_debugWall, Vector2.zero, Quaternion.identity);

            if (!_gridA.AddWall(newWall, mouseWorldPosition) &&
                !_gridB.AddWall(newWall, mouseWorldPosition))
            {
                Destroy(newWall);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Destroy(_gridA.RemoveWall(mouseWorldPosition));
            Destroy(_gridB.RemoveWall(mouseWorldPosition));
        }
    }
}
