using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    public bool isGained;
    public bool isAttacking;
    public float attackRange = 2.0f;
    public int attackDamage = (int) LOSE.BossAttack;
    public float moveDist;
    public float lastAttackTime;
    public float attackCooldown;
    private float timeToStartMoving = 7.5f;
    private float maxHp = 2500;

    void Start()
    {
        spawned = true;
        AudioManager.Instance.SFXBossSpawned();
    }

    void Update()
    {
        if (spawned)
        {
            existTime += Time.deltaTime;
            if(existTime > despawnTime)
            {
                ParticleManager.Instance.Play(disappearPFX, transform);
                Destroy(gameObject);
            }
            CheckDeath();
            UpdateSize();
        }


    }

    private void FixedUpdate()
    {
        if(spawned && existTime >= timeToStartMoving)
        {
            Action();
        }
    }

    /// <summary>
    /// Lesser hp, bigger size. Down to minimum 0.3f scale.
    /// </summary>
    private void UpdateSize()
    {
        float scale;
        if (transform.localScale.x <= 1.5f)
        {
            scale = maxHp / hp;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void Action()
    {
        Vector3 targetDir = ARCoreView.Instance.transform.position - transform.position;

        if(targetDir.magnitude >= attackRange)
        {
            anim.SetBool("isWalking", true);
            transform.LookAt(ARCoreView.Instance.transform.position);
            transform.position += targetDir.normalized * moveDist * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isWalking", false);
            if(Time.time >= lastAttackTime + attackCooldown && maxHp >= 0) // only attack when is alive
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Each boss attack increase its own damage by 10.
    /// </summary>
    private void Attack()
    {
        attackDamage += 10;
        anim.SetTrigger("TriggerAttack");
        ScoreManager.Instance.Deduct(attackDamage);
        AudioManager.Instance.Invoke("SFXDamaged", 0.7f); // hardcoded audio delay for boss attack animation to sync with audio
    }

    private void CheckDeath()
    {
        if(hp <= 0 && !isGained)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            anim.SetBool("isWalking", false);
            anim.SetTrigger("TriggerDie");
            Invoke("DestroyBoss", 6.0f);
            ScoreManager.Instance.Add(GAIN.Boss);
            AudioManager.Instance.SFXDeathMonster();
            isGained = true;
        }
    }

    private void DestroyBoss()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BulletPet" || collision.gameObject.tag == "BulletAlly")
        {
            hp -= 50;
            BulletPool.Instance.DespawnBullet(collision.gameObject);
        }
    }
}
