using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    PLAY,
    PAUSE,
    WIN,
    LOSE,
    MAINMENU,
}

public class GameManager : Singleton<GameManager>
{
    #region Properties
    private GameState gameState = GameState.MAINMENU;
    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            if (gameState == value) return;
            gameState = value;
            OnGameStateChanged?.Invoke();
        }
    }
    #endregion

    #region Cleanup
    static public bool cleanedUp = false;
    public event Action OnApplicationCleanup;

    private void OnApplicationQuit()
    {
        OnApplicationCleanup?.Invoke();
        cleanedUp = true;
    }
    #endregion

    public event Action OnGameStateChanged;

    protected override void Awake()
    {
        base.Awake();
        OnGameStateChanged += DetermineCursorState;
        OnGameStateChanged += SetTimeScale;
    }

    #region GameStateChangeEffects
    private void SetTimeScale()
    {
        if (GameState != GameState.PAUSE) Time.timeScale = 1f;
    }

    private void DetermineCursorState()
    {
        if (gameState == GameState.PLAY)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    #endregion
}