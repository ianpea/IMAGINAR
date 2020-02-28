using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AnimalPool : Pool<Animal>
{
    /// <summary>
    /// Singleton.
    /// </summary>
    private static AnimalPool _instance;
    public static AnimalPool Instance { get { return _instance; } }

    private List<Animal> animalPool;

    void Awake()
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
    
    void Start()
    {
        InstantiatePool();
    }

    void Update()
    {
        UpdatePool();
    }

    public override void InstantiatePool ()
    {
        _instance.animalPool = new List<Animal>(MAX_OBJECTS);
        _instance.pool = _instance.animalPool;
        for(int i = 0; i < MAX_OBJECTS; i++)
        {
            Animal animal = Instantiate(_instance.prefab[Random.Range(0, _instance.prefab.Count)], this.transform).GetComponent<Animal>();
            animal.gameObject.SetActive(false);
            _instance.pool.Add(animal);
        }
    }

    public override Animal GetObject()
    {
        for(int i = 0; i < MAX_OBJECTS; i++)
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
        for(int i = 0; i < MAX_OBJECTS; i++)
        {
            if((_instance.pool[i].transform.position - ARCoreView.Instance.transform.position).magnitude > MAX_RANGE_FROM_PLAYER)
            {
                Despawn(_instance.pool[i]);
            }
        }
    }

    public override void Spawn(Vector3 position)
    {
        Animal animal = GetObject();
       
        if (animal == null)
        {
            return;
        }
        if (!animal.gameObject.activeInHierarchy)
        {
            animal.gameObject.SetActive(true);
            animal.transform.position = position;
            animal.spawned = true;
        }
    }

    public override void Despawn(Animal animal)
    {
        if(animal.tag == "Animal")
        {
            animal.transform.rotation = Quaternion.identity;
            animal.transform.position = Vector3.zero;
            animal.Reset();
            animal.gameObject.SetActive(false);
            Animal temp = animal;
            pool.Remove(animal);
            pool.Insert(pool.Count - 1, temp);
        }
    }
}
