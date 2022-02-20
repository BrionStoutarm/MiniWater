using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UtensilScriptableObject", menuName = "ScriptableObjects/UtensilType")]
public class UtensilScriptableObject : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Transform visual;
}
