using UnityEngine;

public class WorldSetup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite[] _grassSprites;
    [SerializeField] private Grid _worldGrid;

    private void Start()
    {
        Transform _worldObjectsTransform = GameObject.Find("@WorldObjects").transform;

        for (int x = 0; x < _worldGrid.Width; x++)
        {
            for (int y = 0; y < _worldGrid.Height; y++)
            {
                Vector2 spawnPosition = new Vector2(x * _worldGrid.CellSize, y * _worldGrid.CellSize) + _worldGrid.GridOrigin;

                GameObject randomGrass = new($"Grass {x}, {y}", typeof(SpriteRenderer));

                randomGrass.GetComponent<SpriteRenderer>().sprite = _grassSprites[Random.Range(0, _grassSprites.Length)];
                randomGrass.transform.parent = _worldObjectsTransform;
                randomGrass.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 1) + new Vector3(_worldGrid.CellSize, _worldGrid.CellSize) * 0.5f;
            }
        }
    }
}
