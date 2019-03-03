using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum EnemyType : int {
    GRAY, YELLOW, BLUE, RED
}

public class Enemy : MonoBehaviour {

    public int xPos = 0;
    public int yPos = 0;
    private float xOffset = -4.5f;
    private float yOffset = 1;

    public GameObject[] enemys;
    public GameObject enemy;
    public int type;

    public GameController gameCTL;

    public GameObject bolt;
    public Transform shotSpawn;
    private float fireRate;
    private float delay;
    public AudioClip fire;
    public GameObject explosion;

    public iTweenPath ipathLeft;
    public iTweenPath ipathRight;
    public GameObject[] Randompath;

    Vector3[] path;
    Vector3[] ipath;


    private bool IsAttack;
    private Vector3 playerTrans;

    public int value;

    private Transform parentTrans;



    void Start() {
        IsAttack = false;
        StartCoroutine(Fire());        
        //ShakePosition();
    }



    private void AddBG() {
        if (enemy != null)
            return;
        if (yPos > 4) {
            type = (int)EnemyType.RED;
            value = 40;
        } else if (yPos > 3) {
            type = (int)EnemyType.YELLOW;
            value = 30;
        } else if (yPos > 2) {
            type = (int)EnemyType.BLUE;
            value = 20;
        } else {
            type = (int)EnemyType.GRAY;
            value = 10;
        }
        enemy = (GameObject)Instantiate(enemys[type]);
        enemy.transform.parent = transform;


    }
    public void UpdatePosition() {
        AddBG();
        parentTrans = GameObject.FindGameObjectWithTag("EnemyParent").transform;
        transform.position = new Vector3((xPos) + xOffset + parentTrans.position.x, (yPos) + yOffset + parentTrans.position.y, 0);
        //ShakePosition();

    }

    public void GetIPath() {
        iTweenPath mypath = Randompath[Random.Range(0, Randompath.Length)].GetComponent<iTweenPath>();
        ipath = new Vector3[mypath.nodeCount];
        for (int i = 0; i < mypath.nodeCount; i++) {
            ipath[i] = new Vector3(xPos + xOffset, yPos + yOffset) + mypath.nodes[i];
        }
        transform.position += mypath.nodes[0];

    }
    public void TweenToPosition() {

        GetIPath();
        transform.DOLocalPath(ipath, 4f, PathType.CatmullRom, PathMode.TopDown2D, 5).OnComplete(ShakePosition);

        //transform.position=Vector3.Lerp (transform.position,new Vector3(xPos + xOffset + parentTrans.position.x, yPos + yOffset + yOffset + parentTrans.position.y, 0),10f );
    }

    /* ShakePosition()
    {        
        while (!IsAttack)
        {            
            transform.DOShakePosition(5f, new Vector3(0.05f, 0.05f, 0), 1, 1f, false, true);            
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            //transform.DOPlayBackwards();
        }
    }*/

    public void ShakePosition() {
        /*if (IsAttack)
            transform.DOShakePosition(5f, new Vector3(0.05f, 0.05f, 0), 1, 1f, false, true).Kill();
        else*/
        //transform.DOShakePosition(10f, new Vector3(0.05f, 0.05f, 0), 1, 1f, false, false).SetLoops(-1, LoopType.Restart).SetDelay(5f);
        transform.DOShakePosition(10f, new Vector3(0.05f, 0.05f, 0), 1, 1f, false, false).SetLoops(-1, LoopType.Restart);

    }
    IEnumerator Fire() {
        while (GameController.levelstart && GameController.IsGameOver == false) {
            yield return new WaitForSeconds(Random.Range(5f, 30f));
            if (!IsAttack) {
                Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);
                AudioSource.PlayClipAtPoint(fire, transform.position);
                yield return new WaitForSeconds(Random.Range(5f, 30f));
            }
        }
    }

    void GetPath() {
        if (xPos > 4) {
            path = new Vector3[ipathRight.nodeCount];
            for (int i = 0; i < ipathRight.nodeCount; i++) {
                path[i] = transform.position + ipathRight.nodes[i];
            }
        } else {
            path = new Vector3[ipathLeft.nodeCount];
            for (int i = 0; i < ipathLeft.nodeCount; i++) {
                path[i] = transform.position + ipathLeft.nodes[i];
            }
        }
    }

    public void Attack() {
        IsAttack = true;
        float speed = Random.Range(1f, 3f);
        GetPath();
        playerTrans = GameObject.FindGameObjectWithTag(Tags.Player).transform.position + new Vector3(0, -4, 0);
        transform.DOPath(path, 1f, PathType.CatmullRom, PathMode.TopDown2D, 5).SetLookAt(0.01f);
        transform.DOMove(playerTrans, speed).SetDelay(1f);
        //transform.DOLookAt(playerTrans, speed,AxisConstraint.Z).SetDelay(1f);
        Destroy(gameObject, speed + 1);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == Tags.Bolt) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
            GameController.score += value;
        }
    }
}
