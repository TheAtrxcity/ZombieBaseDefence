using UnityEngine;

public class PlacementHandlerr : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private GameObject _debugPlacementObject;
 
    private Camera _camera;

    private GameObject _objectToPlace;

    private bool _holdingPlacementButton;

    private void Awake() => _camera = Camera.main;

    private void Update()
    {
        HandlePlacement();
    }

    private Vector2Int GetMousePosition()
    {
        var worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
    }

    private void HandlePlacement()
    {
        if (Input.GetMouseButton(0)) { _holdingPlacementButton = true; }

        if (_holdingPlacementButton)
        {
            if (_objectToPlace == null)
            {
                _objectToPlace = Instantiate(_debugPlacementObject, (Vector2)GetMousePosition(), Quaternion.identity);
            }

            _objectToPlace.transform.position = (Vector2)GetMousePosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _holdingPlacementButton = false;
            _objectToPlace = null;
        }
    }
}
