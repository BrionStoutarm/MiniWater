using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ObjectUIData {
    public string objectName, objectDescription;
    public bool hasProgress;
    public float progressAmount;

    public ObjectUIData(string name, string description, bool hasProgress, float progressAmount) { objectName = name; objectDescription = description; this.hasProgress = hasProgress; this.progressAmount = progressAmount; }
}
public interface ObjectUI {  
    ObjectUIData GetUIData();
}
