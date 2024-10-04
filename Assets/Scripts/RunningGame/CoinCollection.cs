using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public GameObject[] Coins1;
    public GameObject[] Coins2;

    public void SetActiveCoins1()
    {
        for(int i = 0; i < Coins1.Length; i++)
        {
            Coins1[i].gameObject.SetActive(true);
        }
    }
    public void SetActiveCoins2()
    {
        for (int i = 0; i < Coins2.Length; i++)
        {
            Coins2[i].gameObject.SetActive(true);
        }
    }

}
