using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Vector3 goal;
    public Transform homeLocation;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void DoTask() {
        
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
