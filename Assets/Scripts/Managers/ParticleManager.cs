using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager _instance;
    public static ParticleManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            if(_instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        UpdatePFXList();
    }

    public List<ParticleSystem> pfxList;

    /// <summary>
    /// Add to pfxlist and play particle pfx at transform t.
    /// </summary>
    /// <param name="pfx"></param>
    /// <param name="t"></param>
    public void Play(ParticleSystem pfx, Transform t)
    {
        ParticleSystem p = Instantiate(pfx, t.position, Quaternion.identity);
        pfxList.Add(p);
        p.transform.parent = transform;
        Destroy(p.gameObject, 3.0f);
    }

    public void UpdatePFXList()
    {
        for(int i = 0; i < pfxList.Count; i++)
        {
            if(pfxList[i] == null)
            {
                pfxList.Remove(pfxList[i]);
            }
        }
    }
}
