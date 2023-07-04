using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
    [SerializeField] private float _resourceInterval;
    [SerializeField] private int _resourceGain;

    private int _resources;

    private float _resourceTimer;

    private void Update() => HandleResourceGain();

    private void HandleResourceGain()
    {
        _resourceTimer += Time.deltaTime;

        if (_resourceTimer >= _resourceInterval)
        {
            _resources += _resourceGain;
            _resourceTimer = 0.0f;
        }
    }
}
