using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenusHolder;
    public MenuSwapper pauseMenuSwapper;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject journalHolder;
    public static bool isPaused
    {
        get
        {
            return Time.timeScale == 0f;
        }
    }

    public void OnPaused(SignalArguments args)
    {
        PauseGame();
        //pauseMenuUI.SetActive(true);
        pauseMenusHolder.SetActive(true);
        pauseMenuSwapper.OpenMenu(args.intArgs[0]); // Makes sure we open to the default "Pause" menu.
    }


    public void PauseGame()
    {

        ServiceLocator.Get<InputManager>().EnablePlayerInput(false);
        Time.timeScale = 0;

        InputManager.playerInputActions.UI.Enable();
        InputManager.playerInputActions.UI.Unpause.performed += OnPauseButtonPressed;
    }

    public void UnpauseGame()
    {
        journalHolder.SetActive(false);

        InputManager.playerInputActions.UI.Unpause.performed -= OnPauseButtonPressed;
        InputManager.playerInputActions.UI.Disable();

        Time.timeScale = 1;
        ServiceLocator.Get<InputManager>().EnablePlayerInput(true);
    }

    private void OnPauseButtonPressed(InputAction.CallbackContext context) // Unpauses the game while in a menu
    {
        // TODO: Close ALL pause menus, or something like that.
        //pauseMenuUI.SetActive(false);
        pauseMenuSwapper.OpenMenu(0);
        pauseMenusHolder.SetActive(false);
        
        UnpauseGame();
    }

    public void QuitToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu_SCN");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        InputManager.playerInputActions.UI.Unpause.performed -= OnPauseButtonPressed;
    }
}
