using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenShots;
    [SerializeField]
    private float shotRadius;

    private Projectiles projectile;
    private EnemyScript targetEnemy = null;
    private float attackCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<EnemyScript> GetAllEnemy()
    {
        List<EnemyScript> enemiesInRange = new List<EnemyScript>();
        foreach (EnemyScript enemy in GameManager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position)<= shotRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    private EnemyScript GetClosestEnemy()
    {
        EnemyScript closestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (EnemyScript enemy in GetAllEnemy())
        {
            if (Vector2.Distance(transform.position, enemy.transform.position)<smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
