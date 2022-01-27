using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacedObject : PlacedObject
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
        placedObject.buildingJob = placeableType.buildingJob;
        return placedObject;
    }

    private void Update() {
        if(assignedVillager != null) {
            buildingJob.DoJob();
        }
    }

    public void AssignVillager(Villager villager) {
        assignedVillager = null;
        assignedVillager = villager;
    }

    public BuildingPlaceableScriptableObject GetBuildingType() {
        return placeableType as BuildingPlaceableScriptableObject;
    }
}
