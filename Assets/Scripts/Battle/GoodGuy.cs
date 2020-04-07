using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GoodGuy : Fighter
{
    protected Text hp_text, mp_text, status_text;
    protected Slider hp_slider, mp_slider;

    public bool inBattle = true;
    
    public override void GetGUI()
    {
        if (inBattle)
        {
            damage_tooltip = GameObject.Find("f" + fighter_number + "_damage");
            status_text = GameObject.Find("f" + fighter_number + "_status").GetComponent<Text>();
            hp_text = GameObject.Find("f" + fighter_number + "_hp").GetComponent<Text>();
            hp_slider = hp_text.GetComponentInChildren<Slider>();


            if (stats.MaxMP > 0)
            {
                mp_text = GameObject.Find("f" + fighter_number + "_mp").GetComponent<Text>();
                mp_slider = mp_text.GetComponentInChildren<Slider>();
            }

            status_text.transform.localScale = Vector3.one;
            updateStatus();
        }
        

    }

    public void updateStatus()
    {
        UI.updateMeters(stats, status_text.transform);
    }
}
