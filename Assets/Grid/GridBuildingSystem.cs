using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


public class GridBuildingSystem : MonoBehaviour {
    public int gridWidth, gridHeight;
    public float gridScale;
    public int gridDensity;
    public Collider deckCollider;

    public Material gridMat;

    [SerializeField] private List<BuildingPlaceableScriptableObject> buildingTypeList;
    private BuildingPlaceableScriptableObject currentPlaceBuilding;

    private Grid<GridObject> grid;
    private PlaceableScriptableObject.Dir dir = PlaceableScriptableObject.Dir.Down;

    private static GridBuildingSystem s_instance;

    public event EventHandler<OnSelectedBuildingChangedArgs> OnSelectedBuildingChanged;
    public class OnSelectedBuildingChangedArgs : EventArgs { }

    public event EventHandler<OnBuildingSysActiveArgs> OnBuildingSysActive;
    public class OnBuildingSysActiveArgs : EventArgs {
        public bool active;
    }

    //temporary i think, for villager testing purposes
    public event EventHandler<OnPlacedBuildingArgs> OnPlacedBuilding;
    public class OnPlacedBuildingArgs : EventArgs {
        public BuildingPlacedObject placedObject; //should subclass this to be a building, as there will be more placeable objects
        public Vector2Int gridPosition;
    }

    private bool m_isActive = true;

    public static GridBuildingSystem Instance {
        get => s_instance;
        set {
            if (value != null && s_instance == null)
                s_instance = value;
        }
    }

    public bool HasCurrentBuildingSelected() {
        return currentPlaceBuilding != null;
    }

    private void OnEnable() {
        if (OnBuildingSysActive != null) { OnBuildingSysActive(this, new OnBuildingSysActiveArgs { active = true }); }
    }

    private void OnDisable() {
        if (OnBuildingSysActive != null) { OnBuildingSysActive(this, new OnBuildingSysActiveArgs { active = false }); }
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        if (enabled) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                Vector3 hitPoint = hit.point;
                grid.GetXZ(hitPoint, out int x, out int z);

                Vector2Int rotationOffset = currentPlaceBuilding.GetRotationOffset(dir, 1);
                Vector3 placeObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridScale;

                return placeObjectWorldPosition;
            }
        }
        return Vector3.zero;
    }

    public void toggleActive() {
        enabled = true;
    }

    public bool isActive() {
        return enabled;
    }

    public PlaceableScriptableObject GetPlacedObjectType() {
        return currentPlaceBuilding;
    }

    public PlacedObject GetPlacedObjectAtWorldPosition(Vector3 worldPosition) {
        GridObject gridObject = grid.GetGridObject(worldPosition);
        PlacedObject placedObject = gridObject.GetPlacedObject();
        return placedObject;
    }

    public PlacedObject GetObjectAtGridPosition(int x, int z) {
        GridObject gridObject = grid.GetGridObject(x, z);
        PlacedObject placedObject = gridObject.GetPlacedObject();
        return placedObject;
    }

    public Quaternion GetPlacedObjectRotation() {
        return Quaternion.Euler(0, currentPlaceBuilding.GetRotationAngle(dir), 0);
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Multiple Building Systems");
            return;
        }
        Instance = this;

        currentPlaceBuilding = buildingTypeList[0];


        //Register Events to listen to
        PlayerInput.OnLeftClickEvent += Instance_OnLeftClickEvent;
        PlayerInput.OnRightClickEvent += Instance_OnRightClickEvent;
        enabled = m_isActive;

        CreateDeckGrid();
        enabled = false;
    }

    private void Instance_OnLeftClickEvent(object sender, PlayerInput.OnLeftClickArgs e) {
        Instance.HandleLeftClick(Input.mousePosition);
    }

    private void HandleLeftClick(Vector3 mousePosition) {
        if (enabled) {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit)) {
                //Debug.Log(hit.point);
                Vector3 hitPoint = hit.point;
                //hitPoint.y = 0f;

                grid.GetXZ(hitPoint, out int x, out int z);

                List<Vector2Int> gridPositionList = currentPlaceBuilding.GetGridPositionList(new Vector2Int(x, z), dir, gridDensity);


                //Test can build 
                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList) {
                    if (gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < grid.Width() && gridPosition.y < grid.Height()) {
                        if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                            //cannot build here
                            canBuild = false;
                            break;
                        }
                    }
                    else {
                        canBuild = false;
                    }
                }

                GridObject gridObject = grid.GetGridObject(x, z);
                if (canBuild) {
                    Vector2Int buildingRotationOffset = currentPlaceBuilding.GetRotationOffset(dir, gridDensity);
                    Vector3 placeObjectWorldPosition = GetMouseWorldSnappedPosition();
                    placeObjectWorldPosition.y = hitPoint.y;


                    BuildingPlacedObject placedObject = BuildingPlacedObject.CreateBuilding(placeObjectWorldPosition, new Vector2Int(x, z), dir, currentPlaceBuilding, gridDensity);
                    foreach (Vector2Int gridPosition in gridPositionList) {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }


                    if (OnPlacedBuilding != null) { OnPlacedBuilding(this, new OnPlacedBuildingArgs { placedObject = placedObject, gridPosition = new Vector2Int(x, z) }); }
                }
                else {
                    //StaticFunctions.CreateWorldTextPopup("Cannot build here!", hitPoint);
                    Debug.Log("Cannot build here");
                }
            }
        }
    }

    private void CreateDeckGrid() {
        Collider deckCollider = GameObject.Find("Deck").GetComponent<MeshCollider>();

        grid = new Grid<GridObject>(gridWidth, gridHeight, gridScale, gridDensity, deckCollider.bounds.min, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), GameManager.Instance.OnDebug());
    }

    private void Instance_OnRightClickEvent(object sender, PlayerInput.OnRightClickArgs e) {
        Instance.HandleRightClick(Input.mousePosition);
    }
    private void HandleRightClick(Vector3 mousePosition) {
        currentPlaceBuilding = null;
        enabled = false;
    }

    private void Update() {
        //Rotate current placing building
        if(enabled) {
            if (Input.GetKeyDown(KeyCode.R)) {
                dir = PlaceableScriptableObject.GetNextDir(dir);
                Debug.Log(dir);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            enabled = true;
            currentPlaceBuilding = buildingTypeList[0]; 
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            enabled = true;
            currentPlaceBuilding = buildingTypeList[1];
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            enabled = true;
            currentPlaceBuilding = buildingTypeList[2];
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            enabled = true;
            currentPlaceBuilding = buildingTypeList[3];
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
    }

}
public class GridObject {
    private Grid<GridObject> grid;
    private int x;
    private int z;
    private PlacedObject placedObject;

    public GridObject(Grid<GridObject> grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public PlacedObject GetPlacedObject() {
        return placedObject;
    }

    public void SetPlacedObject(PlacedObject placedObject) {
        this.placedObject = placedObject;
        grid.TriggerGridObjectChanged(x, z);
    }

    public void ClearPlacedObject() {
        placedObject = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public bool CanBuild() {
        return placedObject == null;
    }

    public override string ToString() {
        return x + "," + z + "\n" + placedObject;
    }

    public void SetVisible(bool isVisible) {
        if(placedObject != null) {
            placedObject.SetVisible(isVisible);
        }
    }
}
