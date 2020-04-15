using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Leaderboard
{
    private int maxRecordsNumber;
    private List<Record> _records;
    public List<Record> Records
    {
        get
        {
            _records = _records.OrderByDescending(y => y.Score).ThenBy(y => y.Name).ToList();
            return _records;
        }
        set
        {
            _records = value;
        }
    }

    public void AddRecord(Record newRecord)
    {
        if(Records.Count == maxRecordsNumber)
        { // full leaderboard
            if(newRecord.Score > Records[maxRecordsNumber - 1].Score){
                Records[maxRecordsNumber - 1] = newRecord;
            }
        } else {
            Records.Add(newRecord);
        }

    }

    public Leaderboard(int max){
        maxRecordsNumber = max;
        _records = new List<Record>();
    }
}
