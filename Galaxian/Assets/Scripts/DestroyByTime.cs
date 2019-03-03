using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public float DestroyTime;
    // Use this for initialization
    void Start() {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update() {

    }
}
