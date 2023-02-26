using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Board board;

    [SerializeField] private NumberCoin numberCoin;
    [SerializeField] private GameObject youWin;
    [SerializeField] private GameObject youLose;
    [SerializeField] private AudioSource noticeSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private AudioSource backgroundSound;
    [SerializeField] private GameObject coin;
   

    private NumberCoin numCoin;
    public List<int> numberCalledList;
    private GameObject[] buttons;
    private GameObject continueButton;
    private GameObject[] numberOfComputer;
    private GameObject gameStats;
    private GameStats Stats;
    private GameObject soundOnButton;
    private GameObject soundOffButton;
    private AudioSource backgroundMusic;
    private GameObject canvasButton;
    private GameObject numberPanel;

    public int numberCalled;
    public bool won;
    public bool lose;
    public bool pause;
    private bool youWinExist;
    private bool youLoseExist;
    public Vector3 coinInScrollRectPosition;
    private int timeNumberButtonClicked;

    private void Awake()
    {
        backgroundMusic=Instantiate(backgroundSound);
        numberOfComputer = GameObject.FindGameObjectsWithTag("Board");
        gameStats = GameObject.Find("Game Stats");
        Stats = gameStats.GetComponent<GameStats>();
        for(int i=0;i<numberOfComputer.Length - Stats.numberComputer;i++)
        {
            numberOfComputer[i].SetActive(false);
        }

        buttons = GameObject.FindGameObjectsWithTag("Button");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        continueButton = GameObject.Find("Continue Button");
        continueButton.SetActive(false);

        soundOnButton = GameObject.Find("Sound On Button");
        soundOffButton = GameObject.Find("Sound Off Button");
        soundOffButton.SetActive(false);

        canvasButton = GameObject.Find("Content");
        numberPanel = GameObject.Find("Number Panel");
        numberPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;

        numberCalledList = new List<int>();
        won = false;
        lose = false;
        youWinExist = false;
        youLoseExist = false;
        pause = false;

        timeNumberButtonClicked = 0;
    }

    private void Update()
    {
        if (won)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (!buttons[i].activeInHierarchy)
                {
                    buttons[i].SetActive(true);
                }
            }

            if(!youWinExist)
            {
                Instantiate(youWin);
                playWInSound();
                youWinExist = true;
            }
        }

        if (lose)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (!buttons[i].activeInHierarchy)
                {
                    buttons[i].SetActive(true);
                }
            }

            if (!youLoseExist)
            {
                Instantiate(youLose);
                playLoseSound();
                youLoseExist = true;
            }
        }

        if (pause)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (!buttons[i].activeInHierarchy)
                {
                    buttons[i].SetActive(true);
                }
            }
            continueButton.SetActive(true);
        }   
    }

    public void callNumber()
    {
        numCoin = FindObjectOfType<NumberCoin>();
        if(numCoin == null)
        {
            if (!won && !lose && !pause)
            {
                numberCalled = Random.Range(1, 91);
                while (numberCalledList.Contains(numberCalled))
                {
                    numberCalled = Random.Range(1, 91);
                }
                numberCalledList.Add(numberCalled);
            }
        }
        else
        {
            if (!won && !lose && !pause && !numCoin.isRunning)
            {
                numberCalled = Random.Range(1, 91);
                while (numberCalledList.Contains(numberCalled))
                {
                    numberCalled = Random.Range(1, 91);
                }
                numberCalledList.Add(numberCalled);
            }
        }

    }

    public void onButtonPress()
    {
        numCoin = FindObjectOfType<NumberCoin>();
        if(numCoin == null)
        {
            if (!won && !lose && !pause)
            {
                Instantiate(numberCoin);
                timeNumberButtonClicked++;
            }
            
        }
        else
        {
            if (!won && !lose && !pause && !numCoin.isRunning)
            {
                GameObject coinPrefab = GameObject.FindGameObjectWithTag("Respawn");
                GameObject textPrefab = GameObject.FindGameObjectWithTag("TextRespawn");

                int n = timeNumberButtonClicked / 2;
                if(timeNumberButtonClicked%2==1)
                {
                    coinInScrollRectPosition = new Vector3(canvasButton.GetComponent<RectTransform>().position.x + 0.75f,
                                                            canvasButton.GetComponent<RectTransform>().position.y - 1.1f - 1.8f * n, 0f);

                }
                else
                {
                    /*if(timeNumberButtonClicked>=8)
                    {
                        canvasButton.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 50f);
                    }*/

                    coinInScrollRectPosition = new Vector3(canvasButton.GetComponent<RectTransform>().position.x + 2.75f, 
                                                            canvasButton.GetComponent<RectTransform>().position.y - 1.1f - 1.8f * (n - 1), 0f);

                }

                Destroy(coinPrefab);
                Instantiate(coin, coinInScrollRectPosition, Quaternion.identity, canvasButton.transform);
                textPrefab.transform.SetParent(canvasButton.transform);
                textPrefab.transform.localScale = new Vector3(0.15f, 0.15f, 1);
                textPrefab.transform.position = coinInScrollRectPosition;
                textPrefab.tag = "Untagged";

                Instantiate(numberCoin);
                timeNumberButtonClicked++;
            }
        }

    }

    public void pausePress()
    {
        if(!won && !lose)
        {
            pause = true;
        }
    }

    public void continuePress()
    {
        pause = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        continueButton.SetActive(false);
    }

    public void backToMenu()
    {
        Destroy(gameStats);
        SceneManager.LoadScene(0);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void playNoticeSound()
    {
        Instantiate(noticeSound);
    }
    public void playWInSound()
    {
        backgroundSound.Pause();
        Instantiate(winSound);
    }
    public void playLoseSound()
    {
        Instantiate(loseSound);
    }
    public void continueBackgroundSound()
    {
        backgroundSound.UnPause();
    }

    public void soundOnButtonClicked()
    {
        backgroundMusic.mute = true;
        soundOffButton.SetActive(true);
        soundOnButton.SetActive(false);
    }
    public void soundOffButtonClicked()
    {
        backgroundMusic.mute = false;
        soundOffButton.SetActive(false);
        soundOnButton.SetActive(true);
    }
}
