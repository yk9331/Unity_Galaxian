using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour {

    void Update() {
        if (Input.GetKey(KeyCode.Return))
            SceneManager.LoadScene(1);
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
