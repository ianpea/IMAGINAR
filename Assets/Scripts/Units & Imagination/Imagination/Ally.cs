using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ally can only be spawned on Trackable surface.
/// </summary>
public class Ally : Imagination
{
    public float existTime = 0.0f;
    public float despawnTime;

    public float attackDamage;
    public float maxAttackRange;

    public float attackCooldown;
    public float lastAttackTime;

    private float closestEnemyDist = Mathf.Infinity;

    private void Start()
    {
        lastAttackTime = -attackCooldown;
    }

    private void Update()
    {
        if (spawned)
        {
            existTime += Time.deltaTime;
            if (existTime > despawnTime)
            {
                ParticleManager.Instance.Play(disappearPFX, transform);
                AllyPool.Instance.Despawn(this);
                return;
            }
        }
        Attack();
    }

    public override void Attack()
    {
        if (Time.time - attackCooldown < lastAttackTime)
        {
            return;
        }
        else
        {
            lastAttackTime = (float) Time.time;
        }

        GameObject e = GetNearestEnemy();
        if(e == null)
        {
            return;
        }

        float eRange = (e.transform.position - transform.position).magnitude;
        Vector3 attackDir = (e.transform.position - transform.position).normalized;
        if (eRange <= maxAttackRange)
        {
            BulletPool.Instance.BulletFire(attackDir, gameObject);
            AudioManager.Instance.SFXShootPlant();
        }
    }

    private GameObject GetNearestEnemy()
    {
        GameObject e = null;
        closestEnemyDist = Mathf.Infinity;
        for (int i = 0; i < MonsterPool.Instance.pool.Count; i++)
        {
            if (MonsterPool.Instance.pool[i].gameObject.activeInHierarchy)
            {
                e = MonsterPool.Instance.pool[i].gameObject;
                closestEnemyDist = (MonsterPool.Instance.pool[i].gameObject.transform.position - transform.position).magnitude;
            }
            else
            {
                continue;
            }
        }
        return e;
    }

    public void Reset()
    {
        existTime = 0.0f;
        spawned = false;
    }
}
