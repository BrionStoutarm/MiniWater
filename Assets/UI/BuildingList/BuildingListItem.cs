using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ListItemData {
    public string name;

    public ListItemData(string name) { this.name = name; }
}
public interface BuildingListItemUIData {
    ListItemData GetListItemData();
}

public class BuildingListItem : MonoBehaviour {
    public Text nameText;

    private void Awake() {
        
    }

    public void SetData(ListItemData itemData) {
        nameText.text = itemData.name;
    }
}