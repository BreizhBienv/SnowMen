using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);

        if (GameManager.Instance._currState != GameManager.GameState.Menu)
            GameManager.Instance._currState = GameManager.GameState.Menu;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.UpdateGameState(GameManager.GameState.SelectTeams);
    }

    public void BackCredit()
    {
        SceneManager.LoadScene(2);
    }
    public void ControlPanel()
    {
        SceneManager.LoadScene(4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
