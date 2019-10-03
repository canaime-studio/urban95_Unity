using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityStandardAssets.Characters.ThirdPerson;

public class TimelinePlaybackManager : MonoBehaviour
{

    [Header("Timeline References")]
    public PlayableDirector playableDirector;

    [Header("Timeline Settings")]
    public bool playTimelineOnlyOnce;

    [Header("Player Input Settings")]
    public KeyCode interactKey;
    public bool disablePlayerInput = false;
    //public Component inputController;

    [Header("Player Timeline Position")]
    public bool setPlayerTimelinePosition = false;
    public Transform playerTimelinePosition;

    [Header("Trigger Zone Settings")]
    public GameObject triggerZoneObject;

    [Header("UI Interact Settings")]
    public bool displayUI;
    public GameObject interactDisplay;

    [Header("Player Settings")]

    private bool playerInZone = false;
    public bool timelinePlaying = false;
    private float timelineDuration;

    //public GameManager gameManager;

    public Camera cameraCutscene;
    public MissaoDetalhes missao;

    public Transform playerPosition;
    public Transform _exitPlayerPosition;


    public CinematicController cinematicController;


    private void Awake()
    {
        
        
    }

    void Start()
    {


        //if (playerObject == null) GetPlayerObjetc();

        //inputController = cinematicController.player.GetComponent<Pla>
        //inputController = gameManager.playerAtivo.playerController;
        //inputController = playerObject.GetComponent<ThirdPersonUserControl>();

        //playerCutsceneSpeedController = playerObject.GetComponent<PlayerCutsceneSpeedController>();
        cinematicController.SetComponents();
        ToggleInteractUI(false);

        /** Desativa timeline para jogabilidade em VR
        if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            playerObject = GameObject.FindWithTag(playerTag);
            inputController = playerObject.GetComponent<ThirdPersonUserControl>();
            playerCutsceneSpeedController = playerObject.GetComponent<PlayerCutsceneSpeedController>();
            ToggleInteractUI(false);
        } else
        {
            this.gameObject.SetActive(false);            
        }
        */
        
    }

    public void PlayerEnteredZone()
    {
        playerInZone = true;
        Debug.Log("Na zona");
        ToggleInteractUI(playerInZone);
    }

    public void PlayerExitedZone()
    {

        playerInZone = false;

        ToggleInteractUI(playerInZone);

    }

    void Update()
    {

        if (playerInZone && !timelinePlaying)
        {

            var activateTimelineInput = Input.GetKey(interactKey);
            Debug.Log(interactKey);
            Debug.Log(activateTimelineInput);
            if (interactKey == KeyCode.None)
            {
                Debug.Log("Interagir");
                PlayTimeline();
            }
            else
            {
                if (activateTimelineInput)
                {
                    Debug.Log("descobrir");
                    PlayTimeline();
                    ToggleInteractUI(false);
                }
            }

        }

    }

    public void PlayTimeline()
    {
        //if (playerObject == null) GetPlayerObjetc();

        if (setPlayerTimelinePosition)
        {
            SetPlayerToTimelinePosition();
        }

        if (playableDirector)
        {
            Debug.Log("Tocando Timeline");
            if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
            {
                cameraCutscene.enabled = true;
            }
            if(cinematicController.player == null) cinematicController.SetComponents();

            cinematicController.player.GetComponent<Player>().EnabledCanvas(false);
            if (GameManager.gameMode == GameMode.VR)
                cinematicController.player.GetComponent<VRAutoWalk>().enabled = false;

            playableDirector.Play();

        }
        triggerZoneObject.SetActive(false);
        timelinePlaying = true;
        StartCoroutine(WaitForTimelineToFinish());        

    }

    IEnumerator WaitForTimelineToFinish()
    {
        timelineDuration = (float)playableDirector.duration;

        ToggleInput(false);
        yield return new WaitForSeconds(timelineDuration);
        ToggleInput(true);


        if (!playTimelineOnlyOnce)
        {
            triggerZoneObject.SetActive(true);
        }
        else if (playTimelineOnlyOnce)
        {
            playerInZone = false;
        }

        // Final
        timelinePlaying = false;
        cameraCutscene.enabled = false;
        this.gameObject.SetActive(false);
        ExitTimeLine();

        
        //playerObject.GetComponent<Player>().canvasObject.GetComponent<Canvas>().enabled = true;
    }

    void ToggleInput(bool newState)
    {
        if (disablePlayerInput)
        {
            cinematicController.playerCutscene.SetPlayerSpeed();            
            //inputController.inputAllowed = newState;
        }
    }


    void ToggleInteractUI(bool newState)
    {
        if (displayUI)
        {
            interactDisplay.SetActive(newState);
        }
    }

    void SetPlayerToTimelinePosition()
    {
        if (GameManager.gameMode == GameMode.VR)
        {
            if(cinematicController.player == null)
            {
                cinematicController.SetComponents();
            }

            cinematicController.player.transform.position = playerTimelinePosition.position;


            cinematicController.player.transform.position = playerTimelinePosition.transform.position;
            cinematicController.player.transform.rotation = playerTimelinePosition.transform.rotation;
            cinematicController.player.transform.SetParent(playerTimelinePosition.transform);


            cinematicController.player.GetComponent<VRAutoWalk>().enabled = false;
            //playerObject.gameObject.GetComponent<CharacterController>().enabled = false;
            cinematicController.player.transform.localRotation = playerTimelinePosition.rotation;
        }
        
    }

    public void ExitTimeLine()
    {
        Debug.Log("ExitTimeLine");
        if (setPlayerTimelinePosition)
        {
            if (cinematicController.playerRoot != null)
            {
                cinematicController.player.transform.SetParent(cinematicController.playerRoot.transform);
                cinematicController.player.transform.position = _exitPlayerPosition.transform.position;
                cinematicController.player.transform.rotation = new Quaternion(0, 0, 0, 0);
                cinematicController.player.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        

        if (GameManager.gameMode == GameMode.ThirdPerson)
        {
            cinematicController.player.GetComponent<Player>().EnabledCanvas(true);
        } else if (GameManager.gameMode == GameMode.VR)
        {
            cinematicController.player.gameObject.GetComponent<VRAutoWalk>().enabled = true;
        }
    }

    public void GetPlayerObjetc()
    {
        //playerObject = gameManager.playerAtivo.gameObject;
        
        ////playerObject = GameObject.FindWithTag(playerTag);
        //playerRoot = playerObject.transform.root.gameObject;

        //print("Configurando Player: "+playerObject);
    }
}
