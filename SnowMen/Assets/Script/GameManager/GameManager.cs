using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private int _playerPerTeam = 1;

    private List<GameObject> _blueTeam;
    private List<GameObject> _redTeam;

    [SerializeField] private Material _blueSnowman;
    [SerializeField] private Material _redSnowman;

    private GameObject _blueSpawn;
    private GameObject _redSpawn;

    [Header("Teams Score")]
    [SerializeField] private int _blueScore;
    [SerializeField] private int _redScore;

    [SerializeField] private TextMeshProUGUI _blueScoreText;
    [SerializeField] private TextMeshProUGUI _redScoreText;


    [Header("Scene settings")]
    [SerializeField] private GameObject _inGameUI;
    private bool _startedOnce = false;
    private bool _resetOnce = false;

    public GameState _currState;
    private string m_sceneToWait;

    private ChoosingTeams _choosingTeams;
    private Timer _timer;
    private SpawnPlayers _spawnPlayers;

    public enum GameState
    {
        DEFAULT, //Fall-back state, should never happen
        SelectTeams, //players are choosing there teams
        Start, //set up the game
        Running, //players are playing
        Pause, //game in pause
        Endgame, //game has finished
        OverTime, //if scores are equal at the end of timer
        WaitForSceneToLoad,
        Menu
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _choosingTeams = this.GetComponent<ChoosingTeams>();
        _timer = this.GetComponent<Timer>();
        _spawnPlayers = this.GetComponent<SpawnPlayers>();
        _blueTeam = new List<GameObject>(_playerPerTeam);
        _redTeam = new List<GameObject>(_playerPerTeam);

        UpdateGameState(GameState.Menu);
    }

    private void Update()
    {
        switch (_currState)
        {
            case GameState.WaitForSceneToLoad:
                if (SceneManager.GetActiveScene().name == m_sceneToWait)
                {
                    _blueSpawn = GameObject.Find("BlueSpawn");
                    _redSpawn = GameObject.Find("RedSpawn");

                    AttacheAllHead();
                    InitialisePlayers();
                    _inGameUI.gameObject.SetActive(true);
                    GameOver.Instance.enabled = true;
                    _timer.enabled = true;
                    UpdateGameState(GameState.Running);
                }
                break;

            default:
                break;
        }

        UpdateScoresText();
    }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

        set => _instance = value;
    }

    private void UpdateScoresText()
    {
        if (_currState == GameState.Running)
        {
            _blueScoreText.text = _blueScore.ToString();
            _redScoreText.text = _redScore.ToString();
        }
    }

    public void UpdateGameState(GameState p_newState)
    {
        _currState = p_newState;

        switch (p_newState)
        {

            case GameState.Menu:
                break;

            case GameState.Start:
                HandleStart();
                break;

            case GameState.OverTime:
                HandleOverTime();
                break;

            case GameState.Endgame:
                HandleGameOver();
                break;


            case GameState.SelectTeams:
                HandleSelectTeam();
                break;

            case GameState.Pause:
            case GameState.Running:
            case GameState.WaitForSceneToLoad:
            case GameState.DEFAULT:
            default:
                break;
        }
    }

    private void HandleSelectTeam()
    {
        _choosingTeams.enabled = true;
        _spawnPlayers.enabled = true;

        _resetOnce = false;
    }

    private void HandleStart()
    {
        if (!_startedOnce)
        {
            _startedOnce = true;
            m_sceneToWait = "Arena_01";

            _choosingTeams.enabled = false;

            _currState = GameState.WaitForSceneToLoad;

            LoadingScene.Instance.LoadScene("Arena_01");
        }
    }

    private void HandleOverTime()
    {
        foreach (GameObject player in _blueTeam)
        {
            player.GetComponent<PlayerInfo>().CurrHP = 1;
        }

        foreach (GameObject player in _redTeam)
        {
            player.GetComponent<PlayerInfo>().CurrHP = 1;
        }

        _blueSpawn.SetActive(false);
        _redSpawn.SetActive(false);
    }

    private void HandleGameOver()
    {
        ResetGame();

        if (GameOver.Instance.BlueLoose)
        {
            LoadingScene.Instance.LoadScene("RedWin");
        }
        else if (GameOver.Instance.RedLoose)
        {
            LoadingScene.Instance.LoadScene("BlueWin");
        }
    }

    private void ResetGame()
    {
        if (_resetOnce)
            return;

        _resetOnce = true;

        foreach (GameObject player in _blueTeam)
        {
            Destroy(player.GetComponent<PlayerInfo>().PlayerHead);
            Destroy(player);
        }

        foreach (GameObject player in _redTeam)
        {
            Destroy(player.GetComponent<PlayerInfo>().PlayerHead);
            Destroy(player);
        }

        _blueTeam.Clear();
        _redTeam.Clear();

        _blueScore = 0;
        _redScore = 0;

        Timer.Instance.TimeLeft = Timer.Instance.BaseTime;
        _timer.enabled = false;

        _startedOnce = false;
        _choosingTeams.enabled = false;

        _inGameUI.gameObject.SetActive(false);
    }

    private void InitialisePlayers()
    {
        _spawnPlayers.FirstSpawn(BlueTeam, _blueSpawn, "Blue", _blueSnowman);
        _spawnPlayers.FirstSpawn(RedTeam, _redSpawn, "Red", _redSnowman);
    }

    private void AttacheAllHead()
    {
        foreach (GameObject player in _blueTeam)
        {
            player.GetComponent<HeadControls>().PickUpHead();
        }

        foreach (GameObject player in _redTeam)
        {
            player.GetComponent<HeadControls>().PickUpHead();
        }
    }

    public static List<GameObject> BlueTeam { get => Instance._blueTeam; set => Instance._blueTeam = value; }

    public static List<GameObject> RedTeam { get => Instance._redTeam; set => Instance._redTeam = value; }

    public static bool StartedOnce { get => Instance._startedOnce; }
    public int BlueScore { get => _blueScore; set => _blueScore = value; }
    public int RedScore { get => _redScore; set => _redScore = value; }
    public int PlayerPerTeam { set => _playerPerTeam = value; }
    public GameObject BlueSpawn { get => _blueSpawn; }
    public GameObject RedSpawn { get => _redSpawn; }
}