using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


    public Enemy enemy;
    public static readonly int colNumber = 10;
    public static readonly int rowNumber = 6;

    public Transform EnemyParent;
    private ArrayList enemyArray;

    public Boundary bound;

    public static int life;
    public GameObject[] lifes;
    public static bool IsGameOver;

    public static int level;
    public GameObject[] levels;
    public static bool levelstart;

    public GameObject gameoverPanel;
    public GameObject winPanel;

    public static int score;
    public Text scoreText;

    public static int hiScore;
    public Text hiScoreText;
    public static int newUserScore = 0;

    public static float Timer;
    private float CountDown;
    public Text CountDownText;






    void Start() {

        CountDown = 3f;
        Timer = 0;
        life = 3;
        score = 0;
        level = 0;
        hiScore = PlayerPrefs.GetInt("HISCORE", newUserScore);
        hiScoreText.text = hiScore.ToString();
        IsGameOver = false;
        levelstart = true;
        gameoverPanel.SetActive(false);
        winPanel.SetActive(false);
        foreach (GameObject image in levels) {
            image.SetActive(false);
        }
        foreach (GameObject image in lifes) {
            image.SetActive(true);
        }
        enemyArray = new ArrayList();
        CreatEnemyArray();
        StartCoroutine(CreatRandomEnemy());
        StartCoroutine(EnemyAttack());


    }


    void CreatEnemyArray() {
        if (IsGameOver == false) {
            for (int y = 0; y < rowNumber; y++) {
                ArrayList tmp = new ArrayList();
                for (int x = 0; x < colNumber; x++) {
                    Enemy e = null;
                    if (x >= y - 2 && x <= rowNumber + 5 - y) {
                        e = AddEnemy(x, y);
                        e.ShakePosition();
                    }
                    tmp.Add(e);
                }
                enemyArray.Add(tmp);
            }
            levelstart = true;
        }

    }

    IEnumerator CreatRandomEnemy() {
        yield return new WaitForSeconds(5f);
        while (levelstart && IsGameOver == false) {
            int x = Random.Range(0, colNumber - 1);
            int y = Random.Range(0, rowNumber - 1);
            if (x >= y - 2 && x <= rowNumber + 5 - y) {
                Enemy e = GetEnemy(x, y);
                if (e == null) {
                    //Debug.Log("Enemy Created");                    
                    e = AddEnemy(x, y);
                    e.TweenToPosition();
                    SetEnemy(x, y, e);
                    yield return new WaitForSeconds(Random.Range(3f, 10f));
                }
                yield return new WaitForSeconds(Random.Range(2f, 4f));
            }
        }
    }

    public void CheckLifes() {
        switch (life) {
            case 2:
                lifes[2].SetActive(false);
                break;
            case 1:
                lifes[1].SetActive(false);
                break;
            case 0:
                lifes[0].SetActive(false);
                GameOver();
                break;
            default:
                break;
        }

    }
    void CheckLevel() {
        switch (level) {
            case 1:
                levels[0].SetActive(true);
                break;
            case 2:
                levels[1].SetActive(true);
                break;
            case 3:
                levels[2].SetActive(true);
                break;
            case 4:
                Win();
                break;
            default:
                break;
        }
    }
    void Update() {
        CheckLifes();
        CheckLevel();

        Timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (CountDown > 0) {
            CountDown -= Time.deltaTime;
            CountDownText.text = Mathf.CeilToInt(CountDown) + "";
        } else
            CountDownText.text = "";


        scoreText.text = score.ToString();
        if (score > hiScore) {
            scoreText.color = Color.HSVToRGB(0, 214, 236);
            hiScoreText.color = Color.gray;
        }
        if (levelstart) {
            if (CheckEnemy()) {
                level++;
                enemyArray.Clear();
                levelstart = false;
                Invoke("CreatEnemyArray", 2f);
            }
        }
    }




    private Enemy AddEnemy(int xPos, int yPos) {
        Object o = Instantiate(enemy);
        Enemy e = o as Enemy;
        e.transform.parent = EnemyParent;
        e.xPos = xPos;
        e.yPos = yPos;
        e.UpdatePosition();
        e.gameCTL = this;
        return e;
    }


    private Enemy GetEnemy(int x, int y) {
        ArrayList tmp = enemyArray[y] as ArrayList;
        Enemy e = tmp[x] as Enemy;
        return e;
    }

    private void SetEnemy(int x, int y, Enemy e) {
        ArrayList tmp = enemyArray[y] as ArrayList;
        tmp[x] = e;
    }

    IEnumerator EnemyAttack() {
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        while (levelstart && IsGameOver == false) {
            int x = Random.Range(0, colNumber - 1);
            int y = Random.Range(0, rowNumber - 1);
            Enemy e = GetEnemy(x, y);
            if (e != null) {
                e.Attack();
                yield return new WaitForSeconds(Random.Range(5f, 30f - (5 * level)));
            }
        }
    }

    void GameOver() {
        IsGameOver = true;
        if (score > hiScore) {
            PlayerPrefs.SetInt("HISCORE", score);
        }
        gameoverPanel.SetActive(true);
        if (Input.GetKey(KeyCode.Return))
            SceneManager.LoadScene(1);
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
    void Win() {
        IsGameOver = true;
        if (score > hiScore) {
            PlayerPrefs.SetInt("HISCORE", score);
        }
        winPanel.SetActive(true);
        if (Input.GetKey(KeyCode.Return))
            SceneManager.LoadScene(1);
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

    }

    public bool CheckEnemy() {
        bool result = false;
        int counter = 0;
        for (int y = 0; y < rowNumber; y++) {
            for (int x = 0; x < colNumber; x++) {
                Enemy e = GetEnemy(x, y);
                if (e != null) {
                    counter++;
                    break;
                }
            }
            if (counter > 0)
                break;
        }
        if (counter == 0)
            result = true;
        return result;
    }


}
