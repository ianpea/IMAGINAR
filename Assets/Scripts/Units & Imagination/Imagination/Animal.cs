using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class Animal : Imagination
{
    public ParticleSystem disintegratePFX;
    public float existTime = 0.0f;
    public float despawnTime;

    // Animal fields
    public float closestPhenomenonDist = Mathf.Infinity;
    public float moveDist;
    public float timeToDisintegrate = 10.0f;
    public float maxDistFromPhenomenon = 1.0f;

    private float timeToStartMoving = 5.0f;
    private Coroutine disintegrateCo;
    private bool isDisintegrating;
    private Vector3 oriScale;

    void Start()
    {
        type = IMAGINATION_TYPE.Animal;
        oriScale = transform.localScale;
    }

    private void Update()
    {
        if (spawned)
        {
            existTime += Time.deltaTime;
            if (existTime > despawnTime)
            {
                ParticleManager.Instance.Play(disappearPFX, transform);
                AudioManager.Instance.SFXDeathAnimal();
                AnimalPool.Instance.Despawn(this);
                return;
            }
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
    /// Move to closest phenomenon after 5s of spawning.
    /// </summary>
    public override void Action()
    {
        GameObject p = GetNeartestPhenomenon();
        if (p == null)
        {
            anim.SetBool("isWalking", false);
            return;
        }
        Vector3 targetDir = (p.transform.position - gameObject.transform.position);

        if (targetDir.magnitude <= maxDistFromPhenomenon)
        {
            anim.SetBool("isWalking", false);
            Disintegrate();
            return;
        }

        if (p != null && GameManager.Instance.Model.sessionAnchor != null)
        {
            transform.LookAt(p.transform);
            gameObject.transform.position += targetDir.normalized * moveDist * Time.deltaTime;
            currenState = IMAGINATION_STATE.Moving;
        }
        anim.SetBool("isWalking", true);
    }

    /// <summary>
    /// Returns the nearest phenomenon closest to this animal.
    /// </summary>
    /// <returns></returns>
    private GameObject GetNeartestPhenomenon()
    {
        GameObject p = null;
        closestPhenomenonDist = Mathf.Infinity;
        for (int i = 0; i < PhenomenonPool.Instance.pool.Count; i++)
        {
            if (PhenomenonPool.Instance.pool[i].gameObject.activeInHierarchy)
            {
                if ((PhenomenonPool.Instance.pool[i].gameObject.transform.position - transform.position).magnitude < closestPhenomenonDist)
                {
                    p = (PhenomenonPool.Instance.pool[i].gameObject);
                    closestPhenomenonDist = (PhenomenonPool.Instance.pool[i].gameObject.transform.position - transform.position).magnitude;
                }
            }
            else
            {
                continue;
            }
        }
        return p;
    }

    /// <summary>
    /// Slowly disintegrate when animal reaches the phenomenon.
    /// </summary>
    public override void Disintegrate()
    {
        if (currenState != IMAGINATION_STATE.Disintegrating && !isDisintegrating)
        {
            disintegrateCo = StartCoroutine(DisintegrateCo());
            currenState = IMAGINATION_STATE.Disintegrating;
        }
        // loop pfx until finish disintegrate
    }

    private IEnumerator DisintegrateCo()
    {
        isDisintegrating = true;
        float decrement = Time.fixedDeltaTime / timeToDisintegrate;
        for (float t = 0.0f; t < timeToDisintegrate; t += Time.fixedDeltaTime)
        {
            transform.localScale *= (1.0f - decrement*3);
            yield return null;
        }
        ParticleManager.Instance.Play(disintegratePFX, transform);
        AnimalPool.Instance.Despawn(this);
        isDisintegrating = false;
        yield return null;
    }

    /// <summary>
    /// Reset the animal's field.
    /// </summary>
    public void Reset()
    {
        existTime = 0.0f;
        closestPhenomenonDist = Mathf.Infinity;
        spawned = false;
        gameObject.transform.localScale = oriScale;
    }
}
