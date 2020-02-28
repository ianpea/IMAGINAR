using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhenomenonPool : Pool<Phenomenon>
{
    /// <summary>
    /// Singleton.
    /// </summary>
    private static PhenomenonPool _instance;
    public static PhenomenonPool Instance { get { return _instance; } }

    private List<Phenomenon> phenomenonPool;

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
        _instance.phenomenonPool = new List<Phenomenon>(MAX_OBJECTS);
        _instance.pool = _instance.phenomenonPool;
        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            Phenomenon phenomenon = Instantiate(prefab[Random.Range(0, _instance.prefab.Count)], this.transform).GetComponent<Phenomenon>();
            phenomenon.gameObject.SetActive(false);
            _instance.pool.Add(phenomenon);
        }
    }

    public override Phenomenon GetObject()
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
        Debug.LogWarning("Phenomenon pool maxed out. Returning NULL.");
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

    public override void Spawn(Vector3 position)
    {
        Phenomenon phenomenon = GetObject();

        if (phenomenon == null)
        {
            return;
        }
        if (!phenomenon.gameObject.activeInHierarchy)
        {
            phenomenon.gameObject.SetActive(true);
            phenomenon.transform.position = position;
            phenomenon.spawned = true;
        }
    }

    public override void Despawn(Phenomenon phenomenon)
    {
        if (phenomenon.tag == "Phenomenon")
        {
            phenomenon.transform.rotation = Quaternion.identity;
            phenomenon.transform.position = Vector3.zero;
            phenomenon.gameObject.SetActive(false);
        }
    }
}
