using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletPool : MonoBehaviour
{
    [SerializeField] public GameObject bulletPlayerPrefab;
    [SerializeField] public GameObject bulletPetPrefab;

    public List<GameObject> bulletAllyPool;
    public List<GameObject> bulletPetPool;
    public const int MAX_BULLET = 35;
    public const int ENEMY_MAX_BULLET = 35;
    private const float PET_BULLET_SPEED = 50f;
    private const float ALLY_BULLET_SPEED = 10f;
    private Camera cam;

    private static BulletPool _instance;
    public static BulletPool Instance { get { return _instance; } }

    private void Awake()
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
        InstantiateBullets();
        cam = ARCoreView.Instance.Model.FirstPersonCamera;
    }

    void Update()
    {
        UpdateBullets();
    }

    private void InstantiateBullets()
    {
        bulletAllyPool = new List<GameObject>(MAX_BULLET);
        for (int i = 0; i < MAX_BULLET; i++)
        {
            GameObject bullet = Instantiate(bulletPlayerPrefab, this.transform);
            bullet.SetActive(false);
            bulletAllyPool.Add(bullet);
        }
        bulletPetPool = new List<GameObject>(MAX_BULLET);
        for (int i = 0; i < MAX_BULLET; i++)
        {
            GameObject bullet = Instantiate(bulletPetPrefab, this.transform);
            bullet.SetActive(false);
            bulletPetPool.Add(bullet);
        }
    }

    /// <summary>
    /// Fire bullet function for pet.
    /// </summary>
    /// <param name="cam"></param>
    public void BulletFire(Camera cam)
    {
        GameObject bullet = _instance.GetBullet();
        if (!bullet.activeInHierarchy)
        {
            AudioManager.Instance.SFXRoar();
            bullet.SetActive(true);
            bullet.transform.position = cam.transform.position;
            bullet.transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x + 85, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
            bullet.GetComponent<Rigidbody>().velocity = (cam.transform.forward * PET_BULLET_SPEED);
        }
        else
        {
            Debug.LogError("Bullet already active!");
        }
    }

    /// <summary>
    /// Fire bullet function for ally.
    /// </summary>
    /// <param name="attackDir"></param>
    public void BulletFire(Vector3 attackDir, GameObject ally)
    {
        GameObject bullet = _instance.GetAllyBullet();
        if (!bullet.activeInHierarchy)
        {
            bullet.SetActive(true);
            bullet.transform.position = ally.transform.position;
            bullet.GetComponent<Rigidbody>().velocity = (attackDir * ALLY_BULLET_SPEED);
        }
        else
        {
            Debug.LogError("Bullet already active!");
        }
    }

    private GameObject GetBullet()
    {
        for (int i = 0; i < MAX_BULLET; i++)
        {
            if (!bulletPetPool[i].activeInHierarchy)
            {
                return bulletPetPool[i];
            }
            else
            {
                continue;
            }
        }
        Debug.LogWarning("Bullet pool maxed out. Returning NULL.");
        return null;
    }

    private GameObject GetAllyBullet()
    {

        for (int i = 0; i < MAX_BULLET; i++)
        {
            if (!bulletAllyPool[i].activeInHierarchy)
            {
                return bulletAllyPool[i];
            }
            else
            {
                continue;
            }
        }
        Debug.LogWarning("Bullet pool maxed out. Returning NULL.");
        return null;
    }

    private void UpdateBullets()
    {
        for (int i = 0; i < MAX_BULLET; i++)
        {
            if (bulletAllyPool[i] == null) continue;
            if ((bulletAllyPool[i].transform.position - cam.transform.position).magnitude > 250)
            {
                DespawnBullet(bulletAllyPool[i]);
            }
        }
        for (int i = 0; i < MAX_BULLET; i++)
        {
            if (bulletPetPool[i] == null) continue;
            if ((bulletPetPool[i].transform.position - cam.transform.position).magnitude > 250)
            {
                DespawnBullet(bulletPetPool[i]);
            }
        }
    }

    public void DespawnBullet(GameObject bullet)
    {
        bullet.transform.rotation = Quaternion.identity;
        bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        bullet.SetActive(false);
    }

}
