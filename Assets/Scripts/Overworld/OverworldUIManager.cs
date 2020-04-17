using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OverworldUIManager : UIManager
{
    //info display
    private GameMaster gm;
    private SettingsManager sm;
    private int i = 0;
    private GoodGuy currentSource, currentTarget;
    private Action currentAction;

    public int currentPanel = 0;

    //inventory info
    [Header("Set stuff here")]
    [SerializeField] private GameObject partyObject = null;
    [SerializeField] private List<GoodGuy> party;
    [SerializeField] private GameObject pause_panel = null;
    [SerializeField] private GameObject[] panel = null;
    [SerializeField] private Text goldText = null;
    [SerializeField] private AudioManager audioManager = null;

    public Text desc_text = null;
    public enum Panel
    {
        Main = 0,
        Inventory = 1,
        Abilities = 2,
        Status = 3,
        Save = 4,
        Load = 5,
        Settings = 6,
        Quit = 7,
        Description = 8
    }

    public void showPause(bool onOff)
    {
        hideAllPanels();
        if (onOff)
        {
            audioManager.playButtonSound(3);
            UI.showCursor(true);
            pause_panel.SetActive(true);
        } else
        {
            UI.showCursor(false);
            currentPanel = 0;
            pause_panel.SetActive(false);
        }
    }
    void Start()
    {
        gm = GameMaster.Instance;
        goldText.text = gm.gold + "g";
        sm = panel[(int)Panel.Settings].GetComponent<SettingsManager>();
        desc_text = panel[(int)Panel.Description].GetComponentInChildren<Text>();

        //set party
        int i = 0;
        GoodGuy g;
        party = new List<GoodGuy>();
        foreach(Stats s in gm.Party)
        {
            g = AbilityDB.GetGoodGuy(partyObject, s);
            g.inBattle = false;
            g.stats = gm.Party[i];
            party.Add(g);
            i++;
        }

        showPause(false);
    }

    public void hideAllPanels()
    {
        for(i = 1; i < panel.Length; i++)
        {
            panel[i].SetActive(false);
        }
        desc_text.text = "";
    }

    public void ShowPanel(int p) //shows and updates panels
    {
        if (p < 0 || p > 7)
            return;
        if (p == currentPanel)
        {
            //if equal, disable it
            panel[p].SetActive(false);
            panel[(int)Panel.Description].SetActive(false);
            currentPanel = 0;
            return;
        }
        if(currentPanel != 0)
        {
            //disable other panel, except main
            panel[currentPanel].SetActive(false);
            panel[(int)Panel.Description].SetActive(false);
        }
        //enable new panel
        panel[p].SetActive(true);
        panel[(int)Panel.Description].SetActive(true);
        currentPanel = p;

        //Handle Panel
        switch ((Panel)p)
        {
            case Panel.Inventory:
                handleInventory();
                break;
            case Panel.Abilities:
                handleAbilities();
                break;
            case Panel.Status:
                handleStatus();
                break;
            case Panel.Save:
                handleSave();
                break;
            case Panel.Load:
                handleLoad();
                break;
            case Panel.Settings:
                handleSettings();
                break;
            case Panel.Quit:
                handleQuit();
                break;
            default:
                break;
        }
    }

    private void handleInventory()
    {
        //get inventory buttons object
        Transform tmp = panel[(int)Panel.Inventory].transform.GetChild(1);

        Button[] item_buttons = new Button[tmp.childCount];
        for(int i = 0; i < tmp.childCount; i++)
        {
            item_buttons[i] = tmp.GetChild(i).GetComponent<Button>();
        }
        UI.updateItems(this, gm.inventory, item_buttons);
    }

    private void handleAbilities()
    {
        updateSTPanel(panel[(int)Panel.Abilities].transform.GetChild(0), party, true);
        selectedSource(null);
    }
    private void handleStatus()
    {
        panel[(int)Panel.Description].SetActive(false); //hide description
        updateSTPanel(panel[(int)Panel.Status].transform.GetChild(0), party, false);
    }
    private void handleSave()
    {
        panel[(int)Panel.Description].SetActive(false); //hide description
        
        for(int i = 0; i < 3; i++)
        {
            UI.setSLButtonText(panel[(int)Panel.Save].transform.GetChild(i).GetComponent<Button>(), i, false);
        }

    }
    private void handleLoad()
    {
        panel[(int)Panel.Description].SetActive(false); //hide description
        
        for (int i = 0; i < 3; i++)
        {
            UI.setSLButtonText(panel[(int)Panel.Load].transform.GetChild(i).GetComponent<Button>(), i, true);
        }
    }
    private void handleSettings()
    {

    }
    private void handleQuit()
    {
        panel[(int)Panel.Description].SetActive(false); //hide description
    }

    public override void showTargets(Action ac)
    {
        selectedAction(ac);
    }

    private void updateSTPanel(Transform panel, List<GoodGuy> list, bool source)
    {

        int i;
        Transform [] btn = new Transform[panel.childCount];
        for (i = 0; i < panel.childCount; i++)
        {
            btn[i] = panel.GetChild(i);
        }

        i = 0;
        foreach(GoodGuy g in list)
        {
            if (i > btn.Length)
                break;

            UI.updateMeters(g.stats, btn[i].GetChild(0));

            btn[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (source)
            {
                btn[i].GetComponent<Button>().onClick.AddListener(() => selectedSource(g));
            } else
            {
                btn[i].GetComponent<Button>().onClick.AddListener(() => selectedTarget(g, panel.gameObject));
            }

            i++;
        }
    }

    private void selectedSource(GoodGuy s)
    {
        //TODO: better logic without gameobject.find
        GameObject tmp = GameObject.Find("inventory_target_btns");
        if(tmp)
            tmp.SetActive(false);
        tmp = GameObject.Find("abilities_target_btns");
        if(tmp)
            tmp.SetActive(false);

        currentSource = s;
        Transform t = panel[(int)Panel.Abilities].transform.GetChild(2);
        int i = 0;
        Button btn;
        if (s != null)
        {
            foreach (Ability a in s.stats.GetAbilities())
            {
                if (i > t.childCount)
                    break;
                if (a.ow_usable)
                {
                    btn = t.GetChild(i).GetComponent<Button>();
                    btn.enabled = true;
                    btn.gameObject.SetActive(true);
                    btn.GetComponentInChildren<Text>().text = a.ToString();
                    btn.onClick.RemoveAllListeners();
                    btn.GetComponent<ActionButton>().setDescription(a.GetDescription());
                    btn.onClick.AddListener(() => selectedAction(a));
                    i++;
                }

            }
        }
        if(i == 0 && s != null)
        {
            btn = t.GetChild(0).GetComponent<Button>();
            btn.gameObject.SetActive(true);
            btn.GetComponentInChildren<Text>().text = "None";
            btn.enabled = false;
            btn.onClick.RemoveAllListeners();
            btn.GetComponent<ActionButton>().setDescription(s + " doesn't have any overworld abilities...");
            i++;
        }
        for (; i < t.childCount; i++)
        {
            t.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void selectedAction(Action action)
    {
        currentAction = action;

        Transform p = panel[currentPanel].transform;
        Transform tp = null, sp = null;
        if(action is Ability)
        {
            sp = p.GetChild(0);
            tp = p.GetChild(3);
        } else if (action is ItemInfo)
        {
            sp = null;
            tp = p.GetChild(2);
        } else
        {
            Debug.Log("What are you doing exactly");
            return;
        }

        tp.gameObject.SetActive(true);
        updateSTPanel(tp, party, false);
        if (sp != null)
        {
            updateSTPanel(sp, party, true);
        }

        //also update sources panel if present
    }

    private void selectedTarget(GoodGuy s, GameObject target_panel)
    {
        if(currentAction != null)
        {
            bool output;
            currentTarget = s;
            string str = null;
            switch (currentPanel)
            {

                case (int)Panel.Inventory:
                    Debug.Log("Item: " + currentAction + ", Target: " + currentTarget);
                    str = AbilityDB.Process(party[0], currentAction, currentTarget, true, out output);
                    handleInventory();
                    target_panel.SetActive(false);
                    break;
                case (int)Panel.Abilities:
                    Debug.Log("Source: " + currentSource + ", Action: " + currentAction + ", Target: " + currentTarget);
                    str = AbilityDB.Process(currentSource, currentAction, currentTarget, true, out output);
                    target_panel.SetActive(false);
                    updateSTPanel(panel[(int)Panel.Abilities].transform.GetChild(0), party, true);
                    break;
                default:
                    break;
            }
            
            UI.changeText(desc_text, str);
        }

        if(currentPanel == (int)Panel.Status)
        {
            panel[(int)Panel.Status].transform.GetChild(2).GetComponent<Text>().text = s + "\n" + s.stats.ToString() + "\n\n\n" + s.description;
        }
    }
}
