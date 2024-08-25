using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputUI : MonoBehaviour{

    public string matchingID = "";

    [Space]
    [Header("References")]
    [Space]
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

        } else {
            Destroy(gameObject);
        }
        
    }
}
