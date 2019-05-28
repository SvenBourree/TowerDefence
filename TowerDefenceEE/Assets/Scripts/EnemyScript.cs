using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField]
    private Transform exit; //stuff happens when enemy gets to this point
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float navUpdate;
    [SerializeField]
    private int hitPoints;
    [SerializeField]
    private int reward;

    private int target = 0; //goes to checkpoint zero, checkpoints will be in array
    private Transform enemy;
    private Collider2D enemyCollider;
    private float navTime = 0;
    private bool isDead = false;

    public bool Isdead
    {
        get
        {
            return isDead;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints!=null && !isDead)
        {
            navTime += Time.deltaTime;
            if (navTime >navUpdate)
            {
                if (target < waypoints.Length)
                {
                    // build in methods moveTowards takes 3 args: current possition, target position, time
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navTime);
                }
                navTime = 0;
            }
        }
    }
    //make this 2D, it wont work otherwise you big dumb dumb
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="WayPoints")
        {
            target += 1;
        }
        else if (collision.tag== "Finish")
        {
            GameManager.Instance.UnRegisterEnemy(this);
            
        }
        else if (collision.tag == "Projectiles")
        {
           
            Projectiles newProjectile = collision.gameObject.GetComponent<Projectiles>();
            enemyHit(newProjectile.AttackStrength);
            //this destroys the projectiles on impact
            Destroy(collision.gameObject);
        }
    }

    public void dead()
    {
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.UnRegisterEnemy(this);
        //to do: death animation

    }

    public void enemyHit(int hp)
    {
        if (hitPoints - hp > 0 )            
        {
        hitPoints -= hp;
        }
        else
        {
            dead();
        }
    }

}
