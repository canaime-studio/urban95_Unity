using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject player;
    public PlayerCutsceneSpeedController playerCutscene;
    public GameManager gameManager;
    public GameObject playerRoot;

    public Component inputController;


    private void Start()
    {
        //if(SetComponents())
        //{
        //    Debug.Log("Tudo Okay");
        //} else
        //{
        //    Debug.Log("Algum componente Nao está referenciado");
        //}
        
    }

    public bool SetComponents()
    {
        if(gameManager == null) gameManager = FindObjectOfType<GameManager>();
        if (player == null) player = gameManager.playerAtivo.gameObject;
        if (playerCutscene == null) playerCutscene = player.GetComponent<PlayerCutsceneSpeedController>();
        if (inputController == null) inputController = player.GetComponent<Player>().playerController;
        if (playerRoot == null) playerRoot = player.transform.root.gameObject;

        if (player != null && playerRoot !=null && playerCutscene != null && inputController != null)
            return true;
        else return false;
        
    }

}
