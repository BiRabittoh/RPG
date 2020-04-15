using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : NPC
{
    [SerializeField] private string bossType = null;

    public override void actionAfterDialogue(){
        GameMaster.Instance.EnterBattle(name, bossType);
    }
}
