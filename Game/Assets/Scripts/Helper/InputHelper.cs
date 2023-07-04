using UnityEngine;

public class InputHelper : MonoBehaviour
{
    public static InputHelper Instance => _instance;
    private static InputHelper _instance;

    private Camera _sceneCamera;

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _sceneCamera = Camera.main;
    }

    public Vector2 GetMouseWorldPosition() => _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
}
