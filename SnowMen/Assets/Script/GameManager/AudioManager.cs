using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _windInTree;
    private AudioSource _menuTheme;
    private AudioSource _victoryTheme;
    private AudioSource _OverTime;
    private AudioSource _gameTheme;

    // Start is called before the first frame update
    void Start()
    {
        _windInTree = this.transform.Find("TreeSound").GetComponent<AudioSource>();
        _menuTheme = this.transform.Find("MenuTheme").GetComponent<AudioSource>();
        _victoryTheme = this.transform.Find("VictoryTheme").GetComponent<AudioSource>();
        _OverTime = this.transform.Find("OverTime").GetComponent<AudioSource>();
        _gameTheme = this.transform.Find("GameTheme").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance._currState == GameManager.GameState.Running)
        {
            if (!_windInTree.isPlaying)
                _windInTree.Play();

            if (!_gameTheme.isPlaying)
                _gameTheme.Play();
        }
        else
        {
            if (_windInTree.isPlaying)
                _windInTree.Stop();

            if (_gameTheme.isPlaying)
                _gameTheme.Stop();
        }

        if (GameManager.Instance._currState == GameManager.GameState.Menu)
        {
            if (!_menuTheme.isPlaying)
                _menuTheme.Play();
        }
        else
        {
            if (_menuTheme.isPlaying)
                _menuTheme.Stop();
        }

        if (GameManager.Instance._currState == GameManager.GameState.OverTime)
        {
            if (!_OverTime.isPlaying)
                _OverTime.Play();
        }
        else
        {
            if (_OverTime.isPlaying)
                _OverTime.Stop();
        }

        if (GameManager.Instance._currState == GameManager.GameState.Endgame)
        {
            if (!_victoryTheme.isPlaying)
                _victoryTheme.Play();
        }
        else
        {
            if (_victoryTheme.isPlaying)
                _victoryTheme.Stop();
        }
    }
}
