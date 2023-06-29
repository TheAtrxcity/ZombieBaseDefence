using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        transform.position += (Vector3)velocity * _moveSpeed * Time.deltaTime;
    }
}
    