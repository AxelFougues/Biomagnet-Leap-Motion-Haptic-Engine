using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputUI : MonoBehaviour{

    public string matchingID = "";

    [Space]
    [Header("References")]
    [Space]
    public Toggle toggle;
    public TMP_Text text;
    public TMP_Text idText;

    StimulationOutput stimulationOutput;

    private void Start() {
        GameObject go = GameObject.Find(matchingID);
        if (go != null) {
            stimulationOutput = go.GetComponent<StimulationOutput>();
            stimulationOutput.setUI(this);
            idText.text = matchingID;
            text.text = "0%";
            toggle.onValueChanged.AddListener(onToggle);

        } else {
            Destroy(gameObject);
        }
        
    }


    public void onToggle(bool value) {
        stimulationOutput.gameObject.SetActive(value);
        if (value) text.text = "0%";
        else text.text = "--";
    }
}
