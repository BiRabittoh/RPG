using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public static class UI
{
    private static GameMaster gm = GameMaster.Instance;
    private static BattleUIManager battle = null;
    private static Coroutine text_coroutine;


    //hh, HH: 24-hour, 12-hour; MMMM, MM: March, 03
    public static string SaveDateFormat = "dd/MM/yyyy HH:mm";
    public static float TextSpeed = PlayerPrefs.GetFloat("TextSpeed", 0.015f);
    public static float BattleSpeed = PlayerPrefs.GetFloat("BattleSpeed", 1f);
    public static bool displaying_text = false;

    public static string getPlayTimeString(float timer){
        return Mathf.FloorToInt(timer / 60).ToString("00") + ":" + Mathf.RoundToInt(timer % 60).ToString("00");
    }
    public static void setSLButtonText(Button btn, int slot, bool load)
    {
        //Debug.Log("filling button " + btn.ToString());
        float timer;
        DateTime dt = GetSlotDateTime(slot, out timer);
        if(dt == default)
        {
            if(load)
            {
                btn.enabled = false;
            } else
            {
                btn.enabled = true;
            }
            btn.GetComponentInChildren<Text>().text = "Empty";
        } else
        {
            btn.GetComponentInChildren<Text>().text = dt.ToString(SaveDateFormat) + "\nPlay time " + getPlayTimeString(timer);
            btn.enabled = true;
        }
        
    }

    public static DateTime GetSlotDateTime(int slot, out float timer)
    {
        if (File.Exists(Application.persistentDataPath + "/game" + slot + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game" + slot + ".save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            timer = save.timerfloat;
            return save.timestamp;
        }
        else
        {
            timer = 0;
            return default;
        }
    }
    public static bool ShowDamage(Fighter target, int danno, bool didCrit)
    {
        if (!battle)
        {
            battle = UnityEngine.Object.FindObjectOfType<BattleUIManager>();
        }

        if (battle)
        {
            battle.ShowDamage(target, danno, didCrit);
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Slow Text
    public static void changeText(Text textComp, string content) //change any text slowly.
    {
        if(Time.timeScale == 0f)
        {
            textComp.text = content;
        } else
        {
            if (displaying_text)
            {
                stopText();
            }
            textComp.text = "";
            text_coroutine = gm.StartCoroutine(TypeText(textComp, content, null, null, TextSpeed));
        }
    }

    public static void changeDialogue(Text textComp, string content, AudioSource source, AudioClip clip) //change text with sound
    {
        if (Time.timeScale == 0f)
        {
            textComp.text = content;
        }
        else
        {
            if (displaying_text)
            {
                stopText();

            }
            textComp.text = "";
            text_coroutine = gm.StartCoroutine(TypeText(textComp, content, source, clip, TextSpeed));
        }
    }
    public static void stopText()
    {
        displaying_text = false;
        gm.StopCoroutine(text_coroutine);
    }


    public static IEnumerator TypeText(Text textComp, string message, AudioSource source, AudioClip clip, float seconds)
    {
        if(message == null)
        {
            yield break;
        }
        displaying_text = true;
        int i = 0;
        foreach (char letter in message.ToCharArray())
        {
            i++;
            textComp.text += letter;
            if (clip && i % 2 == 0)
            {
                source.PlayOneShot(clip);
            }

            yield return null;
            yield return new WaitForSeconds(seconds);
        }
        displaying_text = false;
    }
    #endregion

    public static void updateItems(UIManager ui, Inventory inv, Button[] item_buttons)
    {
        int i = 0;
        foreach (ItemInfo it in inv.getContents())
        {
            if (i == item_buttons.Length)
            {
                break;
            }

            //show item_buttons
            if (it.item.consumable)
            {
                item_buttons[i].GetComponent<ActionButton>().setDescription(it.GetDescription());
                item_buttons[i].onClick.AddListener(() => ui.showTargets(it));
                item_buttons[i].gameObject.SetActive(true);
                item_buttons[i].GetComponentInChildren<Text>().text = it.ToString();
                i++;
            }
        }
        for (; i < item_buttons.Length; i++) //disable remaining item_buttons 
        {
            item_buttons[i].gameObject.SetActive(false);
        }
    }

    public static void updateMeters(Stats s, Transform t)
    {
        t.GetComponent<Text>().text = s.Name + ":";

        Transform tmphp = t.GetChild(0), tmpmp = null;

        tmphp.GetComponent<Text>().text = (int)s.HP + "/" + (int)s.MaxHP + " HP";
        tmphp.GetComponentInChildren<Slider>().value = (float)(s.HP / s.MaxHP);
        if (s.MaxMP > 0)
        {
            tmpmp = t.GetChild(1);
            tmpmp.GetComponent<Text>().text = (int)s.MP + "/" + (int)s.MaxMP + " MP";
            tmpmp.GetComponentInChildren<Slider>().value = (float)(s.MP / s.MaxMP);
        }
    }

    public static void showCursor(bool toggle){
        Cursor.visible = toggle;
        if(toggle){
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
