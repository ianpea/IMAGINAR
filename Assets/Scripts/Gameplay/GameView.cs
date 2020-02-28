using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    private static GameView _instance;
    public static GameView Instance { get { return _instance; } }

    [SerializeField] private GameModel _model = new GameModel();
    public GameModel Model { get { return Instance._model; } }

    private GameController _controller = new GameController();
    public GameController Controller { get { return Instance._controller; } }

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
        Controller.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Controller.Update();
    }

    private void OnApplicationQuit()
    {
        Controller.SaveGame();
    }
}
