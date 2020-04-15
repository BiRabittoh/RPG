using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    private DialogueManager dm;
    private string colliderName;
    public Dialogue dialogue;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if (Input.GetKeyUp("e") && !dm.alreadyTalking)
            {
                TriggerDialogue();
            }
        }
        
    }

    public void TriggerDialogue()
    {
        dm.StartDialogue(this);
    }

    public abstract void actionAfterDialogue();
}
