using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScene : MonoBehaviour
{
    
    public  GameObject setupScene, player3Pessoa, playerVR, UI;
    // Start is called before the first frame update
    public GameManager gameManager;

    public GameObject player3PModel;
    public Avatar player3PAvatar;

    public Transform playerStartPosition;


    public float alturaPlayer;
    private void Awake()
    {
        setupScene.transform.SetParent(null);
        player3Pessoa.transform.SetParent(null);
        player3Pessoa.transform.position = playerStartPosition.position;


        playerVR.transform.SetParent(null);
        playerVR.transform.position = playerStartPosition.position;

        UI.transform.SetParent(null);

        if(gameManager == null) gameManager = FindObjectOfType<GameManager>();

        Instantiate(player3PModel, gameManager.ThirdPersonPlayer.gameObject.transform);
        gameManager.ThirdPersonPlayer.GetComponent<Animator>().avatar = player3PAvatar;

        //VR
        var player = Instantiate(player3PModel, gameManager.VRPlayer.gameObject.transform);
        player.transform.localPosition = new Vector3(0,-0.1f,0);
        gameManager.VRPlayer.GetComponent<CharacterController>().height = alturaPlayer;
        gameManager.VRPlayer.GetComponent<CharacterController>().center = new Vector3(0, alturaPlayer/2,0);
        gameManager.VRPlayer.GetComponent<NvrBody>().cameraHeightOffset = alturaPlayer-0.15f;
        
        

        gameManager.VRPlayer.GetComponent<Animator>().avatar = player3PAvatar;
    }

    private void Start()
    {
        
        //gameManager.ThirdPersonPlayer
    }
}
