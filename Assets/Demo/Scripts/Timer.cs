using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour{

    public TMP_Text text;

    float time = 0;

    private void OnEnable() {

        time = 0;
        text.text = "0s";

    }

    private void Update() {

        time += Time.deltaTime;
        text.text = time.ToString("0.0") + "s";

    }

}
