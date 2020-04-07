using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : Singleton<GameMaster>
{
    #region Pubblica
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

    #endregion
    #region Private
    private float timer;
    private BattleManager battle;
    private GameObject player;
    private List<string> killedEnemies = new List<string>();
    private string fighting = "";
    private string enemyType;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public bool loaded = false;

    private string currentLevel = "Level0";
    
    private string[] partyNames = { "Paladin", "Archer", "Boxer", "Mage" };
    private Stats tempStats;
    #endregion

    private void Start()
    {
        //start timer
        timer = 0;

        //create party
        party = new Dictionary<string, Stats>();

        //add members
        foreach(string st in partyNames)
        {
            addToParty(st);
        }

        //create inventory
        inventory = new Inventory();
        inventory.generateItem(new ItemInfo(new BottledBlessing(), 2));
        inventory.generateItem(new ItemInfo(new BottledMiracle(), 1));
        inventory.generateItem(new ItemInfo(new Potion(), 1));

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
        player = GameObject.Find("Player");
        lastPosition = player.transform.position;
        lastRotation = player.transform.rotation;
        
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
            SceneManager.LoadScene(currentLevel);
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
            case "Level2":
                player = FindObjectOfType<NoJumpController>().gameObject;
                
                if (fighting == "") //first spawn
                {
                    if (!loaded)
                    {
                        lastPosition = player.transform.position;
                        lastRotation = player.transform.rotation;
                    }
                }
                else
                { //return from battle
                    foreach (string enemy in killedEnemies)
                    {
                        GameObject.Find(enemy).SetActive(false);
                    }
                }
                player.transform.SetPositionAndRotation(lastPosition, lastRotation);
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
    #endregion

    #region Party Management
    public void addToParty(string name)
    {
        party.Add(name, new Stats(name));
    }
    public void removeFromParty(string name)
    {
        party.Remove(name);
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
        return save;
    }

    public void LoadManager(string cl, List<string> ke, int g, Dictionary<string, Stats> p,Inventory i, string f, Vector3 pos, Quaternion rot, float timerfloat)
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
        timer = timerfloat; //i may need to initialize this with new first.
    }

    public void resetGame()
    {
        loaded = false;
        currentLevel = "Level0";
        killedEnemies = new List<string>();
        gold = 0;
        party = new Dictionary<string, Stats>();
        foreach(string s in partyNames)
        {
            party.Add(s, new Stats(s));
        }
        inventory = new Inventory();
        fighting = "";
        timer = 0;
    }
    #endregion
}
