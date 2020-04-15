using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Record
{
    private string _name;

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
    }
    public string Name
    {
        get
        {
            return _name;
        }
    }

    public string ScoreToString()
    {
        return _score + "g";
    }

    public Record(string name, int score)
    {
        _name = name;
        _score = score;
    }



}
