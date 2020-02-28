using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private static Laser _instance;
    public static Laser Instance { get { return _instance; } }

    private Camera cam;

    public LineRenderer laserBeam;
    public bool isLaser = false;
    public bool isLaserStarted = false;
    public float lastTick = 0.0f;
    public float laserConsumeManaCooldown = 1.0f;
    /// <summary>
    /// Offset to allow laser to be seen by camera.
    /// </summary>
    public Vector3 laserOffset = new Vector3(0, -0.1f, 0);


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

    // Start is called before the first frame update
    void Start()
    {
        laserBeam = GetComponent<LineRenderer>();
        cam = ARCoreView.Instance.Model.FirstPersonCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaser || ScoreManager.Instance.sessionImaginationPoints == 0)
        {
            AudioManager.Instance.SFXError();
            laserBeam.enabled = false;
            return;
        }
        else
        {
            if (!isLaserStarted)
            {
                AudioManager.Instance.SFXLaserStart();
                laserBeam.enabled = true;
                isLaserStarted = true;
            }
        }

        // deduct imagination points while using laser.
        if (Time.time >= lastTick + laserConsumeManaCooldown)
        {
            ScoreManager.Instance.Deduct(LOSE.PetLaser);
            lastTick = Time.time;
        }
        // Laser beam start position always at player position (ARCore position).
        laserBeam.transform.position = ARCoreView.Instance.transform.position + laserOffset;
        laserBeam.SetPosition(1, cam.transform.forward * 1500);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Monster")
            {
                hit.collider.gameObject.GetComponent<Monster>().hp -= 4;
            }
            else if(hit.collider.gameObject.tag == "Boss")
            {
                hit.collider.gameObject.GetComponent<Boss>().hp -= 7;
                AudioManager.Instance.SFXBossDamaged();
            }
        }
        else
        {
        }
        AudioManager.Instance.SFXLaser();
    }

    public void ShootLaser()
    {
        isLaser = true;
        gameObject.SetActive(true);
    }

    public void CancelLaser()
    {
        isLaser = false;
        isLaserStarted = false;
        AudioManager.Instance.Laser.Stop();
        gameObject.SetActive(false);
    }
}
