using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputSelector : MonoBehaviour{

    public string matchingID = "";

    [Space]
    [Header("References")]
    [Space]
    public TMP_Text idText;
    public Toggle toggleLeft;
    public Toggle toggleRight;

    StimulationOutput stimulationOutput;

    private void Start() {
        GameObject go = GameObject.Find(matchingID);
        if (go != null) {
            stimulationOutput = go.GetComponent<StimulationOutput>();
            idText.text = matchingID;
            stimulationOutput.gameObject.SetActive(toggleLeft.isOn || toggleRight.isOn);
            if (toggleLeft.isOn) stimulationOutput.setStereoPan(-1);
            if (toggleRight.isOn) stimulationOutput.setStereoPan(1);
        } else {
            Destroy(gameObject);
        }

    }

    void onToggleLeft(bool value) {
        if (value) {
            stimulationOutput.setStereoPan(-1);
        }
        stimulationOutput.gameObject.SetActive(toggleLeft.isOn || toggleRight.isOn);
    }

    void onToggleRight(bool value) {
        if (value) {
            stimulationOutput.setStereoPan(1);
        }
        stimulationOutput.gameObject.SetActive(toggleLeft.isOn || toggleRight.isOn);
    }

}
