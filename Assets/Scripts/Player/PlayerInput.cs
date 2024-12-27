using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IRequireCleanup
{
    public GameControls input;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += TogglePlayerInput;
        input = new GameControls();
        input.Enable();

        #region Delegates
        input.Pause.Activate.performed += ctx => Pause(ctx);
        GameManager.Instance.OnApplicationCleanup += OnCleanup;
        SceneTransitionManager.Instance.BeforeSceneChange += OnCleanup;
        #endregion
    }

    #region Cleanup
    public void OnDisable()
    {
        if (!GameManager.cleanedUp) OnCleanup();
    }

    public void OnCleanup()
    {
        Debug.Log($"{name}: Unsubscribing in progress...");
        GameManager.Instance.OnGameStateChanged -= TogglePlayerInput;
        GameManager.Instance.OnApplicationCleanup -= OnCleanup;
        SceneTransitionManager.Instance.BeforeSceneChange -= OnCleanup;
        input.Pause.Activate.performed -= ctx => Pause(ctx);
        input.Disable();
    }
    #endregion

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.GameState == GameState.PAUSE)  GameManager.Instance.GameState = GameState.PLAY;
        else GameManager.Instance.GameState = GameState.PAUSE;
    }

    #region Input Activation
    public void TogglePlayerInput()
    {
        if (GameManager.Instance.GameState == GameState.PLAY)
        {
            //Enable
        }
        else
        {
            //Disable
        }
    }

    public void EnablePauseInput()
    {
        input.Pause.Enable();
    }

    public void DisablePauseInput()
    {
        input.Pause.Disable();
    }
    #endregion
}
