using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Use this for initialization
    public GameManager gameManager;
    //public GameMode gm;
    public GameObject PrefabPlayer;
    public Component playerController;
    public GameObject verificarButton;
    public GameObject conversarButton;
    public GameObject inspencionado;
    public CharacterNPC acompanhante;
    public InformationItem information;

    public MonoBehaviour[] componentsPlayerDisable;
    public MonoBehaviour[] componentsPlayerEnable;
    public GameObject cameraObject;
    public Transform cameraPivot;

    public GameObject canvasObject;
    public Transform canvasPosition;
    

    public Rigidbody rb;
    public CharacterController characterController;
    public Vehicle veiculoDisponivel;

    public VehiclePlayerControl vehicleControl;
    public bool serSeguido;

    public bool driving;

    //private ControllerRidingAnimal controllerRidingAnimal;
    //private AnimalControlavel animalControlavel;


    public Canvas canvas;
    public GameObject painel;
    public Text missaoTitulo;
    public Text missaoDescricao;
    public Text informacao_extra;
    public RawImage imagemMissao;
    public GameObject canvasPositionVR;
    public Text cronometro;

    [Header("CanvasPrefabs")]
    public GameObject canvasInformacoesNPC;


    private void OnTriggerEnter(Collider collider)
    {
        if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            if (collider.transform.tag == "ItemInformativo")
            {
                verificarButton.SetActive(true);
                inspencionado = collider.GetComponent<Interativo>().objetoInterativo.gameObject;
            }

            if (collider.transform.tag == "Interativo")
            {
                conversarButton.SetActive(true);
                inspencionado = collider.GetComponent<Interativo>().objetoInterativo.gameObject;
            }
        }
        if (collider.transform.tag == "VeiculoControlavel")
        {            
            veiculoDisponivel = collider.gameObject.GetComponent<Vehicle>();            
            vehicleControl.vehicle = veiculoDisponivel;
        }

        if (collider.transform.tag == "montavel")
        {
          //  Debug.LogError("contato!!");
            //animalControlavel = collider.gameObject.GetComponent<AnimalControlavel>();
            //animalControlavel.contato = true;

        }



    }
    private void OnTriggerExit(Collider collider)
    {
        if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            if (collider.transform.tag == "ItemInformativo")
            {
                verificarButton.SetActive(false);
                inspencionado = null;
            }
            if (collider.transform.tag == "Interativo")
            {
                conversarButton.SetActive(false);
                inspencionado = null;
            }
        }
        if (collider.transform.tag == "VeiculoControlavel")
        {
            veiculoDisponivel = null;
            vehicleControl.vehicle = null;
        }

        if (collider.transform.tag == "montavel")
        {
            //animalControlavel.contato = false;
            //animalControlavel = null;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            if (collision.transform.tag == "Porta")
            {
                collision.gameObject.GetComponent<ControlePorta>().AbrirPorta();
                Debug.Log("Porta");
            }
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        vehicleControl = GetComponent<VehiclePlayerControl>();
    }
    private void Start()
    {
        switch (GameManager.gameMode)
        {
            case GameMode.VR:
                playerController = GetComponent<VRAutoWalk>();
                break;
            case GameMode.ThirdPerson:
                playerController = GetComponent<ThirdPersonUserControl>();
                break;
            case GameMode.FirstPerson:
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        //DirigirCarroca();
    }
    public void EnableComponents(bool enabled, Rigidbody rb) //false
    {
        characterController = GetComponent<CharacterController>();

        if (rb != null)
        {
            rb.detectCollisions = enabled;
            rb.GetComponent<Rigidbody>().isKinematic = !enabled;
            gameObject.GetComponent<CapsuleCollider>().enabled = enabled;
        }
        if (characterController != null)
        {
            characterController.enabled = enabled;
        }
        else
        {
            //Debug.Log("Charactrer vazio - VR");
        }

        if (playerController == null)
        {
            switch (GameManager.gameMode)
            {
                case GameMode.VR:
                    playerController = GetComponent<VRAutoWalk>();
                    break;
                case GameMode.ThirdPerson:
                    playerController = GetComponent<ThirdPersonUserControl>();
                    break;
                case GameMode.FirstPerson:
                    break;
                default:
                    break;
            }
        }
        playerController.SendMessage("isEnable", enabled);

        if (componentsPlayerDisable != null)
        {
            foreach (var c in componentsPlayerDisable)
            {
                c.SendMessage("isEnable", enabled);
                //c.GetComponent<MonoBehaviour>().enabled = active;
            }
        }
        if (componentsPlayerEnable != null)
        {
            foreach (var c in componentsPlayerEnable)
            {
                c.SendMessage("isEnable", enabled);
                //c.GetComponent<MonoBehaviour>().enabled = !enabled;
            }
        }
    }

    #region Comportamentos
    public void AbrirPorta()
    {

    }
    public void Parar()
    {
        //Debug.LogError("Mensagem de Erro");
        playerController.SendMessage("StopWalk");
    }
    public void Andar()
    {

    }
    public void Correr()
    {

    }
    public void Inspecionar()
    {
        //  Debug.LogError("interagindo asdasd");

        if (inspencionado.transform.tag == "Personagens")
        {
            //      Debug.LogError("interagindo personagem");
            acompanhante = inspencionado.GetComponent<CharacterNPC>();

            if(acompanhante != null)
            {
                acompanhante.chamarCanvasMissao();
            } else
            {
                var npcConversar = inspencionado.GetComponent<InformacaoPerosonagem>();
                if(npcConversar != null)
                {
                    npcConversar.Conversar();
                }
            }
            
            
        }
        else if (inspencionado.transform.tag == "ItemInformativo")
        {
            information = inspencionado.GetComponent<InformationItem>();
            information.ExibirInformativoTP();
        }




    }


    public void Sentar(bool isSitting)
    {
        //Debug.Log("Sentado");
        if (playerController != null)
        {
            playerController.GetComponent<Animator>().SetBool("Sentar", isSitting);
            playerController.SendMessage("Sentado", isSitting);
        }
        else
        {
            Debug.Log("Nulo");
        }
    }

    public void ExitWagon()
    {

    }

    public void DirigirCarroca()
    {
    }

    public void SairCarroca()
    {        
    }

    public void EnabledCanvas(bool enabled)
    {
        if (canvasObject != null)
        {
            if (canvasObject.GetComponent<Canvas>())
            {
                canvasObject.GetComponent<Canvas>().enabled = enabled;
            }
            else
            {
                //Debug.LogError("Nao tem canvas");
            }
            
        }        
    }
    #endregion

    public void SetPositionCanvasVR(Transform target)
    {
        canvasObject.transform.SetParent(target);
        canvasObject.transform.localRotation = Quaternion.Euler(new Vector3(0, -46.5f, 0));
        canvasObject.transform.localPosition = new Vector3(-1, -0.45f, 1.45f);
    }

    //public void MontarAnimal()
    //{
    //    animalControlavel.Drive();
    //}

    //public void CaronaAnimal()
    //{
    //    animalControlavel.GetRide();
    //}

    //public void DesmontarAnimal()
    //{
    //    animalControlavel.Exit();
    //}
    //public void ControlarVR()
    //{
    //    if(animalControlavel != null) animalControlavel.ControlsVR();
    //}
}


