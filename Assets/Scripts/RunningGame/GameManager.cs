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
    [HideInInspector]
    public int PlayerSpeed = 0;
    public bool isGameStart = false;
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
    public void GameOver(int score)
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
        isGameStart = true;
        Menu_Panel.SetActive(false);
        PlayerSpeed = 7;
        _playerCon.characterAnimator.SetTrigger("Run");
    }
    
    
}
