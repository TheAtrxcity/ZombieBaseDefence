using UnityEngine;

using static BuildingData;

public class BuildingSystem : MonoBehaviour
{
	[Header("Building Sprites")]
	[SerializeField] private Sprite _tentSprite;
	[SerializeField] private Sprite _rainCatcherSprite;
	[SerializeField] private Sprite _cropFieldSprite;

	[Header("Building Prefabs")]
	[SerializeField] private GameObject _rainCatcherPrefab;
	[SerializeField] private GameObject _cropFieldPrefab;

	[Header("Dependencies")]
	[SerializeField] private Grid _worldGrid;

	private Building _selectedBuilding;

	private GameObject _buildingPreviewObject;

    private void Start()
	{
        _selectedBuilding = Building.None;

		_buildingPreviewObject = new GameObject("BuildingPreview", typeof(SpriteRenderer));
		_buildingPreviewObject.transform.parent = GameObject.Find("@Preview").transform;
    }

    private void Update()
    {
		ChangeSelectedBuilding();
	
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 worldMousePosition = InputHelper.Instance.GetMouseWorldPosition();

			switch (_selectedBuilding)
			{
				case Building.Tent:
					if (_worldGrid.AddBuilding(worldMousePosition, Tent.width, Tent.height))
					{
						MakeBuilding("Tent", _worldGrid.GetPlacementPosition(worldMousePosition), _tentSprite);
                    }
                    break;

                case Building.RainCatcher:
					if (_worldGrid.AddBuilding(worldMousePosition, RainCatcher.width, RainCatcher.height))
					{
						var rc = Instantiate(_rainCatcherPrefab, _worldGrid.GetPlacementPosition(worldMousePosition), Quaternion.identity);

						rc.name = "Rain Catcher";
						rc.transform.parent = GameObject.Find("@Buildings").transform;
                    }
                    break;

                case Building.CropField:
                    if (_worldGrid.AddBuilding(worldMousePosition, BuildingData.CropField.width, BuildingData.CropField.height))
                    {
                        var cf = Instantiate(_cropFieldPrefab, _worldGrid.GetPlacementPosition(worldMousePosition), Quaternion.identity);

                        cf.name = "Crop Field";
                        cf.transform.parent = GameObject.Find("@Buildings").transform;
                    }
                    break;
            }
		}

		_buildingPreviewObject.transform.position = _worldGrid.GetPlacementPosition(InputHelper.Instance.GetMouseWorldPosition());

		if (!_worldGrid.CanBuildHere(InputHelper.Instance.GetMouseWorldPosition(), 3, 3))
		{
			_buildingPreviewObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
		{
            _buildingPreviewObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void ChangeSelectedBuilding()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1)) 
		{ 
			_selectedBuilding = Building.None;

			_buildingPreviewObject.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			_selectedBuilding = Building.Tent;

			_buildingPreviewObject.SetActive(true);
			_buildingPreviewObject.GetComponent<SpriteRenderer>().sprite = _tentSprite;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			_selectedBuilding = Building.RainCatcher;

            _buildingPreviewObject.SetActive(true);
            _buildingPreviewObject.GetComponent<SpriteRenderer>().sprite = _rainCatcherSprite;
        }
		
		if (Input.GetKeyDown(KeyCode.Alpha4)) 
		{
			_selectedBuilding = Building.CropField;

            _buildingPreviewObject.SetActive(true);
            _buildingPreviewObject.GetComponent<SpriteRenderer>().sprite = _cropFieldSprite;
        }
    }

	private void MakeBuilding(string name, Vector2 position, Sprite sprite)
	{
		GameObject building = new(name, typeof(SpriteRenderer));

        building.GetComponent<SpriteRenderer>().sprite = sprite;
		building.transform.position = position;
        building.transform.parent = GameObject.Find("@Buildings").transform;
    }
}

public static class BuildingData
{
	public struct Tent
	{
		public static int width  = 4;
		public static int height = 4;
	}

	public struct RainCatcher
	{
        public static int width  = 3;
        public static int height = 2;
    }

	public struct CropField
	{
        public static int width  = 3;
        public static int height = 3;
    }
}

public enum Building
{
	None,
	Tent,
	RainCatcher,
	CropField
};