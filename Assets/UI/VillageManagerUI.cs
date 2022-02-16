using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VillageManagerUI : MonoBehaviour
{
    VillageManager vm;

    public BuildingListItem listItemPrefab;
    public ScrollRect buildingListScrollRect;
    // Start is called before the first frame update
    void Start()
    {
        vm = VillageManager.Instance;
        GridBuildingSystem.Instance.OnPlacedBuilding += HandlePlacedBuilding;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandlePlacedBuilding(object sender, GridBuildingSystem.OnPlacedBuildingArgs args) {
        BuildingListItem item = Instantiate(listItemPrefab, buildingListScrollRect.content);
        item.SetData(args.placedObject.GetListItemData());
    }
}
