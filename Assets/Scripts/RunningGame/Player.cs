using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    public CoinCollection _CoinColl;
    //public Road _road;
    public PlayerController _playerController;
    public RoadControler _roadCntrl;
    // public GameObject GameOver_Panel;


    private void OnTriggerEnter(Collider other)
    {
        // print("triggerd");
        if (other.gameObject.CompareTag("ExitTrigger"))
        {
         /*   exitObject=other.gameObject.transform.parent.gameObject;
            print(exitObject.name);*/
            _roadCntrl.DestroyexitObject(_roadCntrl.exitObject);
        }
        /*if (other.gameObject.CompareTag("Trigger1"))
        {

            road[1].transform.position = road[0].transform.position + new Vector3(0, 0, 107.8f);
            _CoinColl.SetActiveCoins1();
        }*/
        /*if (other.gameObject.CompareTag("Trigger2"))
        {

            road[0].transform.position = road[1].transform.position + new Vector3(0, 0, 108);
            _CoinColl.SetActiveCoins2();
        }*/
       if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager._inst.GameOver();
            GameManager._inst.PlayerSpeed = 0;
            _playerController.CharAnim();
            //GameManager._inst.HigestScoreGetter(GameManager._inst.H_text);
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            GameManager._inst.score++;
           GameManager._inst.UpdateScore(GameManager._inst.score,GameManager._inst._text);
            if (GameManager._inst.score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", GameManager._inst.score);
            }
            GameManager._inst.HigestScoreGetter(GameManager._inst.H_text);
        }
        
    }
   
   
    
}
