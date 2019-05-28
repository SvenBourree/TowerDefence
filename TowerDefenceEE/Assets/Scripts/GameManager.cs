using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
};

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject[] enemies; 
    [SerializeField]
    private int maxEnemiesOnScreen; 
    [SerializeField]
    private int totalEnemies; 
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Text lblTotalMoney;
    [SerializeField]
    private Text lblCurrentWave;
    [SerializeField]
    private Text lblTotalEscaped;
    [SerializeField]
    private Text lblPlayButton;
    [SerializeField]
    private Button playButton;


    private int waveNumber = 0;
    private int totalMoney = 40;
    private int totalEscaped = 0;
    private int waveEscaped = 0;
    private int totalKilled = 0;
    private int wichEnemiesToSpawn = 0;
    private gameStatus currentState = gameStatus.play;


    public List<EnemyScript> EnemyList = new List<EnemyScript>();


    const float spawnDelay = 0.5f; //every half second enemy spawns

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            lblTotalMoney.text = totalMoney.ToString();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playButton.gameObject.SetActive(false);
        showMenu();

    }

    private void Update()
    {
        handleDeselectTower();
    }



    IEnumerator EnemySpawner()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < maxEnemiesOnScreen)
                {
                    // needs to be cast as gameobject because Instantiate creates an object not a Gameobject
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;

                    newEnemy.transform.position = spawnPoint.transform.position;
                    
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(EnemySpawner());
        }
    }

    public void RegisterEnemy(EnemyScript enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnRegisterEnemy(EnemyScript enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach (EnemyScript enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void subtractMoney (int amount)
    {
        TotalMoney -= amount;
    }

    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                lblPlayButton.text = "Play again";
                break;
            case gameStatus.next:
                lblPlayButton.text = "Next Wave";
                
                break;
            case gameStatus.play:
                lblPlayButton.text = "Play";
                break;
            case gameStatus.win:
                lblPlayButton.text = "Play";
                
                break;
        }
        playButton.gameObject.SetActive(true);
    }

    private void handleDeselectTower()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TowerManager.Instance.disableDragTower();
            TowerManager.Instance.towerButtonPressed = null;
        }
    }

}
