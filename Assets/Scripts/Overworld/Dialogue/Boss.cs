using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : NPC
{
    [SerializeField] private string bossType = null;

    void onDisable(){
        SceneManager.LoadScene(++GameMaster.Instance.currentLevel);
    }

    public override void actionAfterDialogue(){
        Debug.Log("basik finocchio");
        GameMaster.Instance.EnterBattle(name, bossType);
    }
}
