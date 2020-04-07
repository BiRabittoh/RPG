using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Fighter : MonoBehaviour
{
    #region Public
    [HideInInspector] public bool defending;
    [HideInInspector] public GameObject damage_tooltip;
    [HideInInspector] public BattleManager battle;
    [TextArea] public string description;

    [Header("Settings")]
    public Animator animator;
    public bool dead;
    public Stats stats;
    
    #endregion
    
    public int fighter_number = -1;


    public static int SortByHp(Fighter f1, Fighter f2) //before = weaker
    {
        return f1.stats.HP.CompareTo(f2.stats.HP);
    }

    public static int SortByAgl(Fighter f1, Fighter f2) //before = faster
    {
        return f2.stats.AGL.CompareTo(f1.stats.AGL);
    }

    protected void Start()
    {
        //battle = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        animator = GetComponent<Animator>();
        GetGUI();
    }

    public abstract void GetGUI();

    public override string ToString()
    {
        return stats.Name;
    }
    
}
