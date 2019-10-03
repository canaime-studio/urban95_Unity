using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointData : MonoBehaviour {

    public int ID;
    public string descricao;
    public int ordem;
    public int recompensa;
    public bool concluida;

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {		
	}


    public void CheckPointCapturado()
    {
        concluida = true;
    }
}
