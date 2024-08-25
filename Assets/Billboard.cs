using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour{

    public Transform reference;

    void Update(){

        transform.LookAt(reference.position);
        
    }
}
