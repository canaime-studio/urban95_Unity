using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformativoUI : MonoBehaviour {
    public GameObject player;
    private Transform minhaPosicao;
    public InformationScript informativoAtual;
    public float distancia;

    public VRAutoWalk PlayerVRAutoWalk;

    public bool canvasEnabled;

    //UI ELEMENTS
    //public Texture texturaInformativa;
    [Header("CanvasVR")]
    public Canvas canvas;
    public Text GUI_TextoDescricao;
    public Text GUI_TextoDescricaoPlus;
    public Text GUI_TextoTitulo;
    public RawImage GUI_imgRaw;
    public GameObject canvasPostion;

    [Header("CanvasTP")]
    public Text tpGUI_TextoDescricao;
    public Text tpGUI_TextoDescricaoPlus;
    public Text tpGUI_TextoTitulo;
    public RawImage tpGUI_imgRaw;
    public Canvas tpCanvas;

    void Awake()
    {        
        PlayerVRAutoWalk = GameObject.FindObjectOfType<VRAutoWalk>();
    }
	// Use this for initialization
	void Start () {
        minhaPosicao = GetComponent<Transform>();
        canvasEnabled = false;
        //player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);

        //Vector3 direcao = player.transform.position - transform.position;
        //Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        //GetComponent<Rigidbody>().MoveRotation(novaRotacao);
        //float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);

        //Debug.Log(distancia);
        //Debug.Log(distancia);
        if (GameManager.gameMode.Equals(GameMode.VR)) {
            if (PlayerVRAutoWalk.moveForward == true)
            {
                canvas.enabled = false;
            }
            else
            {
                //CloseInformation();
            }

        }


        


    }

    public void OpenCanvasInformation()
    {
     /*   if (distancia < 10.0f && distancia > 3.0f)
        {
            this.transform.position = canvasPostion.transform.position;
            this.transform.rotation = canvasPostion.transform.rotation;
            canvas.enabled = true;
        }*/
        this.transform.position = canvasPostion.transform.position;
        this.transform.rotation = canvasPostion.transform.rotation; 
        canvas.enabled = true;
    }


    public void CloseInformation()
    {
        //PlayerVRAutoWalk.parar();
        //PlayerVRAutoWalk.FecharInformacao(); 
        //Debug.LogError("Desativar");
        canvas.enabled = false;
        
        //informativoAtual.gameObject.SetActive(true);
        //canvasEnabled = false;
        //player.GetComponent<VRAutoWalk>().moveForward = false;
        //player.GetComponent<VRAutoWalk>().enabled = true;

        //gameObject.SetActive(false);
    }
    void PlayerAndar(bool andar)
    {

        if (andar)
        {

            //Debug.LogError("Pode Andar: " + reticleDistanceInMeters);
            PlayerVRAutoWalk.podeAndar = true;
        }
        else
        {
            // Debug.LogError("Nao pode andar agora" + reticleDistanceInMeters);
            PlayerVRAutoWalk.podeAndar = false;
            PlayerVRAutoWalk.moveForward = false;
        }

    }

}


