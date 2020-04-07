using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class BattleUIManager : UIManager
{
    private BattleManager battle;
    private GameMaster gm;

    //smart input variables
    private const string itemsGuiName = "Items";
    private const int maxAbilityNumber = 6;
    private const int maxTargetNumber = 5;
    private const int maxItemNumber = 9;

    private Button[] ability_buttons;
    private Button[] item_buttons;
    private Button[] target_buttons;
    private List<Ability> abilities;
    private Coroutine text_coroutine;
    private int i = 0;


    public Text status;
    public GameObject status_panel;
    public GameObject results_panel;
    public Text drops;
    public GameObject input_panel;
    public GameObject targets_panel;
    public GameObject items_panel;

    private void Start()
    {
        gm = GameMaster.Instance;
        battle = GetComponent<BattleManager>();

        /*
        status_panel = GameObject.Find("status_panel");
        status = status_panel.GetComponentInChildren<Text>();
        input_panel = GameObject.Find("Input_UI");
        items_panel = GameObject.Find("items_panel");
        targets_panel = GameObject.Find("targets_panel");
        results_panel = GameObject.Find("Results_UI");
        drops = GameObject.Find("drops_text").GetComponent<Text>();
        */

        input_panel.SetActive(false);
        targets_panel.SetActive(false);
        items_panel.SetActive(false);
        results_panel.SetActive(false);

        ability_buttons = input_panel.transform.GetComponentsInChildren<Button>();
        target_buttons = targets_panel.GetComponentsInChildren<Button>();
        item_buttons = items_panel.GetComponentsInChildren<Button>();

        
        #region Set target buttons
        foreach (Fighter f in battle.fighters)
        {
            if (f.fighter_number < 0 || f.fighter_number >= maxTargetNumber)
            {
                i = maxTargetNumber - 1;
            }
            else
            {
                i = f.fighter_number;
            }
            target_buttons[i].GetComponentInChildren<Text>().text = f.ToString();
        }
        foreach (Button b in target_buttons)
        {
            if (b.GetComponentInChildren<Text>().text == "")
            {
                b.gameObject.SetActive(false);
            }
        }
        #endregion
    }
    
    public void showAbilities(bool show) //show abilities menu 
    {
        if (show && input_panel.activeSelf == false)
        {
            input_panel.SetActive(true);

            //read abilities from current fighter
            abilities = battle.turn.stats.GetAbilities();

            i = 0;
            foreach(Ability a in abilities)
            {
                if(i == maxAbilityNumber - 1)
                {
                    break;
                }

                //show ability_buttons
                ability_buttons[i].gameObject.SetActive(true);
                ability_buttons[i].GetComponentInChildren<Text>().text = a.ToString();
                ability_buttons[i].GetComponent<ActionButton>().setDescription(a.GetDescription());
                ability_buttons[i].onClick.AddListener(() => selectTarget(a));
                i++;
            }

            //add Items entry for everybody
            ability_buttons[i].gameObject.SetActive(true);
            ability_buttons[i].GetComponentInChildren<Text>().text = itemsGuiName;
            ability_buttons[i].GetComponent<ActionButton>().setDescription("Use an item.");
            ability_buttons[i].onClick.AddListener(() => showItems(true));
            i++;

            for (; i < maxAbilityNumber; i++)
            {
                //disable remaining ability_buttons
                ability_buttons[i].gameObject.SetActive(false);
            }

        } else if (!show && input_panel.activeSelf == true)
        {
            input_panel.SetActive(false);
        }
    }

    

    private void showItems(bool onOff)
    {
        if (onOff)
        {
            items_panel.SetActive(true);
            targets_panel.SetActive(false);
            UI.updateItems(this, gm.inventory, item_buttons);
        } else
        {
            items_panel.SetActive(false);
        }
        
    }

    public override void showTargets(Action ac) //show targets menu for abilities 
    {
        targets_panel.SetActive(true);
        if(ac is Ability)
        {
            items_panel.SetActive(false);
        }

        //set current buttons
        foreach (Fighter f in battle.fighters)
        {
            if (f.fighter_number < 0 || f.fighter_number >= maxTargetNumber)
            {
                i = maxTargetNumber - 1;
            }
            else
            {
                i = f.fighter_number;
            }

            target_buttons[i].onClick.RemoveAllListeners();
            target_buttons[i].onClick.AddListener(() => sendInput(ac, f));
        }
    }

    private void sendInput(Action ac, Fighter target)
    {
        //hide panel
        targets_panel.SetActive(false);
        items_panel.SetActive(false);
        input_panel.SetActive(false);

        //send input
        battle.getInput(ac, target);
    }

    private void selectTarget(Ability a) //asks user the target for an ability 
    {
        if (a.hasTarget)
        {
            UI.changeText(status, battle.turn + ": Select target for " + a.guiName);
            showTargets(a);
        } else
        {
            sendInput(a, null);
        }
    }

    public void showResults(int gold, Inventory items)  //shows drops list at the end of battle 
    {
        drops.text = gold + "g\n";

        if (items != null)
        {
            foreach (ItemInfo it in items.getContents())
            {
                drops.text += "\n" + it.ToString();
            }
        }
        //hide status, show results_panel
        status_panel.SetActive(false);
        results_panel.SetActive(true);
    }

    public void ShowDamage(Fighter f, int damage, bool crit) //shows damage tooltip above a fighter. TODO: handle text color (green for healing)
    {
        StartCoroutine(showDamageLate(f, damage, crit, UI.BattleSpeed, UI.BattleSpeed * 0.5f));
    }

    #region Damage Tooltip stuff
    private IEnumerator showDamageLate(Fighter f, int damage, bool crit, float offset, float duration)
    {
        //get tooltip
        Text tmptext = f.damage_tooltip.GetComponent<Text>();
        //wait for attack animation to finish
        yield return new WaitForSeconds(offset);

        //show tooltip
        tmptext.text = "";
        tmptext.color = new Color(1f, 0.5f, 0f);
        if (damage == 0)
        {
            tmptext.text += "Miss!";
        }
        else
        {
            if(damage < 0)
            {
                //target was healed
                damage *= -1;
                tmptext.color = Color.green;
            } else
            {
                //target was damaged
                f.animator.SetTrigger("Damage");
                if (crit)
                {
                    tmptext.color = Color.red;
                }
            }
            tmptext.text += damage;
        }
        

        //wait, then hide tooltip
        yield return new WaitForSeconds(duration);
        f.damage_tooltip.GetComponent<Text>().text = "";

    }
    #endregion

}
