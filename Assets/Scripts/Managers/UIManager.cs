using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private Coroutine transformCo;
    [SerializeField] private bool _isTransforming = false;
    public bool isTransforming
    {
        get { return _isTransforming; }
        set { _isTransforming = value; }
    }

    public const float chargeTime = 1.0f;

    public bool isPointerEnter = false;
    public bool isPointerExit = false;

    /// <summary>
    /// Fading image coroutine.
    /// </summary>
    public Coroutine fadeCo;
    public Image fadeImg;

    // UI buttons
    public Button transformBtn;
    public Image transformImg;
    public Button volumeBtn;
    public Button mainMenuBtn;
    public Button laserBtn;

    // Images for UI, volume buttons, unit frame on top left , player image and pet image and etc.
    public Image playerImg;
    public Image petImg;
    public Image volumeOn;
    public Image volumeOff;
    public Image collectImg;
    public Image attackImg;
    public Image crosshair;
    public Image unitFrame;

    public TMPro.TextMeshProUGUI pauseButton;
    public TMPro.TextMeshProUGUI scoreTMP;
    public TMPro.TextMeshProUGUI totalScoreTMP;
    public TMPro.TextMeshProUGUI imaginationPointsTMP;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameState();
    }

    public void UpdateGameState()
    {
        GAME_STATE gameState = GameView.Instance.Model.currentState;
        if (gameState == GAME_STATE.Player)
        {
            playerImg.gameObject.SetActive(true);
            petImg.gameObject.SetActive(false);
            collectImg.gameObject.SetActive(true);
            attackImg.gameObject.SetActive(false);
            laserBtn.gameObject.SetActive(false);
            unitFrame.color = Color.green;
        }
        else if(gameState == GAME_STATE.Pet)
        {
            playerImg.gameObject.SetActive(false);
            petImg.gameObject.SetActive(true);
            collectImg.gameObject.SetActive(false);
            attackImg.gameObject.SetActive(true);
            laserBtn.gameObject.SetActive(true);
            unitFrame.color = Color.magenta;
        }
    }

    public void StartTransform()
    {
        transformCo = StartCoroutine(Transform());
    }

    private IEnumerator Transform()
    {
        isTransforming = true;
        AudioManager.Instance.SFXTransform();
        for(float t = 0.0f; t < chargeTime; t += Time.deltaTime)
        {
            if (transformImg.fillAmount > 0.0f)
            {
                transformImg.fillAmount -= Time.deltaTime/chargeTime;
            }
            if(Mathf.Approximately(transformImg.fillAmount, 0.0f)) // If the fillAmount is close to zero, reset it.
            {
                GameView.Instance.Controller.SwitchState();
                CancelTransform();
                break;
            }
            yield return null;
        }
        isTransforming = false;
    }

    public void CancelTransform()
    {
        StopCoroutine(transformCo);
        isTransforming = false;
        transformImg.fillAmount = 1.0f;
        AudioManager.Instance.Transform.Stop();
    }

    public void Pause()
    {
        AudioManager.Instance.SFXButtonPressed();
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
            totalScoreTMP.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(false);
            volumeBtn.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            totalScoreTMP.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(true);
            volumeBtn.gameObject.SetActive(false);
            mainMenuBtn.gameObject.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.SFXButtonPressed();
        Time.timeScale = 1.0f;
        StartCoroutine(Fade.FadeTo(1.0f, 1.5f, fadeImg));
        GameView.Instance.Controller.SaveGame();
        Invoke("LoadMainMenu", 1.5f);
    }

    /// <summary>
    /// Used for Invoke, after screen fades.
    /// </summary>
    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
