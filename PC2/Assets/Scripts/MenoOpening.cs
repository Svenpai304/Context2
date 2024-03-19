using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenoOpening : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUIPrefab;
    public GameObject pauseMenuUIInstance;
    public GameObject encyclopediaMenuUIPrefab;
    public GameObject encyclopediaMenuUIInstance;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        PlayerAbilityManager.instance.EnableAll();
        //pauseMenuUI.SetActive(false);
        
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PlayerAbilityManager.instance.DisableAll();
        //pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void PauseMenuInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (pauseMenuUIInstance != null)
            {
                Destroy(pauseMenuUIInstance);
                Resume();
            }
            else
            {
                if (encyclopediaMenuUIInstance == null)
                {
                    pauseMenuUIInstance = Instantiate(pauseMenuUIPrefab, Vector3.zero, Quaternion.identity);
                    Pause();
                }
            }
        }
    }
    public void EncyclopediaMenuInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (encyclopediaMenuUIInstance != null)
            {
                Destroy(encyclopediaMenuUIInstance);
                Resume();
            }
            else
            {
                if (pauseMenuUIInstance == null)
                {
                    encyclopediaMenuUIInstance = Instantiate(encyclopediaMenuUIPrefab, Vector3.zero, Quaternion.identity);
                    Pause();
                }
            }
        }
    }
}