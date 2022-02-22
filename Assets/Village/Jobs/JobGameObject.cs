using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JobGameObject : MonoBehaviour
{
    public JobScriptableObject jobType;

    protected BuildingPlacedObject building;
    protected List<Utensil> activeUtensils;
    protected Villager workingVillager;
    protected float jobProgress = 0;

    public static JobGameObject CreateJobObject(JobScriptableObject jobType, BuildingPlacedObject building) {
        Transform placedObjectTransform = Instantiate(jobType.prefab, building.transform);

        JobGameObject placedObject = placedObjectTransform.GetComponent<JobGameObject>();
        placedObject.transform.parent = building.transform;
        placedObject.jobType = jobType;
        placedObject.building = building;
        placedObject.activeUtensils = new List<Utensil>();

        return placedObject;
    }

    public void SetJobBuilding(BuildingPlacedObject building) { this.building = building; }
    public void AddActiveUtensil(Utensil utensil) { activeUtensils.Add(utensil); }
    public void SetWorkingVillager(Villager villager) { this.workingVillager = villager; }

    public float GetProgressAmount() { return jobProgress; }
    // called every "jobProgressRate" seconds
    public abstract void DoJob();

    public float GetJobAmount() { return jobProgress; }
}
