﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : Singleton<GameMaster>
{
    #region Public
    private Dictionary<string, Stats> party;
    public List<Stats> Party
    {
        get
        {
            List<Stats> l = new List<Stats>();
            foreach(KeyValuePair<string, Stats> kv in party)
            {
                l.Add(kv.Value);
            }
            return l;
        }
    }

    [Header("Inventory")]
    public int gold = 0;
    public Inventory inventory;
    public int currentLevel = 0;
    #endregion
    #region Private
    private float timer;
    private BattleManager battle;
    private Transform player;
    private List<string> killedEnemies = new List<string>();
    private string fighting = "";
    private bool loaded = false;
    private string enemyType;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    
    private string[] partyNames = { "Paladin", "Archer", "Boxer", "Mage" };
    private List<string> bossNames = new List<string>{ "Brady", "Ganfaul" };
    private Stats tempStats;
    #endregion

    private void Start()
    {
        //start timer
        timer = 0;

        initParty(partyNames);

        //create inventory
        inventory = new Inventory();
        inventory.generateItem(new ItemInfo(new BottledBlessing(), 1));

        SceneManager.sceneLoaded += sceneChanged;
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }

    #region InizioBattaglia
    public void EnterBattle(string enemyName, string enemyType)
    {
        fighting = enemyName;
        this.enemyType = enemyType;
        player = FindObjectOfType<NoJumpController>().transform;
        lastPosition = player.position;
        lastRotation = player.rotation;
        
        SceneManager.LoadScene("Battle");
    }
    #endregion

    #region FineBattaglia
    public void EndBattle(bool didYouWin)
    {
        if (didYouWin) //Won Battle
        {
            battle = FindObjectOfType<BattleManager>();

            //save party stats for later battles
            foreach (Fighter f in battle.fighters)
            {
                if(party.TryGetValue(f.ToString(), out tempStats)) //save the reference to party's stats in tempStats
                {
                    tempStats.HP = f.stats.HP;
                    tempStats.MP = f.stats.MP;
                }
            } 
            gold += battle.enemyGold;
            inventory.addInventory(battle.enemyItems);

            killedEnemies.Add(fighting);

            if(bossNames.Contains(fighting)){
                currentLevel++;
                killedEnemies = new List<string>();
            }
            Debug.Log("battle ended. fighting=" + fighting + ", currentLevel: " + currentLevel + ", loaded:" + loaded);
            Debug.Log("killed enemies:");
            foreach (string str in killedEnemies)
            {
                Debug.Log(str);
            }
            SceneManager.LoadScene("Level" + currentLevel);
        } else //Lost Battle
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    #endregion

    #region Cambio scena (evento)
    private void sceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        switch (arg0.name)
        {
            #region Inizio battaglia
            case "Battle":
                break;
            #endregion

            #region Battaglia vinta o ritorno all'overworld
            case "Level0":
            case "Level1":
                player = FindObjectOfType<NoJumpController>().transform;

                if(loaded || fighting != ""){
                    player.SetPositionAndRotation(lastPosition, lastRotation);
                    loaded = false;
                }
                foreach (string enemy in killedEnemies)
                {
                    GameObject.Find(enemy).SetActive(false);
                }
                break;
            #endregion

            #region Battaglia persa
            case "Menu":

                break;
            case "GameOver":

                break;
            #endregion

            #region Altro
            default:
                break;
                #endregion
        }
    }
    #endregion

    #region Getters, setters
    public string GetEnemyType()
    {
        return enemyType;
    }

    public string GetPlayTime(){
        return UI.getPlayTimeString(timer);
    }
    #endregion

    #region Party Management
    private void initParty(string[] names){
        party = new Dictionary<string, Stats>();
        foreach(string s in names)
        {
            addToParty(s);
        }
    }
    public bool addToParty(string name)
    {
        if(!party.ContainsKey(name)){
            party.Add(name, new Stats(name));
            return true;
        }
        return false;
    }
    public bool removeFromParty(string name)
    {
        if(party.ContainsKey(name)){
            party.Remove(name);
        return true;
        }
        return false;
    }
    #endregion

    #region Save and Load
    public Save CreateSaveGameObject()
    {
        Transform t = FindObjectOfType<NoJumpController>().transform; //Find player
        Save save = new Save();
        save.timestamp = DateTime.Now;

        save.currentLevel = currentLevel;
        save.enemiesKilled = new List<string>(killedEnemies);
        save.gold = gold;
        save.party = new Dictionary<string, Stats>(party);
        save.inventory = new List<ItemInfo>(inventory.getContents());
        save.fighting = fighting;
        save.posx = t.position.x;
        save.posy = t.position.y;
        save.posz = t.position.z;
        save.rot0 = t.rotation.x;
        save.rot1 = t.rotation.y;
        save.rot2 = t.rotation.z;
        save.rot3 = t.rotation.w;
        save.timerfloat = timer;
        //Debug.Log("saving scene Level" + currentLevel + ", fighting: " + fighting);
        return save;
    }

    public void LoadManager(int cl, List<string> ke, int g, Dictionary<string, Stats> p, Inventory i, string f, Vector3 pos, Quaternion rot, float timerfloat)
    {
        loaded = true;
        currentLevel = cl;
        killedEnemies = ke;
        gold = g;
        party = p;
        inventory = i;
        fighting = f;
        lastPosition = pos;
        lastRotation = rot;
        timer = timerfloat;
        //Debug.Log("loading scene Level" + currentLevel + ", fighting: " + fighting);
    }

    public void resetGame()
    {
        currentLevel = 0;
        killedEnemies = new List<string>();
        gold = 0;
        initParty(partyNames);
        inventory = new Inventory();
        fighting = "";
        timer = 0;
    }
    #endregion
}
