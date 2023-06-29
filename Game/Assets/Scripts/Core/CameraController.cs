using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed;

    private Camera _sceneCamera;

    private void Awake()
    {
        _sceneCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += (Vector3)velocity * _moveSpeed * Time.deltaTime;

        _sceneCamera.orthographicSize -= Input.mouseScrollDelta.y;
        _sceneCamera.orthographicSize = Mathf.Clamp(_sceneCamera.orthographicSize, 10f, 20f);
    }
}
    