using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private GameStateManager _gameStateManager = null;

    private void Start()
    {
        _inputManager.Initialize();
        _gameStateManager.Initialize();
        GameStateManager.Instance.LaunchTransition(EGameState.MAINMENU);
    }
}
