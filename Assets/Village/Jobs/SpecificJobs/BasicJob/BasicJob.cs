using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJob : JobGameObject
{
    // called every "jobProgressRate" seconds
    public override void DoJob() { 
        if(jobType.requiresUtensils) {
            if(activeUtensils.Count > 0) {
                //Debug.Log("Doing basic job: " + jobProgress);
                jobProgress += jobType.progressIncrement;
                foreach(Utensil utensil in activeUtensils) {
                    utensil.IncreaseDirtAmount(5f);
                }
                workingVillager.DecreaseEnergy(jobType.energyCost);
                if (jobProgress >= 100) {
                    Debug.Log("Jobs Done!");
                    jobProgress = 0;
                }
            }
            else {
                Debug.Log("Needs utensils");
            }
        }
    }
}
