using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacedObject : PlacedObject, ObjectUI
{
    //will expand to allow multiple workers
    Villager assignedVillager;
    Job buildingJob;

    public static BuildingPlacedObject CreateBuilding(Vector3 worldPosition, Vector2Int origin, PlaceableScriptableObject.Dir dir, BuildingPlaceableScriptableObject placeableType, int cellScale) {
        Transform placedObjectTransform = Instantiate(placeableType.prefab, worldPosition, Quaternion.Euler(0, placeableType.GetRotationAngle(dir), 0));

        BuildingPlacedObject placedObject = placedObjectTransform.GetComponent<BuildingPlacedObject>();

        placedObject.placeableType = placeableType;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.originalScale = placeableType.prefab.localScale;
        placedObject.gridDensity = cellScale;
        placedObject.buildingJob = Instantiate(placeableType.buildingJob, placedObject.transform);
        placedObject.name = "Building: " + origin.ToString();
        return placedObject;
    }

    public Job GetJob() { return buildingJob; }

    private void Update() {

    }

    private void WorkJob() {
        Debug.Log(gameObject.name + " working: ");
        buildingJob.DoJob();
    }

    public void AssignVillager(Villager villager) {
        assignedVillager = villager;
        buildingJob.SetWorkingVillager(villager);
        assignedVillager.UnassignVillagerEvent += HandleUnassignedVillager;
        //need to wait until villager is at the building to start this.
        InvokeRepeating("WorkJob", 0, buildingJob.progressRate);
    }

    public void HandleUnassignedVillager(object sender, Villager.UnassignVillagerEventArgs args) {
        if(args.workedBuilding == this) {
            //will change to check if workers are still working at this
            Debug.Log("Worked stopped at: " + buildingJob.GetJobAmount());
            assignedVillager.UnassignVillagerEvent -= HandleUnassignedVillager;
            CancelInvoke("WorkJob");
        }
    }

    public BuildingPlaceableScriptableObject GetBuildingType() {
        return placeableType as BuildingPlaceableScriptableObject;
    }

    public ObjectUIData GetUIData() {
        return new ObjectUIData(name, buildingJob.description, true, buildingJob.jobProgressAmount);            
    }                            
}
