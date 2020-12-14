using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleARCore;

public class MenuManager : Singleton<MenuManager>
{
    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }

    // start game
    private Coroutine startGameCo;
    [SerializeField] private bool _isStarting = false;
    public bool isStarting
    {
        get { return _isStarting; }
        set { _isStarting = value; }
    }

    // quit game
    private Coroutine quitGameCo;
    [SerializeField] private bool _isQuitting = false;
    public bool isQuitting
    {
        get { return _isQuitting; }
        set { _isQuitting = value; }
    }

    public const float timeToPress = 1.0f;

    // UI elements
    public Button startBtn;
    public Image startBtnImg;
    public Image fadeImg;
    public TMPro.TextMeshProUGUI startBtnTMP;

    public Button quitBtn;
    public Image quitBtnImg;
    public TMPro.TextMeshProUGUI quitBtnTMP;

    public TMPro.TextMeshProUGUI scoreTxt;
    public TMPro.TextMeshProUGUI imaginationPointsTxt;

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
        LoadProgress();
    }

    public void StartGameInstant()
    {
        AudioManager.Instance.SFXButtonPressed();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Fade.FadeTo(1.0f, 2.0f, fadeImg);
    }

    public void QuitGameInstant()
    {
        AudioManager.Instance.SFXButtonPressed();
        Application.Quit();
    }

    public void StartGame()
    {
        if (startGameCo != null)
            StopCoroutine(startGameCo);
        startGameCo = StartCoroutine(StartGameCo());
    }

    public void QuitGame()
    {
        if (quitGameCo != null)
            StopCoroutine(quitGameCo);
        quitGameCo = StartCoroutine(QuitGameCo());
    }

    public IEnumerator StartGameCo()
    {
        isStarting = true;
        for(float t = 0; t < timeToPress; t+= Time.deltaTime)
        {
            if (startBtnImg.fillAmount > 0.0f)
            {
                startBtnImg.fillAmount -= Time.deltaTime / timeToPress;
            }
            if (Mathf.Approximately(startBtnImg.fillAmount, 0.0f)) // If the fillAmount is close to zero, reset it.
            {
                StartCoroutine(Fade.FadeTo(1.0f, 2.0f, fadeImg));
                CancelStartGame();
                AudioManager.Instance.SFXButtonPressed();
                Invoke("LoadGame", 2.0f);
                break;
            }
            yield return null;
        }
        isStarting = false;
    }

    /// <summary>
    /// Used for Invoke, after screen fades.
    /// </summary>
    private void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator QuitGameCo()
    {
        isQuitting = true;
        for (float t = 0; t < timeToPress; t += Time.deltaTime)
        {
            if (quitBtnImg.fillAmount > 0.0f)
            {
                quitBtnImg.fillAmount -= Time.deltaTime / timeToPress;
            }
            if (Mathf.Approximately(quitBtnImg.fillAmount, 0.0f)) // If the fillAmount is close to zero, reset it.
            {
                AudioManager.Instance.SFXButtonPressed();
                Application.Quit();
                CancelQuitGame();
                break;
            }
            yield return null;
        }
        isQuitting = false;
        yield return null;
    }

    public void CancelQuitGame()
    {
        StopCoroutine(quitGameCo);
        isQuitting = false;
        quitBtnImg.fillAmount = 1.0f;
    }

    public void CancelStartGame()
    {
        StopCoroutine(startGameCo);
        isStarting = false;
        startBtnImg.fillAmount = 1.0f;
    }

    private void LoadProgress()
    {
        int score = PlayerPrefs.GetInt("Score");
        int ip = PlayerPrefs.GetInt("ImaginationPoints");

        scoreTxt.text = score.ToString(); ;
        imaginationPointsTxt.text = ip.ToString();
    }
}

