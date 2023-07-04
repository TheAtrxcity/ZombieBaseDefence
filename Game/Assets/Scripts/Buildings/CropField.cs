using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Sprite[] _cropFieldSprites;

    [Header("Settings")]
    [SerializeField] private float _cropGrowInterval;

    private int _currentSpriteIndex = 0;

    private float _cropGrowTimer;

    private void Update() => HandleCropGrowing();

    private void HandleCropGrowing()
    {
        _cropGrowTimer += Time.deltaTime;

        if (_cropGrowTimer >= _cropGrowInterval && _currentSpriteIndex < _cropFieldSprites.Length - 1)
        {
            _currentSpriteIndex++;
            GetComponent<SpriteRenderer>().sprite = _cropFieldSprites[_currentSpriteIndex];

            _cropGrowTimer = 0.0f;
        }
    }
}
