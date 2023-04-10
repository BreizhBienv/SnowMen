using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputManagerScript : MonoBehaviour
{
    private int _gamepadCounter = 0;

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerInfo playerInfo = playerInput.GetComponent<PlayerInfo>();

        playerInfo.PLayerID = playerInput.playerIndex + 1;

        // Mark gamepad #_gamepadCounter as being for player [playerindex + 1].
        InputSystem.SetDeviceUsage(playerInput.devices[0], "Player" + _gamepadCounter);
        // And later look it up.
        playerInfo.PlayerGamePad = InputSystem.GetDevice<Gamepad>(new InternedString("Player" + _gamepadCounter));

        _gamepadCounter += 1;

        if (PlayerInputManager.instance.playerCount > 2)
        {
            GameManager.BlueTeam.Capacity = 2;
            GameManager.RedTeam.Capacity = 2;
        }
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        if (PlayerInputManager.instance.playerCount < 3)
        {
            GameManager.BlueTeam.Capacity = 1;
            GameManager.RedTeam.Capacity = 1;
        }
    }
}
