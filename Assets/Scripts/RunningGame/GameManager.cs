using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _inst;
    public PlayerController _playerCon;
    [HideInInspector]
    public int score = 0;
    public int highScore;
    public Text _text;
    public Text H_text;
    public GameObject GameOver_Panel;
    public GameObject Menu_Panel;
    public GameObject playerSelct_panel;
    public GameObject Lock_panel;
    [HideInInspector]
    public int PlayerSpeed = 0;
    public bool isGameStart = false;
    //public Image Btn_GameStart;
    public Button startButton;



    private int selectedPlayer = 0;
    private void Awake()
    {
     // PlayerPrefs.DeleteAll();
    }
    private void Start()
    {
        _inst = this;
        HigestScoreGetter(H_text);
        UpdateScore(score,_text);
    }
    public void UpdateScore(int score, Text _text)
    {
        //PlayerPrefs.SetInt("HigestScore", score);
        _text.text = "Score: " + score;
        
    }
    public void HigestScoreGetter( Text _text)
    {
        
            highScore = PlayerPrefs.GetInt("HighScore");

        
        _text.text = "HighScore: " + highScore;

    }
    public void GameOver()
    {
        GameOver_Panel.SetActive(true);
        isGameStart = false ;
        /*if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);

        }*/
        //highScore = PlayerPrefs.GetInt("HighScore");

    }
    public void RetryButton()
    {
        SceneManager.LoadScene("RunningGame1");
        // GameManager._inst.HigestScoreGetter(GameManager._inst.H_text);
    }
    public void StartButtonClick()
    {
        Menu_Panel.SetActive(false);
        playerSelct_panel.SetActive(true);
    }
    public void GameStart()
    {
        isGameStart = true;
        playerSelct_panel.SetActive(false);
        PlayerSpeed = 7;
        _playerCon.characterAnimator.SetTrigger("Run");
    }
    public void SelectPlayerBtnClick(int val)// left click =-1  right=1
    {
        startButton.interactable = false;
        Lock_panel.SetActive(true);
        selectedPlayer = selectedPlayer + val;  //-1 +1
        selectedPlayer = Mathf.Clamp(selectedPlayer, 0, _playerCon.allCharactors.Count - 1);
        //Debug.Log(selectedPlayer);
        for (int i = 0; i < _playerCon.allCharactors.Count; i++)
        {
            if (i == selectedPlayer)
            {
                _playerCon.allCharactors[selectedPlayer].SetActive(true);
                _playerCon.characterAnimator = _playerCon.allCharactors[selectedPlayer].GetComponent<Animator>();
            }
            else
                _playerCon.allCharactors[i].SetActive(false);
        }
        if(_playerCon.allCharactors[selectedPlayer] == _playerCon.allCharactors[0])
        {
            startButton.interactable = true;
            Lock_panel.SetActive(false);
        }
        UnlockChar();
    }
    public void UnlockChar()
    {
        for (int i = 1; i <= _playerCon.allCharactors.Count-1; i++)
        {
            if (highScore >= i * 100 && _playerCon.allCharactors[selectedPlayer] == _playerCon.allCharactors[i])
            {
                startButton.interactable = true;
                Lock_panel.SetActive(false);
               // break;
            }
        }
        /*  if (selectedPlayer < value)
          {
              startButton.interactable = true;
              Lock_panel.SetActive(false);
          }
          else
          {
              startButton.interactable = false;
              Lock_panel.SetActive(true);
          }*/

    }

}
