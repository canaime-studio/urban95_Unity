using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cronometro : MonoBehaviour
{
    public Text cronometro;
    public float tempoJogo = 100f;
    

    // Update is called once per frame
    void Update()
    {

        if (tempoJogo > 0)
        {
            tempoJogo -= Time.deltaTime;
           // int tempoTXT = ;

            cronometro.text = ""+ (int)tempoJogo+ "''";
        }
        
    }
}
