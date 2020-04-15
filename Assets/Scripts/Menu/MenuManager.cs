using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject loadPanel;
    public GameObject creditsPanel;
    public GameObject settingsPanel;
    public AudioMixer mixer;

    private SettingsManager sm;
    enum Panel
    {
        None = -1,
        Load = 0,
        Credits = 1,
        Settings = 2
    }
    Panel currentPanel = Panel.None;

    private void Start() //remember this is shared between gameover and main menu
    {
        UI.showCursor(true);
        //set music menu
        mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 0.75f)) * 20);
        mixer.SetFloat("SfxVolume", Mathf.Log10(PlayerPrefs.GetFloat("SfxVolume", 0.75f)) * 20);

        GameMaster.Instance.resetGame();
        if (settingsPanel)
        {
            sm = settingsPanel.GetComponent<SettingsManager>();
            settingsPanel.SetActive(false); //TODO: start with settings panel open or set mixer volume in another way
        }
    }

    #region Menu buttons
    public void NewGame()
    {
        SceneManager.LoadScene("Level0");
    }

    public void LoadGame()
    {
        if(currentPanel == Panel.Load)
        {
            hideAllPanels();
            currentPanel = Panel.None;
        } else
        {
            currentPanel = Panel.Load;
            Button btn;
            //Fill load slots
            for (int i = 0; i < loadPanel.transform.childCount; i++)
            {
                btn = loadPanel.transform.GetChild(i).GetComponent<Button>();
                UI.setSLButtonText(btn, i, true);
            }
            loadPanel.SetActive(true);
        }
    }

    public void Credits()
    {
        if (currentPanel == Panel.Credits)
        {
            hideAllPanels();
            currentPanel = Panel.None;
        }
        else
        {
            currentPanel = Panel.Credits;
            creditsPanel.SetActive(true);
        }
    }

    public void Settings()
    {
        if (currentPanel == Panel.Settings)
        {
            hideAllPanels();
            currentPanel = Panel.None;
        }
        else
        {
            currentPanel = Panel.Settings;
            settingsPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
    
    #region GameOver Button
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion

    private void hideAllPanels()
    {
        loadPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
