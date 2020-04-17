using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject backButton = null, prevButton = null, nextButton = null, startButton = null;
    [SerializeField] private GameObject[] pages = null;

    private int page = 0;

    private void showPage()
    {
        if(page > -1 && page <= pages.Length)
        {
            foreach(GameObject p in pages)
            {
                p.SetActive(false);
            }
            pages[page].SetActive(true);
        } else {
            Debug.Log("Can't show page " + page);
        }

        if(page == pages.Length - 1)
        {
            nextButton.SetActive(false);
            startButton.gameObject.SetActive(true);
        } else {
            nextButton.SetActive(true);
            startButton.gameObject.SetActive(false);
        }
        if(page == 0)
        {
            prevButton.SetActive(false);
            backButton.gameObject.SetActive(true);
        } else {
            prevButton.SetActive(true);
            backButton.gameObject.SetActive(false);
        }

    }

    public void backToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void nextPage()
    {
        page++;
        //Debug.Log((page - 1) + " => " + page);
        showPage();
    }

    public void prevPage()
    {
        page--;
        //Debug.Log((page + 1) + " => " + page);
        showPage();
    }

    public void startGame()
    {
        Time.timeScale = 1;
        GameMaster.Instance.resetGame();
        SceneManager.LoadScene("Level0");
    }
    void Start()
    {
        Time.timeScale = 0;
    }
}
