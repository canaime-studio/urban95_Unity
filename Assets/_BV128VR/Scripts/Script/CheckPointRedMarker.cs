using MobileVRInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointRedMarker : MonoBehaviour {


    public CheckpointData checkData;
    MissaoRondaCheckPoints scriptMRCP;
    public GameObject checkPoints;
    Transform minhaPosicao;
    BoxCollider collider;

    void Start()
    {
        scriptMRCP = checkPoints.GetComponent<MissaoRondaCheckPoints>();
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        minhaPosicao = GetComponent<Transform>();

    }
	void FixedUpdate () {
        float distancia = Vector3.Distance(GetComponent<Transform>().position, scriptMRCP.gameManager.playerAtivo.transform.position);
        if (distancia < 1.5)
        {
            collider.enabled = true;  
        }  

    }

    
   private void  OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        scriptMRCP.IndexCheckPointAtual++;
        scriptMRCP.SendMessage("ProximoCheck", scriptMRCP.IndexCheckPointAtual);
        collider.enabled = false;

   }

    public CheckpointData verificarDadosCheckpoint()
    {
        return checkData;
    }
    
}
