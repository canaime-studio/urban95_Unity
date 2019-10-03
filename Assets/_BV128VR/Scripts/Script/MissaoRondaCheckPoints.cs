using MobileVRInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissaoRondaCheckPoints : MonoBehaviour {

    public CheckpointData[] checkpointsData;
   // public Text descricaoMissao;
    public CheckPointRedMarker checkPointRedMarkerPrefab;
    public int IndexCheckPointAtual=0;
    public CheckPointRedMarker checkPointObject;
    //public GameObject Player;
    public bool statusMissao=false;

    public MissaoDetalhes ProximaMissaoDetalhes;

    public GameManager gameManager;

    //public AudioClip audioClip;
    //public AudioSource audioSource;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = GameManager.FindObjectOfType<GameManager>();        
    }


    //Excluir
    //public VRInventoryExampleSceneController1 exampleController1;
    public int PremioMissao;


    void Start () {
        //IniciaMissao();

    }

    public void IniciaMissao()
    {
        IndexCheckPointAtual = 0;
        checkPointObject =  Instantiate(checkPointRedMarkerPrefab, checkpointsData[IndexCheckPointAtual].gameObject.transform.position, checkpointsData[IndexCheckPointAtual].gameObject.transform.rotation);        
        checkPointObject.transform.SetParent(checkpointsData[IndexCheckPointAtual].transform);
        checkPointObject.checkData = checkpointsData[IndexCheckPointAtual];
        checkPointObject.checkPoints = gameObject;
    }
	

    public void ProximoCheck(int proxIndex)
    {        
        if(proxIndex < checkpointsData.Length)
        {
            //exampleController1.ShowMessage("Locais visitados: " + IndexCheckPointAtual + " / " + checkpointsData.Length);
            Debug.Log("tentar tocar Audio");
            Debug.Log("Proximo Check");
            //Debug.Log(checkPointObject.GetComponent<Collider>());
            checkPointObject.transform.position = new Vector3(checkpointsData[proxIndex].transform.position.x, checkpointsData[proxIndex].transform.position.y, checkpointsData[proxIndex].transform.position.z);
            checkPointObject.transform.SetParent(checkpointsData[proxIndex].transform);
            checkPointObject.checkData = checkpointsData[proxIndex];
            
            CheckPoint();
        } else if(proxIndex == checkpointsData.Length)
        {
            Debug.Log("Ultimo Check - Missão Completa: " + checkPointObject.name);
            MissaoCompleta();
            Destroy(checkPointObject.gameObject);
        }
        else
        {
            //Debug.Log("teste final  Check");            
            statusMissao = false;
            checkPointObject.gameObject.SetActive(false);
            Destroy(checkPointObject);
        }
    }

    public void CheckPoint()
    {
        checkPointObject.checkData.CheckPointCapturado();
        gameManager.playerAtivo.Parar();
        
        //Player.GetComponent<VRAutoWalk>().parar();
        //Debug.Log("Check Capturado" + checkPointObject.checkData.descricao);
    }

    public void MissaoCompleta()
    {
        checkPointObject.checkData.CheckPointCapturado();
        gameManager.playerAtivo.Parar();
        //Player.GetComponent<VRAutoWalk>().parar();
        //Debug.Log("Missao Completa" + checkPointObject.checkData.descricao);
        ProximaMissaoDetalhes.eventoAtivador();
        ProximaMissaoDetalhes.ativa = true;
    }
}

