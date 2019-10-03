using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissaoController : MonoBehaviour
{
    public MissaoDetalhes[] missoes;
    public MissaoDetalhes missaoAtual;
    public VRAutoWalk PlayerVRAutoWalk;

    public GameManager gameManager;
    public AudioSource audio;


    [Header("CanvasVR")]
    public Canvas canvas;
    public GameObject painel;
    public Text missaoTitulo;
    public Text missaoDescricao;
    public Text informacao_extra;
    public RawImage imagem;
    public GameObject canvasPosition;


    [Header("Canvas3P")]
    public Canvas tpCanvas;
    public GameObject tpPainel;
    public Text tpMissaoTitulo;
    public Text tpMissaoDescricao;
    public Text tpInformacao_extra;
    public RawImage tpImagem;
    



    void Awake()
    {
        if (gameManager == null) gameManager = GameManager.FindObjectOfType<GameManager>();
        PlayerVRAutoWalk = gameManager.VRPlayer.GetComponent<VRAutoWalk>();
        //PlayerVRAutoWalk = GameObject.FindObjectOfType<VRAutoWalk>();

        if (audio == null) audio = GetComponent<AudioSource>();
    }


    public void Start()
    {
        if(missaoAtual.iniciarAltomaticamente)
        {
            missaoAtual.eventoAtivador();
        }
    }
    public void FixedUpdate()
    {
        if (PlayerVRAutoWalk.moveForward == true)
        {
            canvas.enabled = false;
        }
        missaoAtual.verificarStatusAssistente();


    }

    public void mudarMissao(int id)
    {
 
        missaoAtual.desativaAssistente();
        missaoAtual.ativa = false;

        missaoAtual = missoes[id];
        missaoAtual.ativa = true;
        if (GameManager.gameMode.Equals(GameMode.VR))
        {
            carregaInformacao(missaoAtual, canvas, painel, missaoTitulo, missaoDescricao, informacao_extra, imagem, canvasPosition);
        }
        else if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            carregaInformacao(missaoAtual, tpCanvas, tpPainel, tpMissaoTitulo, tpMissaoDescricao, tpInformacao_extra, tpImagem, canvasPosition);
        }

        // carregaInformacao(missaoAtual, canvas, painel, missaoTitulo, missaoDescricao, informacao_extra, imagem, canvasPosition);




        //missaoTitulo.text = missaoAtual.descricao;
        //missaoDescricao.text = missaoAtual.objetivo;
        //informacao_extra.text = missaoAtual.informacaoExtra;
        //imagem.texture = missaoAtual.imagem;

        //painel.transform.position = canvasPostion.transform.position;
        //painel.transform.rotation = canvasPostion.transform.rotation;

        //canvas.enabled = true;

    }

    public void exibirMissao()
    {
        
        if (GameManager.gameMode.Equals(GameMode.VR))
        {
            carregaInformacao(missaoAtual, canvas, painel, missaoTitulo, missaoDescricao, informacao_extra, imagem, canvasPosition);
        }else if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            carregaInformacao(missaoAtual, tpCanvas, tpPainel, tpMissaoTitulo, tpMissaoDescricao, tpInformacao_extra, tpImagem, null);
        }

    }
    public void carregaInformacao(MissaoDetalhes missaoAtual, Canvas canvas,GameObject painel, Text missaoTitulo, Text missaoDescricao, Text informacao_extra,  RawImage imagem, GameObject canvasPosition )
    {
        if (GameManager.gameMode.Equals(GameMode.VR))
        {
            painel.transform.position = canvasPosition.transform.position;
            painel.transform.rotation = canvasPosition.transform.rotation;
        }
        missaoTitulo.text = missaoAtual.descricao;
        missaoDescricao.text = missaoAtual.objetivo;
        informacao_extra.text = missaoAtual.informacaoExtra;
        imagem.texture = missaoAtual.imagem;

        audio.clip = missaoAtual.audioDescricao;
        audio.Play();
        Debug.Log("vamos testar o audio");

        canvas.enabled = true;
    }
    public void CloseCanvas()
    {
        Debug.Log("Fechando");
    }
}
