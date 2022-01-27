using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Vector3 goal;
    public Transform homeLocation;
    public SelectedVillagerUI villagerUIPrefab;
    private SelectedVillagerUI uiInstance;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //uiInstance = Instantiate(villagerUIPrefab);
        //uiInstance.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void DoTask() {
        
    }

    public void showUi(Vector3 location) {
        //uiInstance.transform.position = location;
        //uiInstance.enabled = true;
    }

    public void onUnselect() {
        //uiInstance.enabled = false;
    }

    // Assign will have a task as well
    public void Assign(Vector3 goal/*, Task task*/) {
        SetDestination(goal);
    }

    private void SetDestination(Vector3 destination) {
        navMeshAgent.destination = destination;
    }

    public class Task {
        string name;
        float duration;
        Vector3 location;
        Villager worker;

        public Task(string name, float duration, Vector3 location, Villager worker) {
            this.name = name;
            this.duration = duration;
            this.location = location;
            this.worker = worker;
        }

        public virtual void DoTask() {
            worker.Assign(location);
        }
    }
}
