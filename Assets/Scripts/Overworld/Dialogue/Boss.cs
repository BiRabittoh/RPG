using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : NPC
{
    [SerializeField] private string bossType;

    void onDisable(){
        SceneManager.LoadScene(++GameMaster.Instance.currentLevel);
    }
}
