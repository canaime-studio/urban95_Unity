using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(InformacaoPerosonagem), typeof(Animator))]
public class CharacterNPC : MonoBehaviour
{
    private GameManager gameManager;
    public MissaoController missaoController;
    private Animator animator;
    Transform minhaPosicao;
    private NavMeshAgent navMeshAgent;
    //public bool isFollowing;
    public bool seguirPlayer;


    private void Awake()
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        if (missaoController == null) missaoController = FindObjectOfType<MissaoController>();
        
       
    }

    private void FixedUpdate()
    {
        if (seguirPlayer)
        {
            float distancia = Vector3.Distance(minhaPosicao.transform.position, gameManager.playerAtivo.gameObject.transform.position);
            if (distancia > 4)
                AndarSeguirPlayer();
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 2)
                Idle();
        }
    }

    public void AndarSeguirPlayer()
    {
        navMeshAgent.isStopped = false;
        animator.SetInteger("status", 1);
        navMeshAgent.destination = gameManager.playerAtivo.transform.position;
    }
    public void Idle()
    {
        animator.SetInteger("status", 0);
        navMeshAgent.isStopped = true;
    }

    void Start()
    {
        // Informacao Player
        HideUI();
        playerNameUIText.text = playerName;

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        minhaPosicao = GetComponent<Transform>();
    }

    
    void Update()
    {
        if (playerNameUI.activeSelf)
        {
            playerNameUI.transform.LookAt(Camera.main.transform);
        }

    }


    #region Informacao Personagem
    public GameObject playerNameUI;
    public Text playerNameUIText;
    public string playerName;
    public string informacoes;

    public void ShowUI()
    {
        print("showUI");
        playerNameUI.gameObject.SetActive(true);
    }
    public void HideUI()
    {
        playerNameUI.gameObject.SetActive(false);
    }
    public void chamarCanvasMissao()
    {
        missaoController.exibirMissao();
    }
    #endregion

}
