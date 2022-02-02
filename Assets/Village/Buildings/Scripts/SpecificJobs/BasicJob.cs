using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJob : Job
{
    // called every "jobProgressRate" seconds
    public override void DoJob() {
        //Debug.Log("Doing basic job: " + jobProgress);
        jobProgress += jobProgressAmount;
        workingVillager.DecreaseEnergy(energyCost);
        if (jobProgress >= 100) {
            Debug.Log("Jobs Done!");
            jobProgress = 0;
        }
    }
}
