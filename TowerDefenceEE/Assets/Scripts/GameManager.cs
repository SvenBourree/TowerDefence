using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{


    public GameObject spawnPoint;
    public GameObject[] enemies; //currentely doesnt need to be an array because only 1 enemy
    public int maxEnemiesOnScreen; // how many on the screen
    public int totalEnemies; //how many enemies in the wave
    public int enemiesPerSpawn; //how many spawn per time
    public List<EnemyScript> EnemyList = new List<EnemyScript>();


    const float spawnDelay = 0.5f; //every half second enemy spawns




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());

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
                    GameObject newEnemy = Instantiate(enemies[1]) as GameObject;

                    newEnemy.transform.position = spawnPoint.transform.position;
                    
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(EnemySpawner());
        }
    }

    public void RegistEnemy(EnemyScript enemy)
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


}
