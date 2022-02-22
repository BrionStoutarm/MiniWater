using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmJob : JobGameObject
{
    public FoodScriptableObject foodProduced;

    public override void DoJob() {
        Debug.Log("Farm working: " + jobProgress);
        jobProgress += jobType.progressIncrement;
        if(jobProgress >= 100f) {
            Debug.Log("Produced: " + foodProduced.foodName);
            FoodGameObject.CreateFoodObject(building.transform.position, foodProduced);
            jobProgress = 0f;
        }
    }
}
