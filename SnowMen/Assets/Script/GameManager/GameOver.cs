using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private static GameOver _instance;

    private bool _blueLoose = false;
    private bool _redLoose = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public static GameOver Instance
    {
        get
        {
            return _instance;
        }

        set => _instance = value;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (Timer.Instance.TimeLeft <= 0 && GameManager.Instance._currState != GameManager.GameState.Endgame)
        {
            if (GameManager.Instance._currState == GameManager.GameState.OverTime)
            {
                if (IsTeamDead(GameManager.BlueTeam))
                {
                    _blueLoose = true;
                }
                else if (IsTeamDead(GameManager.RedTeam))
                {
                    _redLoose = true;
                }
                else
                    return;

                GameManager.Instance.UpdateGameState(GameManager.GameState.Endgame);
            }
            else if (OverTime())
                GameManager.Instance.UpdateGameState(GameManager.GameState.OverTime);
            else if (GameManager.Instance._currState == GameManager.GameState.Running)
            {
                CheckWinner();
                GameManager.Instance.UpdateGameState(GameManager.GameState.Endgame);
            }
        }
    }

    private bool IsTeamDead(List<GameObject> p_team)
    {
        foreach (GameObject player in p_team)
        {
            PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();

            if (!playerInfo.IsDead)
                return false;
        }

        return true;
    }

    private void CheckWinner()
    {
        if (GameManager.Instance.BlueScore < GameManager.Instance.RedScore)
            _blueLoose = true;
        else if (GameManager.Instance.BlueScore > GameManager.Instance.RedScore)
            _redLoose = true;
    }

    private bool OverTime()
    {
        if (Timer.Instance.TimeLeft <= 0 && GameManager.Instance.BlueScore == GameManager.Instance.RedScore
            && GameManager.Instance._currState == GameManager.GameState.Running)
        {
            return true;
        }

        return false;
    }

    public bool BlueLoose { get => _blueLoose; }
    public bool RedLoose { get => _redLoose; }
}
