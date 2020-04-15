using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    #region Private
    private int currentTurn = 0;
    private Fighter currentTarget;
    private Action currentAction;
    private float lerpStart;
    private GameObject player;
    public BattleUIManager ui;
    private BadGuy tmpbad;
    private bool action_output = true;
    #endregion

    public Fighter turn;
    public BadGuy enemy;
    public int enemyGold;
    public Inventory enemyItems;

    public Camera main_camera;
    public GameMaster gm;
    
    enum Status
    {
        StartTurn = 0,
        EnableIO = 1,
        DisableIO = 3,
        NextTurn = 5,
        Wait = 69
    }
    private Status status = Status.Wait;
    

    [Header("Fighter Prefabs")]
    public GameObject p_paladin;
    public GameObject p_archer;
    public GameObject p_boxer;
    public GameObject p_mage;

    [Header("Enemy Prefabs")]
    public GameObject p_orc;
    public GameObject p_mutant;
    public GameObject p_brady;
    public GameObject p_ganfaul;
    //public GameObject p_enemy1;

    [Header("Current fighters")]
    public List<Fighter> fighters;

    // Start is called before the first frame update
    void Start()
    {
        //inizia la battaglia
        gm = GameMaster.Instance;
        ui = GetComponent<BattleUIManager>();

        fighters = new List<Fighter>();
        Fighter tmpfighter;
        foreach(Stats s in gm.Party)
        {
            //instantiate good guy
            tmpfighter = instantiateFighter(s.Name, s.Name + "Spawn").GetComponent<Fighter>();

            //set current HP and MP
            tmpfighter.stats = new Stats(s);

            //check if fighter's already dead
            if (tmpfighter.stats.HP == 0)
            {
                tmpfighter.animator.SetTrigger("Corpse");
                tmpfighter.dead = true;
            }

            fighters.Add(tmpfighter);
        }
        enemy = (BadGuy)instantiateFighter(gm.GetEnemyType(), "EnemySpawn").GetComponent<Fighter>();
        fighters.Add(enemy);

        enemyGold = enemy.goldDrop;
        enemyItems = enemy.itemsDrop;
        
        fighters.Sort(Fighter.SortByAgl);

        //aspetto che finisca l'animazione e poi vado
        UI.changeText(ui.status, enemy.stats.Name + " approaches!");
        StartCoroutine(waitThenStatus(UI.BattleSpeed, Status.StartTurn));

    }

    private void Update()
    {
        #region State Machine
        switch (status)
        {
            case Status.StartTurn:
                turn = fighters[currentTurn];
                //reset defence
                if (turn.defending)
                {
                    turn.defending = false;
                    turn.stats.DEF = turn.stats.MaxDEF;
                }

                if (turn.dead)
                {
                    status = Status.NextTurn;
                } else
                {
                    //step forward
                    if (action_output)
                    {
                        turnEffect(turn, true);
                    }
                    UI.changeText(ui.status, "It's " + turn + "'s turn.");

                    if (turn is BadGuy) //load AI
                    {
                        //execute AI
                        tmpbad = (BadGuy)turn;
                        UI.changeText(ui.status, tmpbad.Combat_AI());

                        //wait for animation
                        StartCoroutine(waitThenStatus(UI.BattleSpeed * 2, Status.NextTurn));
                    }
                    else 
                    {
                        //show input UI
                        ui.showAbilities(true);
                    }
                    status = Status.Wait;
                }
                break;
            case Status.DisableIO:
                UI.changeText(ui.status, AbilityDB.Process(turn, currentAction, currentTarget, true, out action_output));

                if (action_output)
                {
                    StartCoroutine(waitThenStatus(UI.BattleSpeed * 2, Status.NextTurn));
                } else
                {
                    StartCoroutine(waitThenStatus(UI.BattleSpeed * 2, Status.StartTurn));
                }
                status = Status.Wait;
                break;
            case Status.NextTurn:
                if (!turn.dead)
                {
                    turnEffect(turn, false);
                }
                //kill'em
                foreach (Fighter f in fighters)
                {
                    if(f.stats.HP == 0 && f.dead == false)
                    {
                        f.animator.SetTrigger("Dead");
                        f.dead = true;
                    }
                }
                
                //did i lose?
                if (countGuys(false) == 0)
                {
                    StartCoroutine(endBattle(UI.BattleSpeed * 5, false));
                    status = Status.Wait; //wait until scene has changed
                }
                else //did i win?
                if (countGuys(true) == 0)
                {
                    //start victory animations
                    foreach(Fighter f in getGuys(false))
                    {
                        f.animator.SetTrigger("Victory");
                    }
                    StartCoroutine(endBattle(UI.BattleSpeed * 3, true));
                    status = Status.Wait;
                } else
                {
                    //next turn
                    currentTurn++;
                    if (currentTurn == fighters.Count)
                    {
                        currentTurn = 0;
                    }
                    status = Status.StartTurn;
                }
                break;
        }
        #endregion
    }

    public void getInput(Action Action, Fighter Target)
    {
        if (status == Status.Wait)
        {
            currentAction = Action;
            currentTarget = Target;
            status = Status.DisableIO;
        }
    }

    public Fighter[] getGuys(bool bad)
    {
        List<Fighter> tmp = new List<Fighter>();
        foreach (Fighter figh in fighters)
        {
            if ((figh is BadGuy) == bad && !figh.dead)
            {
                tmp.Add(figh);
            }
        }
        return tmp.ToArray();
    }

    public int countGuys(bool bad)
    {
        int i = 0;
        foreach (Fighter figh in fighters)
        {
            if ((figh is BadGuy) == bad && !figh.dead)
            {
                i++;
            }
        }
        return i;
    }

    private GameObject instantiateFighter(string type, string spawnPoint)
    {
        Transform spot = GameObject.Find(spawnPoint).transform;
        switch (type)
        {
            case "Paladin":
                if (p_paladin)
                {
                    return Instantiate(p_paladin, spot.position, spot.rotation);
                }
                break;
            case "Archer":
                if (p_archer)
                {
                    return Instantiate(p_archer, spot.position, spot.rotation);
                }
                break;
            case "Boxer":
                if (p_boxer)
                {
                    return Instantiate(p_boxer, spot.position, spot.rotation);
                }
                break;
            case "Mage":
                if (p_mage)
                {
                    return Instantiate(p_mage, spot.position, spot.rotation);
                }
                break;

            case "Orc":
                return Instantiate(p_orc, spot.position, spot.rotation);
            case "Mutant":
                return Instantiate(p_mutant, spot.position, spot.rotation);
            case "Brady":
                return Instantiate(p_brady, spot.position, spot.rotation);
            case "Ganfaul":
                return Instantiate(p_ganfaul, spot.position, spot.rotation);
            default:
                Debug.Log("Wrong name!");
                return null;
        }
        return null;
    }

    private IEnumerator endBattle(float offset, bool didYouWin)
    {
        yield return new WaitForSeconds(offset);
        if (didYouWin)
        {
            ui.showResults(enemy.goldDrop, enemyItems);
            yield return new WaitForSeconds(offset + 1);
            gm.EndBattle(true);
        } else
        {
            gm.EndBattle(false);
        }
    }

    private IEnumerator waitThenStatus(float seconds, Status nextStatus)
    {
        yield return new WaitForSeconds(seconds);
        status = nextStatus;
    }

    private IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
    }

    public void turnEffect(Fighter f, bool onOff)
    {
        int distance = 4;

        if(f is BadGuy)
        {
            distance *= -1;
        }
        if (!onOff)
        {
            distance *= -1;
        }

        StartCoroutine(MoveFromTo(f.transform, f.transform.position, f.transform.position + new Vector3(0, 0, distance), 20));
    }
}