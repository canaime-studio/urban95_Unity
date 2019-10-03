using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationScript : MonoBehaviour {
    
    public GameObject player;
    public InformativoUI dashBoard;
    Transform minhaPosicao;

    public Texture texturaInformativa;
    public string txtTitulo;
    public string txtDescricao;    

    public Text GUI_TextoDescricao;
    public Text GUI_TextoTitulo;
    public RawImage GUI_imgRaw;

    public Canvas canvas;

    void Start()
    {
        minhaPosicao = GetComponent<Transform>();
    }
    
    void FixedUpdate()
    {
        Vector3 direcao = player.transform.position - transform.position;
        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        //GetComponent<Rigidbody>().MoveRotation(novaRotacao);
        if (dashBoard.canvasEnabled == true) {
            /*
            if (player.GetComponent<VRAutoWalk>().enabled == true)
            {
                //Debug.Log("Ativo");
                player.GetComponent<VRAutoWalk>().enabled = false;
            } else
            {
                //Debug.Log("Desativado");
            }
            */
            
        }
    }

    public void ativarDash()
    {
        float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);
        if (distancia < 6)
        {
            player.GetComponent<VRAutoWalk>().moveForward = false;
            player.GetComponent<VRAutoWalk>().enabled = false;
            //Instantiate(dashBoard, transform.position, transform.rotation);

            dashBoard.transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
            dashBoard.gameObject.SetActive(true);
            //dashBoard.enabled = true;
            canvas.gameObject.SetActive(true);

            GUI_imgRaw.GetComponent<RawImage>().texture = texturaInformativa;
            GUI_TextoTitulo.text = txtTitulo;
            GUI_TextoDescricao.text = txtDescricao;
            dashBoard.canvasEnabled = true;
            
            // Esconde Informativo
            dashBoard.informativoAtual = this;
            gameObject.SetActive(false);

        }
        
    }

}


