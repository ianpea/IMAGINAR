using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioSource BGMPlayer;
    public AudioSource BGMPet;
    public AudioSource Roar; // attack sound for pet
    public AudioSource Teleport;
    public AudioSource Collect;
    public AudioSource Transform;
    public AudioSource ShootPlant;
    public AudioSource DeathMonster;
    public AudioSource DeathAnimal;
    public AudioSource ButtonPressed;
    public AudioSource LaserStart;
    public AudioSource Laser;
    public AudioSource Error;
    public AudioSource Damaged;
    public AudioSource BossDamaged;
    public AudioSource BossSpawned;

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

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateBGM();
    }

    public void UpdateBGM()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        GAME_STATE state = GameView.Instance.Model.currentState;
        if (state == GAME_STATE.Player)
        {
            SFXBGMPlayer();
        }
        else if (state == GAME_STATE.Pet)
        {
            SFXBGMPet();
        }
    }

    public void Mute()
    {
        SFXButtonPressed();
        AudioListener.pause = !AudioListener.pause;
        if (AudioListener.pause)
        {
            UIManager.Instance.volumeOff.gameObject.SetActive(true);
            UIManager.Instance.volumeOn.gameObject.SetActive(false);
        }
        else
        {
            UIManager.Instance.volumeOff.gameObject.SetActive(false);
            UIManager.Instance.volumeOn.gameObject.SetActive(true);
        }
    }

    public void SFXBGMPlayer()
    {
        if (!BGMPlayer.isPlaying)
        {
            BGMPet.Stop();
            BGMPlayer.Play();
        }
    }

    public void SFXBGMPet()
    {
        if (!BGMPet.isPlaying)
        {
            BGMPlayer.Stop();
            BGMPet.Play();
        }
    }

    public void SFXTransform()
    {
        if (!Transform.isPlaying)
        {
            Transform.Play();
        }
    }

    public void SFXRoar()
    {
        Roar.PlayOneShot(Roar.clip);

    }

    public void SFXCollect()
    {
        Collect.PlayOneShot(Collect.clip);
    }

    public void SFXDeathAnimal()
    {
        DeathAnimal.PlayOneShot(DeathAnimal.clip);
    }

    public void SFXDeathMonster()
    {
        DeathMonster.PlayOneShot(DeathMonster.clip);
    }

    public void SFXShootPlant()
    {
        ShootPlant.PlayOneShot(ShootPlant.clip);
    }

    public void SFXButtonPressed()
    {
        ButtonPressed.PlayOneShot(ButtonPressed.clip);
    }

    public void SFXLaserStart()
    {
        if(!LaserStart.isPlaying)
            LaserStart.Play();
    }

    public void SFXLaser()
    {
        if(!Laser.isPlaying)
            Laser.Play();
    }

    public void SFXError()
    {
        if(!Error.isPlaying)
            Error.Play();
    }

    public void SFXDamaged()
    {
        Damaged.PlayOneShot(Damaged.clip);
    }

    public void SFXBossDamaged()
    {
        if (!BossDamaged.isPlaying)
            BossDamaged.Play();
    }

    public void SFXBossSpawned()
    {
        BossSpawned.PlayOneShot(BossSpawned.clip);
    }
}
