using UnityEngine;

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

	private BuildingData _tentData;
	private BuildingData _rainCatcherData;
	private BuildingData _cropFieldData;
	private BuildingData _selectedBuildingData;

	private GameObject _buildingPreviewObject;

    private void Start()
	{
		_tentData = new BuildingData(4, 4);
		_rainCatcherData = new BuildingData(3, 2);
		_cropFieldData = new BuildingData(3, 4);

        _selectedBuilding = Building.None;

		_buildingPreviewObject = new GameObject("BuildingPreview", typeof(SpriteRenderer));
		_buildingPreviewObject.transform.position += new Vector3(0, 0, -5);
		_buildingPreviewObject.transform.parent = GameObject.Find("@Preview").transform;

		Cursor.visible = false;
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
					if (_worldGrid.AddBuilding(worldMousePosition, _selectedBuildingData.width, _selectedBuildingData.height))
					{
						MakeBuilding("Tent", _worldGrid.GetPlacementPosition(worldMousePosition), _tentSprite);
                    }
                    break;

                case Building.RainCatcher:
					if (_worldGrid.AddBuilding(worldMousePosition, _selectedBuildingData.width, _selectedBuildingData.height))
					{
						var rc = Instantiate(_rainCatcherPrefab, _worldGrid.GetPlacementPosition(worldMousePosition), Quaternion.identity);

						rc.name = "Rain Catcher";
						rc.transform.parent = GameObject.Find("@Buildings").transform;
                    }
                    break;

                case Building.CropField:
                    if (_worldGrid.AddBuilding(worldMousePosition, _selectedBuildingData.width, _selectedBuildingData.height))
                    {
                        var cf = Instantiate(_cropFieldPrefab, _worldGrid.GetPlacementPosition(worldMousePosition), Quaternion.identity);

                        cf.name = "Crop Field";
                        cf.transform.parent = GameObject.Find("@Buildings").transform;
                    }
                    break;
            }
		}

		_buildingPreviewObject.transform.position = _worldGrid.GetPlacementPosition(InputHelper.Instance.GetMouseWorldPosition());

		if (!_worldGrid.CanBuildHere(InputHelper.Instance.GetMouseWorldPosition(), _selectedBuildingData.width, _selectedBuildingData.height))
		{
			_buildingPreviewObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.75f);
        }
        else
		{
			_buildingPreviewObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
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
			_selectedBuildingData = _tentData;

			_buildingPreviewObject.SetActive(true);
			_buildingPreviewObject.GetComponent<SpriteRenderer>().sprite = _tentSprite;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			_selectedBuilding = Building.RainCatcher;
			_selectedBuildingData = _rainCatcherData;

            _buildingPreviewObject.SetActive(true);
            _buildingPreviewObject.GetComponent<SpriteRenderer>().sprite = _rainCatcherSprite;
        }
		
		if (Input.GetKeyDown(KeyCode.Alpha4)) 
		{
			_selectedBuilding = Building.CropField;
			_selectedBuildingData = _cropFieldData;

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

public enum Building
{
	None,
	Tent,
	RainCatcher,
	CropField
};

public struct BuildingData
{
	public int width;
	public int height;

	public BuildingData(int width, int height)
	{
		this.width = width;
		this.height = height;
	}
}