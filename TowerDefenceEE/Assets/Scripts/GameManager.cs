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
    private int totalEnemies = 3; 
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

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int WaveEscaped
    {
        get
        {
            return waveEscaped;
        }
        set
        {
            waveEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
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
                if (EnemyList.Count < totalEnemies)
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

    public void isWaveDone()
    {
        lblTotalEscaped.text = $"Escaped {TotalEscaped}/10";
        if ((WaveEscaped+TotalKilled)==totalEnemies)
        {
            setCurrentGameState();
            showMenu();
        }
    }

    private void setCurrentGameState()
    {
        //hard coded after 10 escaped enemies the game is over
        if (TotalEscaped>=10)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber==0 && (TotalKilled+WaveEscaped)==0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber>=totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void playButtonPressed()
    {
        Debug.Log("pressed");
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                totalEscaped = 0;
                TotalMoney = 40;
                waveNumber = 0;
                TowerManager.Instance.destroyAllTowers();
                TowerManager.Instance.renameBuildsiteTags();
                lblTotalMoney.text = TotalMoney.ToString();
                lblTotalEscaped.text = $"Escaped {TotalEscaped}/10";
                break;
        }

        DestroyAllEnemies();
        TotalKilled = 0;
        WaveEscaped = 0;
        lblCurrentWave.text = $" Wave {waveNumber + 1}";
        StartCoroutine(EnemySpawner());
        playButton.gameObject.SetActive(false);
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
