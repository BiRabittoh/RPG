using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject inputPanel = null;
    [SerializeField] private GameObject highscoresPanel = null;
    [SerializeField] private Text highscoreName = null;
    [SerializeField] private Text[] nameObjects = null;
    [SerializeField] private Text[] scoreObjects = null;
    [SerializeField] private Text scoreText = null;
    
    [Header("Good Guys")]
    [SerializeField] private GameObject[] goodGuys = null;

    private Leaderboard leaderboard;
    private int userScore;
    const int maxRecordsNumber = 4;

    void Start(){
        GameMaster gm = GameMaster.Instance;
        userScore = gm.gold;
        string timeString = gm.GetTimerString();
        //TODO: set text
        scoreText.text = "You finished the game in " + timeString + "\nand you collected " + userScore + "g";
    }

    public void saveHighScore()
    {
        inputPanel.SetActive(false);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        //check if leaderboard file exists
        if (File.Exists(Application.persistentDataPath + "/leaderboard.bin"))
        {
           
            file = File.Open(Application.persistentDataPath + "/leaderboard.bin", FileMode.Open);
            leaderboard = (Leaderboard) bf.Deserialize(file);
            file.Close();
        } else {
            leaderboard = new Leaderboard(maxRecordsNumber);
        }

        //add new record
        leaderboard.AddRecord(new Record(highscoreName.text, userScore));
        file = File.Create(Application.persistentDataPath + "/leaderboard.bin");
        bf.Serialize(file, leaderboard);
        file.Close();

        //show records list
        int i = 0;
        foreach (Record record in leaderboard.Records)
        {
            nameObjects[i].enabled = true;
            nameObjects[i].text = (i + 1) + ". " + record.Name;
            scoreObjects[i].enabled = true;
            scoreObjects[i].text = record.ScoreToString();
            i++;
        }
        for(; i < maxRecordsNumber; i++)
        {
            nameObjects[i].enabled = false;
            scoreObjects[i].enabled = false;
        }

        highscoresPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
