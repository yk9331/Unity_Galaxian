using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

    public float Speed = 0;

    void Start() {
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 1, 0) * Speed;
    }


}
