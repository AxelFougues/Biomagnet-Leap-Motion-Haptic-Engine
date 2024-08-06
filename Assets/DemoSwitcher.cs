using System.Collections.Generic;
using UnityEngine;

public class DemoSwitcher : MonoBehaviour{

    public int startingDemo;
    public List<GameObject> demos;

    int currentDemo = 0;

    void Start(){
        if (startingDemo < 0 || startingDemo > demos.Count) startingDemo = 0;
        for (int i = 0; i < demos.Count; i++) {
            if (demos[i] == null) {
                demos.RemoveAt(i);
                i--;
            } else {
                demos[i].SetActive(i == startingDemo);
            }
        }
        currentDemo = startingDemo;
    }

    public void next() {
        demos[currentDemo].SetActive(false);
        currentDemo++;
        if (currentDemo >= demos.Count) currentDemo = 0;
        demos[currentDemo].SetActive(true);
    }

    public void previous() {
        demos[currentDemo].SetActive(false);
        currentDemo--;
        if (currentDemo < 0) currentDemo = demos.Count - 1;
        demos[currentDemo].SetActive(true);
    }

}
