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

    public StimulationOutput stimulationOutput;

    private void Start() {
        idText.text = matchingID;
        stimulationOutput.gameObject.SetActive(toggleLeft.isOn || toggleRight.isOn);
        if (toggleLeft.isOn) stimulationOutput.setStereoPan(-1);
        if (toggleRight.isOn) stimulationOutput.setStereoPan(1);
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
