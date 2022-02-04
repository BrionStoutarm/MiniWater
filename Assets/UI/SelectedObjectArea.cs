using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectArea : MonoBehaviour
{
    public Text objectNameText;
    public Text objectDescriptionText;
    public Slider objectProgressSlider;

    private bool visible;
    private Vector3 origScale;
    private ObjectUI selectedObject;

    public void Awake() {
        origScale = transform.localScale;
        objectProgressSlider.minValue = 0.0f;
        objectProgressSlider.maxValue = 100.0f;
    }

    public void SetVisible(bool visible) {
        this.visible = visible;
        if (visible) {
            transform.localScale = origScale;
        }
        else {
            transform.localScale = Vector3.zero;
        }
    }

    public void Update() {
        if(visible) {
            ShowUI();
        }
    }

    private void ShowUI() {
        if(selectedObject != null) {
            ObjectUIData data = selectedObject.GetUIData();
            objectNameText.text = data.objectName;
            objectDescriptionText.text = data.objectDescription;
            if(data.hasProgress) {
                objectProgressSlider.value = data.progressAmount;
            }
        }
    }

    public void Clear() { selectedObject = null; }
    public void SetObject(ObjectUI selectedObject) { this.selectedObject = selectedObject; visible = true; }
}
