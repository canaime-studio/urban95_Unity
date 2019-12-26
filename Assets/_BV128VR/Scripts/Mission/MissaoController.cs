using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissaoController : MonoBehaviour
{
    private bool isVR;
    public float tempoJogo = 100f;
    public GameObject mapaMissao;
    public Texture mapaMissaoImg;
    public AuxiliarMissao auxiliarMissao;


    public MissaoDetalhes[] missoes;
    public MissaoDetalhes missaoAtual;
    public VRAutoWalk PlayerVRAutoWalk;

    public GameManager gameManager;
    public AudioSource audio;

    public Player player;

    public bool carregarMissaoController;

    Interativo[] npcInterativos;


    void Awake()
    {
        if (auxiliarMissao == null) auxiliarMissao = FindObjectOfType<AuxiliarMissao>();

        isVR = GameManager.gameMode.Equals(GameMode.VR) ? true : false;
        if (isVR)
        {
            //this.gameObject.SetActive(true);
        }
        else if (!isVR)
        {
            //this.gameObject.SetActive(false);

        }        

        if (gameManager == null) gameManager = GameManager.FindObjectOfType<GameManager>();
        PlayerVRAutoWalk = gameManager.VRPlayer.GetComponent<VRAutoWalk>();
        //PlayerVRAutoWalk = GameObject.FindObjectOfType<VRAutoWalk>();

        if (audio == null) audio = GetComponent<AudioSource>();

        npcInterativos = FindObjectsOfType<Interativo>();
    }

    public void AtivarMissao()
    {
        carregarMissaoController = !carregarMissaoController;
        this.gameObject.SetActive(carregarMissaoController);
        foreach(Interativo i in npcInterativos)
        {
            i.PodeConversar(carregarMissaoController);
        }
    }


    public void Start()
    {
        if(missaoAtual.iniciarAltomaticamente)
        {
            missaoAtual.eventoAtivador();
        }
        player = gameManager.playerAtivo;

        // Atribui mapa da missao no elemento
        mapaMissao.GetComponentInChildren<RawImage>().texture = mapaMissaoImg;
    }
    public void FixedUpdate()
    {
        if (PlayerVRAutoWalk.moveForward == true)
        {
            player.canvas.enabled = false;
            //canvas.enabled = false;
        }
        missaoAtual.verificarStatusAssistente();

        if (tempoJogo > 0)
        {
            tempoJogo -= Time.deltaTime;
            gameManager.playerAtivo.cronometro.text = "" + (int)tempoJogo + "''";
        }

    }

    public void mudarMissao(int id)
    {
 
        missaoAtual.desativaAssistente();
        missaoAtual.ativa = false;

        missaoAtual = missoes[id];
        missaoAtual.ativa = true;

        if(player == null)
        {
            player = gameManager.playerAtivo;
        }

        if (GameManager.gameMode.Equals(GameMode.VR))
        {

            carregaInformacao(missaoAtual, player.canvas, player.painel, player.missaoTitulo, player.missaoDescricao, player.informacao_extra, player.imagemMissao, player.canvasPositionVR);
        }
        else if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            carregaInformacao(missaoAtual, player.canvas, player.painel, player.missaoTitulo, player.missaoDescricao, player.informacao_extra, player.imagemMissao, null);
            //carregaInformacao(missaoAtual, tpCanvas, tpPainel, tpMissaoTitulo, tpMissaoDescricao, tpInformacao_extra, tpImagem, canvasPosition);
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
            carregaInformacao(missaoAtual, player.canvas, player.painel, player.missaoTitulo, player.missaoDescricao, player.informacao_extra, player.imagemMissao, player.canvasPositionVR);
        }else if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            carregaInformacao(missaoAtual, player.canvas, player.painel, player.missaoTitulo, player.missaoDescricao, player.informacao_extra, player.imagemMissao, null);
            
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
        //Debug.Log("vamos testar o audio");

        canvas.enabled = true;
    }
    public void CloseCanvas()
    {
        //Debug.Log("Fechando");
    }
}
