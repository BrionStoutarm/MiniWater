using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobType", menuName = "ScriptableObjects/JobType")]
public class JobScriptableObject : ScriptableObject
{
    public string jobName;
    public string description;
    public float progressRate;
    public float progressIncrement;
    public float energyCost;
    public bool requiresUtensils;

    public Transform prefab;
}
