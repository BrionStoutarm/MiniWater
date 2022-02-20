using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacedObject : PlacedObject, ObjectUI, BuildingListItemUIData {
    //will expand to allow multiple workers
    Villager assignedVillager;
    Job buildingJob;
    int numUtensilSlots;
    List<UtensilScriptableObject> compatibleUtensils;

    protected List<Utensil> activeUtensils;
    bool isTested = false;
    public static BuildingPlacedObject CreateBuilding(Vector3 worldPosition, Vector2Int origin, PlaceableScriptableObject.Dir dir, BuildingPlaceableScriptableObject placeableType, int cellScale) {
        Transform placedObjectTransform = Instantiate(placeableType.prefab, worldPosition, Quaternion.Euler(0, placeableType.GetRotationAngle(dir), 0));

        BuildingPlacedObject placedObject = placedObjectTransform.GetComponent<BuildingPlacedObject>();

        placedObject.placeableType = placeableType;
        placedObject.numUtensilSlots = placeableType.numUtensilSlots;
        placedObject.compatibleUtensils = placeableType.compatibleUtensils;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.originalScale = placeableType.prefab.localScale;
        placedObject.gridDensity = cellScale;
        placedObject.buildingJob = Instantiate(placeableType.buildingJob, placedObject.transform);
        placedObject.name = "Building: " + origin.ToString();

        placedObject.activeUtensils = new List<Utensil>();

        return placedObject;
    }

    public Job GetJob() { return buildingJob; }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            TestUtensils();
        }
    }

    private void WorkJob() {
        Debug.Log(gameObject.name + " working: ");
        buildingJob.DoJob();
        if(activeUtensils.Count == 0) {
            Debug.Log("No utensils!");
        }
        foreach(Utensil utensil in activeUtensils) {
            utensil.IncreaseDirtAmount(5f); // HARDCODED will extract to scriptableobj for Utensils
        }
    }

    public bool canAddUtensil(UtensilScriptableObject utensil) { return (activeUtensils.Count < numUtensilSlots) && compatibleUtensils.Contains(utensil); }
    public void addUtensil(Utensil utensil) {
        if (canAddUtensil(utensil.utensilType)) {
            activeUtensils.Add(utensil);
        }
        else
            Debug.Log("Cannot add utensil compatibleUtensils.count = " + compatibleUtensils.Count);
    }

    private void TestUtensils() {
        if(!isTested) {
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

                addUtensil(utensil);
            }
            isTested = true;
        }
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
        return new ObjectUIData(buildingJob.jobName, buildingJob.description, true, buildingJob.jobProgressAmount);            
    }                           
    
    public ListItemData GetListItemData() {
        return new ListItemData(buildingJob.jobName);
    }
}
