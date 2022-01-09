using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSelectController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    public class SelectedObjectEventArgs : EventArgs {
        public PlacedObject placedObject;
    };
    public static EventHandler<SelectedObjectEventArgs> OnSelectedObjectChanged;

    public static PlacedObject SelectedObject { get; private set; }

    string text;
    private void Awake() {
        PlayerInput.OnLeftClickEvent += HandleLeftClick;
        PlayerInput.OnRightClickEvent += HandleRightClick;
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
            PlacedObject placedObject = GridBuildingSystem.Instance.GetPlacedObjectAtWorldPosition(hit.point);
            if (placedObject != null) {

                //fill out selected object window
                //text = placedObject.GetObjectName();
                //m_selectedObjectArea.UpdateUI(placedObject);
                //SelectedObject = placedObject;

                //if its a building, enable assign villager button
                //if(placedObject is BuildingPlacedObject) {
                //    BuildingPlacedObject buildingObj = placedObject as BuildingPlacedObject;

                //}
                if(OnSelectedObjectChanged != null) { OnSelectedObjectChanged(this, new SelectedObjectEventArgs { placedObject = placedObject }); }
            }
            else {
                Debug.Log("Didnt hit placedObject");
            }
        }
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
