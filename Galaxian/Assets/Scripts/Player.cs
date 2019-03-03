using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class Boundary {
    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;

}

public class Player : MonoBehaviour {


    public Boundary bound;
    public GameObject bolt;
    private Rigidbody2D rig;
    private AudioSource playerAudio;
    public Transform[] shotSpawns;
    public float Speed = 10.0f;
    public float fireRate = 0.25f;
    private float nextFire = 0;
    public AudioClip fireClip;

    public static bool IsPress;

    public GameObject playerExplosion;

    
    void Start() {
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();

        bound = new Boundary();
        IsPress = false;
    }
    
    void Update() {
        if (GameController.Timer >= 3) {
            nextFire = Time.deltaTime + nextFire;
            if (!IsPress) {
                if (Input.GetKey("space") && nextFire > fireRate) {
                    nextFire = 0;
                    foreach (var spawnPos in shotSpawns) {
                        Instantiate(bolt, spawnPos.position, spawnPos.rotation);
                    }
                    playerAudio.PlayOneShot(fireClip);
                }
            }
        }
    }

    private void FixedUpdate() {
        if (GameController.Timer >= 3) {
            float h = Input.GetAxis("Horizontal");
            Vector3 move = new Vector3(h, 0, 0);
            rig.velocity = move * Speed;
            rig.position = new Vector3(Mathf.Clamp(rig.position.x, -6.7f, 6.7f), -7, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == Tags.EnemyBolt || other.tag == Tags.Enemy) {

            GameController.life -= 1;
            Instantiate(playerExplosion, transform.position, transform.rotation);
            gameObject.transform.DOPunchPosition(new Vector3(0.5f, 0, 0), 0.5f);
            Destroy(other.gameObject);
            if (GameController.life < 1) {
                Destroy(gameObject);
            }
        }
    }
}
