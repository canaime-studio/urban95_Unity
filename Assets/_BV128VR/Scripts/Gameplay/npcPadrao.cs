using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcPadrao : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent navMesh;
    //public GameObject Player;
    Transform minhaPosicao;
    public Animator animator;
    public Transform destinoNpc;
    public bool andando;
    public Rigidbody rigidbody;
    public bool passandoPelaPorta;
    public GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = GameManager.FindObjectOfType<GameManager>();
    }

    void Start()
    {
        minhaPosicao = GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        float distancia = Vector3.Distance(minhaPosicao.transform.position, gameManager.playerAtivo.gameObject.transform.position);

        if (distancia > 4 && andando==false)
        {
            transform.LookAt(gameManager.playerAtivo.gameObject.transform);
        }
        if (!navMesh.pathPending && navMesh.remainingDistance < 3.5f)
        {
            idle();      
        }
        if (andando == true)
        {
            rigidbody.isKinematic = false;
        }
        else
        {
            rigidbody.isKinematic = true;
        }
    }

    private void idle()
    {
        andando = false;
        animator.SetInteger("status", 0);
    }

    public void moverNPC()
    {
        if (destinoNpc != null)
        {
            andando = true;
            navMesh.isStopped = false;
            animator.SetInteger("status", 1);
            navMesh.destination = destinoNpc.position;
        }
    }
}
