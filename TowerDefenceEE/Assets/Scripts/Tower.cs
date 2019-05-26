using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenShots;
    [SerializeField]
    private float shotRadius;
    [SerializeField]
    private Projectiles projectile;

    private EnemyScript targetEnemy = null;
    private float attackCounter; // delay between shots fired
    private bool isAttacking = false;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        attackCounter -= Time.deltaTime;
        if (targetEnemy == null)
        {
            EnemyScript closestEnemy = GetClosestEnemy();

            if (closestEnemy != null && Vector2.Distance(transform.localPosition, closestEnemy.transform.localPosition) <= shotRadius)
            {
                targetEnemy = closestEnemy;
            }
        }

        else
        {
            if (attackCounter <= 0f)
            {
                isAttacking = true;
                attackCounter = timeBetweenShots;
            }
            else
            {
                isAttacking = false;
            }

            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > shotRadius)
            {
                targetEnemy = null;
            }
        }

    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = false;
        Projectiles newProjectile = Instantiate(projectile) as Projectiles;
        newProjectile.transform.localPosition = transform.localPosition;
        if (targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            //projectile goes to enemy
            StartCoroutine(ProjectileMovement(newProjectile));
        }
    }

    IEnumerator ProjectileMovement(Projectiles projectile)
    {
        while (getTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            var direction = targetEnemy.transform.localPosition - transform.localPosition;
            var directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.AngleAxis(directionAngle, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if (projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }

    private float getTargetDistance(EnemyScript enemy)
    {
        if (enemy == null)
        {
            enemy = GetClosestEnemy();
            if (enemy == null)
            {
                return 0f;
            }
        }

        return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));

    }

    private List<EnemyScript> GetAllEnemyInRange()
    {
        List<EnemyScript> enemiesInRange = new List<EnemyScript>();
        foreach (EnemyScript enemy in GameManager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= shotRadius)
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

        foreach (EnemyScript enemy in GetAllEnemyInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
