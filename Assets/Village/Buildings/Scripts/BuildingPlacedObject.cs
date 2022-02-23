using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacedObject : PlacedObject, ObjectUI, BuildingListItemUIData {
    //will expand to allow multiple workers
    Villager assignedVillager;
    JobGameObject buildingJob;
    int numUtensilSlots;
    List<UtensilScriptableObject> compatibleUtensils;
    BuildingPlaceableScriptableObject buildingType;

    List<FoodGameObject> storedFood;
    public Transform pickupLocation; 
    bool isTested = false;
    public static BuildingPlacedObject CreateBuilding(Vector3 worldPosition, Vector2Int origin, PlaceableScriptableObject.Dir dir, BuildingPlaceableScriptableObject buildingType, int cellScale) {
        Transform placedObjectTransform = Instantiate(buildingType.prefab, worldPosition, Quaternion.Euler(0, buildingType.GetRotationAngle(dir), 0));

        BuildingPlacedObject placedObject = placedObjectTransform.GetComponent<BuildingPlacedObject>();

        placedObject.buildingType = buildingType;
        placedObject.numUtensilSlots = buildingType.numUtensilSlots;
        placedObject.compatibleUtensils = buildingType.compatibleUtensils;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.originalScale = buildingType.prefab.localScale;
        placedObject.gridDensity = cellScale;
        placedObject.buildingJob = JobGameObject.CreateJobObject(buildingType.buildingJob, placedObject);
        placedObject.name = "Building: " + origin.ToString();

        if (buildingType.hasCapacity)
            placedObject.storedFood = new List<FoodGameObject>();

        return placedObject;
    }

    public JobGameObject GetJob() { return buildingJob; }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            TestUtensils();
        }
    }

    public bool CanAddFood() { return buildingType.hasCapacity && storedFood.Count < buildingType.maxCapacity; }
    public bool HasStoredFood() { return buildingType.hasCapacity && storedFood.Count > 0; }
    public bool HasFoodWaiting() { return HasStoredFood() && !buildingType.canPreventSpoiling; }
    public void AddFoodObject(FoodGameObject food) {
        storedFood.Add(food);
    }

    public FoodGameObject GetNextFoodObject() {
        FoodGameObject retFood = storedFood[0];
        storedFood.Remove(retFood);
        return retFood;
    }

    private void WorkJob() {
        Debug.Log(gameObject.name + " working");
        buildingJob.DoJob();
    }

    private void TestUtensils() {
        if(!isTested && buildingType.buildingJob.requiresUtensils) {
            Debug.Log("TESTED UTENSILS");
            for(int i = 0; i < numUtensilSlots; i++) {
                Utensil utensil = Utensil.Create(transform, compatibleUtensils[i]);
                utensil.transform.position = transform.position;
                if (i == 0) {
                    Vector3 adjustment = new Vector3(1f, 0, 0);
                    utensil.transform.localPosition += adjustment;
                }
                else {
                    Vector3 adjustment = new Vector3(-1f, 0, 0);
                    utensil.transform.localPosition += adjustment;
                }

                buildingJob.AddActiveUtensil(utensil);
            }
            isTested = true;
        }
    }

    public void AssignVillager(Villager villager) {
        assignedVillager = villager;
        buildingJob.SetWorkingVillager(villager);
        assignedVillager.UnassignVillagerEvent += HandleUnassignedVillager;
        //need to wait until villager is at the building to start this.
        InvokeRepeating("WorkJob", 0, buildingJob.jobType.progressRate);
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
        return new ObjectUIData(buildingJob.jobType.jobName, buildingJob.jobType.description, true, buildingJob.GetProgressAmount());            
    }                           
    
    public ListItemData GetListItemData() {
        return new ListItemData(buildingJob.jobType.jobName);
    }
}
