using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, ObjectUI {
    private NavMeshAgent navMeshAgent;
    public Vector3 goal;
    public Transform homeLocation;
    public float energyLevel = 100;

    // Start is called before the first frame update
    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //uiInstance = Instantiate(villagerUIPrefab);
        //uiInstance.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (energyLevel == 0) {
            Debug.Log(gameObject.name + " is out of energy!");
        }
    }

    public void DecreaseEnergy(float amount) {
        if (energyLevel - amount <= 0)
            energyLevel = 0;
        else
            energyLevel -= amount;
    }

    public void OnUnselect() {
        //uiInstance.enabled = false;
    }

    // Assign will have a task as well
    public void Assign(Vector3 goal/*, Task task*/) {
        SetDestination(goal);
    }

    private void SetDestination(Vector3 destination) {
        navMeshAgent.destination = destination;
    }

    //UI
    public void ShowUI(SelectedObjectArea area) {
        area.objectNameText.text = gameObject.name;
        area.objectDescriptionText.text = "This villager fucks";
        area.objectProgressSlider.value = energyLevel;
    }
}
