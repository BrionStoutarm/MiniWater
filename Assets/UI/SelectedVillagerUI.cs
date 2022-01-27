using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedVillagerUI : MonoBehaviour
{
    public Text villagerName;
    public Text villagerJob;
    public Slider villagerEnergy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setName(string name) {
        villagerName.text = name;
    }

    public void setJob(string job) {
        villagerJob.text = job;
    }
}
