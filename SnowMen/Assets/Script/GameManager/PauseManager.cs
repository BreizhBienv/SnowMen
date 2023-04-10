using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    //private GameObject _pauseMenu;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Pause(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.started)
            {
                GameObject canvas = GameObject.Find("TrueCanvas");
                Transform MenuPause = canvas.transform.Find("Pause");

                GameManager.GameState Current = GameManager.Instance._currState;

                if (Current ==  GameManager.GameState.Pause)
                {
                    if (SceneManager.GetActiveScene().name == "ChooseTeamScene")
                        GameManager.Instance._currState = GameManager.GameState.SelectTeams;
                    else
                        GameManager.Instance._currState = GameManager.GameState.Running;

                    Time.timeScale = 1;
                    MenuPause.gameObject.SetActive(false);
                }
                else if (Current != GameManager.GameState.Menu)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.Pause);
                    Time.timeScale = 0;
                    MenuPause.gameObject.SetActive(true);
                }
            }
        }
    }
}
