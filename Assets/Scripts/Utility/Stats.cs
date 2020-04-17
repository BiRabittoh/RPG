using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    #region Private
    private string name;

    private double hp;
    private double mp;
    private double atk;
    private double def;
    private double agl;
    private double lck;

    private double maxHP;
    private double maxMP;
    private double maxATK;
    private double maxDEF;
    private double maxAGL;
    private double maxLCK;

    private List<Ability> abilities;
    #endregion

    #region Constructors
    public Stats(string name, double HP, double MP, double ATK, double DEF, double AGL, double LCK, List<Ability> abilities)
    {
        this.name = name;
        hp = maxHP = HP;
        mp = maxMP = MP;
        atk = maxATK = ATK;
        def = maxDEF = DEF;
        agl = maxAGL = AGL;
        lck = maxLCK = LCK;
        
        this.abilities = new List<Ability>(abilities);
    }

    public Stats(Stats s) //copy stats
    {
        name = s.name;

        maxHP = s.MaxHP;
        maxMP = s.MaxMP;
        maxATK = s.MaxATK;
        maxDEF = s.MaxDEF;
        maxAGL = s.MaxAGL;
        maxLCK = s.MaxLCK;

        hp = s.HP;
        mp = s.MP;
        atk = s.ATK;
        def = s.DEF;
        agl = s.AGL;
        lck = s.LCK;

        abilities = new List<Ability>(s.abilities);
    }
    #endregion

    #region Stats DB
    public Stats(string key)
    {
        name = key;
        abilities = new List<Ability>();
        abilities.Add(new Attack());
        switch (key)
        {
            case "Paladin":
                hp = maxHP = 500;
                mp = maxMP = 0;
                atk = maxATK = 40;
                def = maxDEF = 63;
                agl = maxAGL = 30;
                lck = maxLCK = 15;
                abilities.Add(new Defend());
                break;
            case "Archer":
                hp = maxHP = 400;
                mp = maxMP = 400;
                atk = maxATK = 30;
                def = maxDEF = 50;
                agl = maxAGL = 35;
                lck = maxLCK = 40;
                abilities.Add(new Defend());
                abilities.Add(new Heal());
                abilities.Add(new ReviveHalf());
                break;
            case "Boxer":
                hp = maxHP = 600;
                mp = maxMP = 0;
                atk = maxATK = 50;
                def = maxDEF = 80;
                agl = maxAGL = 12;
                lck = maxLCK = 5;
                abilities.Add(new Defend());
                break;
            case "Mage":
                hp = maxHP = 375;
                mp = maxMP = 450;
                atk = maxATK = 5;
                def = maxDEF = 50;
                agl = maxAGL = 32;
                lck = maxLCK = 5;
                abilities.Add(new HealBig());
                abilities.Add(new FireBall());
                abilities.Add(new ReviveFull());
                break;
            case "Orc":
                hp = maxHP = 700;
                mp = maxMP = 0;
                atk = maxATK = 40;
                def = maxDEF = 55;
                agl = maxAGL = 25;
                lck = maxLCK = 15;
                break;
            case "Mutant":
                hp = maxHP = 700;
                mp = maxMP = 0;
                atk = maxATK = 43;
                def = maxDEF = 60;
                agl = maxAGL = 27;
                lck = maxLCK = 20;
                break;
            case "Brady":
                hp = maxHP = 1500;
                mp = maxMP = 600;
                atk = maxATK = 50;
                def = maxDEF = 60;
                agl = maxAGL = 30;
                lck = maxLCK = 35;
                abilities.Add(new FireBall());
                abilities.Add(new HealBig());
                break;
            case "Ganfaul":
                hp = maxHP = 2750;
                mp = maxMP = 660;
                atk = maxATK = 55;
                def = maxDEF = 60;
                agl = maxAGL = 32;
                lck = maxLCK = 30;
                abilities.Add(new FireBall());
                abilities.Add(new HealBig());
                break;
            default:

                break;
        }
    }
    #endregion

    #region Getters and Setters
    public string Name { get => name; protected set => name = value; }

    public double HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (value > MaxHP)
            {
                hp = MaxHP;
            }
            else
            if(value < 0)
            {
                hp = 0;
            }
            else
            {
                hp = value;
            }
        }
    }
    public double MP
    {
        get

        {
            return mp;
        }
        set
        {
            if (value > MaxMP)
            {
                mp = MaxMP;
            }
            else
            if (value < 0)
            {
                mp = 0;
            }
            else
            {
                mp = value;
            }
        }
    }
    public double ATK
    {
        get
        {
            return atk;
        }
        set
        {
            if (value < 1)
            {
                atk = 1;
            } else
            {
                atk = value;
            }
        }
    }
    public double DEF
    {
        get
        {
            return def;
        }
        set
        {
            if (value < 1)
            {
                def = 1;
            }
            else
            {
                def = value;
            }
        }
    }
    public double AGL
    {
        get
        {
            return agl;
        }
        set
        {
            if (value < 1)
            {
                agl = 1;
            }
            else
            {
                agl = value;
            }
        }
    }
    public double LCK
    {
        get
        {
            return lck;
        }
        set
        {
            if (value < 1)
            {
                lck = 1;
            }
            else
            {
                lck = value;
            }
        }
    }

    public double MaxHP { get => maxHP; protected set => maxHP = value; }
    public double MaxMP { get => maxMP; protected set => maxMP = value; }
    public double MaxATK { get => maxATK; protected set => maxATK = value; }
    public double MaxDEF { get => maxDEF; protected set => maxDEF = value; }
    public double MaxAGL { get => maxAGL; protected set => maxAGL = value; }
    public double MaxLCK { get => maxLCK; protected set => maxLCK = value; }

    public List<Ability> GetAbilities()
    {
        return new List<Ability>(abilities);
    }
    #endregion
    

    public override string ToString()
    {
        return "HP: " + (int)hp + "/" + (int)maxHP + "\n" +
               "MP: " + (int)mp + "/" + (int)maxMP + "\n" +
               "ATK: " + (int)atk + "\n" +
               "DEF: " + (int)def + "\n" +
               "AGL: " + (int)agl + "\n" +
               "LCK: " + (int)lck;
    }
}
