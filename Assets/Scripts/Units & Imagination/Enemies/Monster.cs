using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    public bool isWander = false;
    public bool isWandering = false;
    public float timeToWander = 3.0f;
    private float dampFactor = 0.6f;
    private Coroutine wanderCo;

    void Update()
    {
        if (spawned)
        {
            existTime += Time.deltaTime;
            if (existTime > despawnTime)
            {
                ParticleManager.Instance.Play(disappearPFX, transform);
                MonsterPool.Instance.Despawn(this);
            }
            CheckDeath();
        }
    }

    void FixedUpdate()
    {
        if (isWander && !isWandering && GameView.Instance.Model.currentState == GAME_STATE.Pet)
        {
            Action();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BulletPet" || collision.gameObject.tag == "BulletAlly")
        {
            hp-=50;
            BulletPool.Instance.DespawnBullet(collision.gameObject);
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Starts wandering.
    /// </summary>
    public void Action()
    {
        Vector3 targetPos, targetDir;
        float wanderDistance = Random.Range(2.5f, 3.5f);
        if ((transform.position - ARCoreView.Instance.gameObject.transform.position).magnitude > 1.5f)
        {
            targetPos = Random.insideUnitSphere * wanderDistance;
            targetDir = (targetPos - transform.position).normalized;
        }
        else
        {
            targetDir = (transform.position - ARCoreView.Instance.gameObject.transform.position).normalized;
            targetPos = transform.position + targetDir * wanderDistance;
        }
        base.FaceDirection(targetPos);
        wanderCo = StartCoroutine(WanderCo(targetPos, targetDir));
    }

    /// <summary>
    /// Wander coroutine for monster.
    /// </summary>
    /// <param name="targetPos">Target position to wander.</param>
    /// <param name="targetDir">Normalized target direction to wander.</param>
    /// <returns></returns>
    public IEnumerator WanderCo(Vector3 targetPos, Vector3 targetDir)
    {
        float dist = (targetPos - transform.position).magnitude;
        isWandering = true;
        for(float t = 0.0f; t < timeToWander; t += Time.fixedDeltaTime)
        {
            transform.position += targetDir * dampFactor * Time.fixedDeltaTime / timeToWander;
            yield return null;
        }
        isWandering = false;
        yield return null;
    }

    private void CheckDeath()
    {
        if (hp <= 0)
        {
            anim.SetTrigger("TriggerDie");
            ParticleManager.Instance.Play(destroyedPFX, transform);
            MonsterPool.Instance.Despawn(this);
            ScoreManager.Instance.Add(GAIN.Monster);
            AudioManager.Instance.SFXDeathMonster();
        }
        else
        {

            anim.SetTrigger("TriggerHit");
        }
    }
}
