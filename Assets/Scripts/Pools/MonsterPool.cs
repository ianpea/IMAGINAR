using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : Pool<Monster>
{
    /// <summary>
    /// Singleton.
    /// </summary>
    private static MonsterPool _instance;
    public static MonsterPool Instance { get { return _instance; } }

    private List<Monster> monsterPool;
    private List<GameObject> monsterGOs;

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

    private void Start()
    {
        InstantiatePool();
    }

    private void Update()
    {
        UpdatePool();
    }

    public override void InstantiatePool()
    {
        _instance.monsterPool = new List<Monster>(MAX_OBJECTS);
        _instance.pool = _instance.monsterPool;
        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            Monster monster = Instantiate(_instance.prefab[Random.Range(0, _instance.prefab.Count)], this.transform).GetComponent<Monster>();
            monster.gameObject.SetActive(false);
            _instance.pool.Add(monster);
        }
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

    public override Monster GetObject()
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
        Debug.LogWarning("Monster pool maxed out. Returning NULL.");
        return null;
    }

    public override void Spawn(Vector3 position)
    {

        Monster monster = GetObject();

        if (monster == null)
        {
            return;
        }
        if (!monster.gameObject.activeInHierarchy)
        {
            monster.gameObject.SetActive(true);
            monster.transform.position = position;
            monster.spawned = true;
        }
    }

    public override void Despawn(Monster monster)
    {
        if (monster.tag == "Monster")
        {
            monster.transform.rotation = Quaternion.identity;
            monster.transform.position = Vector3.zero;
            monster.Reset();
            monster.gameObject.SetActive(false);
            Monster temp = monster;
            pool.Remove(monster);
            pool.Insert(pool.Count - 1, temp);
        }
    }
}
