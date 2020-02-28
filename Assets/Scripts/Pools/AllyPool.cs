using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AllyPool : Pool<Ally>
{
    /// <summary>
    /// Singleton.
    /// </summary>
    private static AllyPool _instance;
    public static AllyPool Instance { get { return _instance; } }

    private List<Ally> allyPool;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        InstantiatePool();
    }

    void Update()
    {
        UpdatePool();
    }

    public override void InstantiatePool()
    {
        _instance.allyPool = new List<Ally>(MAX_OBJECTS);
        _instance.pool = _instance.allyPool;
        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            Ally ally = Instantiate(_instance.prefab[0], this.transform).GetComponent<Ally>();
            ally.gameObject.SetActive(false);
            _instance.pool.Add(ally);
        }
    }

    public override Ally GetObject()
    {
        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            if (!_instance.pool[i].gameObject.activeInHierarchy)
            {
                return _instance.pool[i];
            }
            else
            {
                continue;
            }
        }
        Debug.LogWarning("Animal pool maxed out. Returning NULL.");
        return null;
    }

    public override void UpdatePool()
    {
        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            if ((_instance.pool[i].transform.position - ARCoreView.Instance.transform.position).magnitude > MAX_RANGE_FROM_PLAYER)
            {
                Despawn(_instance.pool[i]);
            }
        }
    }

    public override void Spawn(TrackableHit hit)
    {
        Ally ally = GetObject();

        if (ally == null)
        {
            return;
        }
        if (!ally.gameObject.activeInHierarchy)
        {
            ally.gameObject.SetActive(true);
            ally.gameObject.transform.position = hit.Pose.position;
            ally.spawned = true;

            ally.transform.Rotate(0, ARCoreModel.k_PrefabRotation, 0, Space.Self);
            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
            ally.transform.parent = anchor.transform;
        }
    }

    public override void Despawn(Ally ally)
    {
        if (ally.tag == "Ally")
        {
            ally.transform.rotation = Quaternion.identity;
            ally.transform.position = Vector3.zero;
            ally.Reset();
            ally.gameObject.SetActive(false);
            Ally temp = ally;
            pool.Remove(ally);
            pool.Insert(pool.Count - 1, temp);
        }
    }
}
