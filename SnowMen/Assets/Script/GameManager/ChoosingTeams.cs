using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class ChoosingTeams : MonoBehaviour
{
    //time to wait after both teams have complete to start game in milliseconds
    [SerializeField] private int _timeToWait;

    private bool _isBlueTeamFull = false;
    private bool _isRedTeamFull = false;

    IEnumerator TimerRoutine = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //test if both teams have required player to start game
        AreTeamsComplete();

        //if both team are full, start counting necessary time before starting the game
        CanGameStart();
    }

    private void AreTeamsComplete()
    {
        if (GameManager.BlueTeam.Count == GameManager.BlueTeam.Capacity)
            _isBlueTeamFull = true;
        else
            _isBlueTeamFull = false;

        if (GameManager.RedTeam.Count == GameManager.RedTeam.Capacity)
            _isRedTeamFull = true;
        else
            _isRedTeamFull = false;
    }

    private void CanGameStart()
    {
        if (TimerRoutine != null)
            return;

        if (_isBlueTeamFull && _isRedTeamFull &&
            PlayerInputManager.instance.playerCount != 3 &&
            GameManager.Instance._currState == GameManager.GameState.SelectTeams)
        {
            TimerRoutine = CheckTimer(_timeToWait);
            StartCoroutine(TimerRoutine);
        }
    }

    private IEnumerator CheckTimer(float p_time)
    {
        yield return new WaitForSeconds(p_time);

        if (_isBlueTeamFull && _isRedTeamFull && PlayerInputManager.instance.playerCount != 3)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
        }

        TimerRoutine = null;

        yield return null;
    }
}
