﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public OverworldUIManager ui;
    public DialogueManager dm;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(dm.alreadyTalking == null)
                ui.showPause(togglePause());
        }
        
    }

    private bool togglePause()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            return true;
        }
        else
        {
            Time.timeScale = 1f;
            return false;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

}