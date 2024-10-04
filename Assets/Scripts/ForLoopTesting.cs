using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ForLoopTesting : MonoBehaviour
{
    public Text NewText;
    int rows = 5;
    int column = 6;

    public void LoopTesting()
    {
        //int k = 0;
        for(int i=0;i<rows;i++)
        {
            
            for(int j=0;j<(column-i-1); j++)
            {
                //k ++;
                // Debug.Log(1);
                NewText.text += " ";
            }
            for(int k = 0; k < (2 * i + 1); k++)
            {
                NewText.text += "*";
            }
            NewText.text += "\n";
        }
    }
  /*  public void testing()
    {
        NewText.text += j ;
    }*/
    // Start is called before the first frame update
    void Start()
    {
        LoopTesting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
