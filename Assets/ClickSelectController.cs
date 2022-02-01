using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSelectController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    private static ClickSelectController s_instance;
    public static ClickSelectController Instance {
        get => s_instance;
        set {
            if (value != null && s_instance == null) {
                s_instance = value;
            }
        }
    }

    public class SelectedBuildingArgs : EventArgs {
        public BuildingPlacedObject selectedBuilding;
    };
    public event EventHandler<SelectedBuildingArgs> SelectedBuildingChanged;
    public class SelectedVillagerArgs : EventArgs {
        public Villager selectedVillager;
    };
    public event EventHandler<SelectedVillagerArgs> SelectedVillagerChanged;

    string text;
    private void Awake() {
        PlayerInput.OnLeftClickEvent += HandleLeftClick;
        PlayerInput.OnRightClickEvent += HandleRightClick;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleUILoadEvent(object sender, UIController.UILoadEventArgs args) {

    }

    private void HandleLeftClick(object sender, PlayerInput.OnLeftClickArgs args) {
        if (!GridBuildingSystem.Instance.isActive()) {
            RaycastHit hit = StaticFunctions.GetMouseRaycastHit();
            if(hit.collider != null) {
                GameObject hitObject = hit.collider.gameObject;
                if (StaticFunctions.GetVillagerFromGameObject(hitObject) != null) {
                    Villager selectedVillager = StaticFunctions.GetVillagerFromGameObject(hitObject);
                    if (SelectedVillagerChanged != null) { SelectedVillagerChanged(this, new SelectedVillagerArgs { selectedVillager = selectedVillager }); }
                }
                else if(StaticFunctions.GetBuildingFromGameObject(hitObject) != null) {
                    BuildingPlacedObject selectedBuilding = StaticFunctions.GetBuildingFromGameObject(hitObject);
                    if (SelectedBuildingChanged != null) { SelectedBuildingChanged(this, new SelectedBuildingArgs { selectedBuilding = selectedBuilding }); }
                }
                else {
                    if (SelectedVillagerChanged != null) { SelectedVillagerChanged(this, new SelectedVillagerArgs { selectedVillager = null }); }
                    if (SelectedBuildingChanged != null) { SelectedBuildingChanged(this, new SelectedBuildingArgs { selectedBuilding = null }); }
                    Debug.Log("Didnt hit anything");
                }
            }
        }
        //else let GridBuildingSystem handle it
    }

    private void HandleRightClick(object sender, PlayerInput.OnRightClickArgs args) {
        if (!GridBuildingSystem.Instance.isActive()) {
          
        //    if (SelectedObject != null) {
        //        m_selectedObjectArea.Clear();
        //        SelectedObject = null;
        //    }
        //    else {
        //        Debug.Log("Didnt hit placedObject");
        //    }
        }
    }
}
