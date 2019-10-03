using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acompanhante : MonoBehaviour {

    public GameManager gameManager;
    public MissaoController controleMissao;
    public Animator animator;
    Transform minhaPosicao;
    public UnityEngine.AI.NavMeshAgent navMesh;
    public bool estaSeguindo;

    // Start is called before the first frame update
    private void Awake()
    {
        if (gameManager == null)
            gameManager = GameManager.FindObjectOfType<GameManager>();
    }

    void Start()
    {
        minhaPosicao = GetComponent<Transform>();
        navMesh = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distancia = Vector3.Distance(minhaPosicao.transform.position, gameManager.playerAtivo.gameObject.transform.position);      
        if ((distancia > 4) && (estaSeguindo == true))
        {
            andar();
        }
        if (!navMesh.pathPending && navMesh.remainingDistance < 2f)
        {
            idle();
        }
    }
    private void andar()
    {
        navMesh.isStopped = false;
        animator.SetInteger("status", 1);
        navMesh.destination = gameManager.playerAtivo.transform.position;
    }
    private void idle()
    {
        animator.SetInteger("status", 0);
        navMesh.isStopped = true;
    }

    public void chamarCanvasMissao()
    {
        controleMissao.exibirMissao();
    }
}
