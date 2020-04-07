using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour
{

    private string description;

    public void setDescription(string desc)
    {
        description = desc;
    }
    public string getDescription()
    {
        return description;
    }
}
