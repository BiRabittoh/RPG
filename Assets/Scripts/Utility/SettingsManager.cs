using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    GameMaster gm;
    public OverworldUIManager ui;
    public Slider musicSlider, sfxSlider;
    public Slider textSlider, battleSlider;
    public AudioMixer mixer;
    private new AudioSource audio;
    [SerializeField] AudioClip sfxTest = null;

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSfxVolume(float sliderValue)
    {
        if(sfxTest && !audio.isPlaying)
            audio.PlayOneShot(sfxTest);
        mixer.SetFloat("SfxVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SfxVolume", sliderValue);
    }

    public void SetTextSpeed(float sliderValue)
    {
        UI.TextSpeed = sliderValue;
        PlayerPrefs.SetFloat("TextSpeed", sliderValue);
    }

    public void SetBattleSpeed(float sliderValue)
    {
        UI.BattleSpeed = sliderValue;
        PlayerPrefs.SetFloat("BattleSpeed", sliderValue);
    }

    public void Start()
    {
        gm = GameMaster.Instance;
        audio = GetComponent<AudioSource>();
        //restore playerprefs
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        textSlider.value = PlayerPrefs.GetFloat("TextSpeed", 0.015f);
        battleSlider.value = PlayerPrefs.GetFloat("BattleSpeed", 1f);
    }

    #region Resolutions
    public void GoHD()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreen);
    }
    public void GoFullHD()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
    public void Go2K()
    {
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
    }
    #endregion

    #region Save and load
    public void SaveGame(int slot)
    {
        gm = GameMaster.Instance;
        Save save = gm.CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game" + slot + ".save");
        bf.Serialize(file, save);
        file.Close();

        //Debug.Log("Game saved in " + Application.persistentDataPath + "/game" + slot + ".save");

        ui.hideAllPanels();
        ui.currentPanel = 0;
    }
    public void LoadGame(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/game" + slot + ".save"))
        {
            gm = GameMaster.Instance;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game" + slot + ".save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            Vector3 lastPosition = new Vector3(save.posx, save.posy, save.posz);
            Quaternion lastRotation = new Quaternion(save.rot0, save.rot1, save.rot2, save.rot3);
            gm.LoadManager(save.currentLevel, new List<string>(save.enemiesKilled), save.gold,
                new Dictionary<string, Stats>(save.party), new Inventory(save.inventory),
                save.fighting, lastPosition, lastRotation, save.timerfloat);

            Time.timeScale = 1f;
            SceneManager.LoadScene("Level" + save.currentLevel);
        } else
        {
            Debug.Log("No file found.");
        }
    }
    #endregion
}
