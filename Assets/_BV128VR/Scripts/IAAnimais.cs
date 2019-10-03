using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IAAnimais : MonoBehaviour {
    public Animator animator;
    public int velocidadeAndar=1;
    public int velocidadeCorrer=2;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private float Cronometro=0;
    public int tempo=10;
    int i;
    float timePeso=7;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }


    void FixedUpdate()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            GotoNextPoint();
        }
        Cronometro += Time.deltaTime*timePeso;
        if(Cronometro>tempo)
        {
            proximaAção();
            Cronometro = 0;
        }



    }

    private void proximaAção()
    {
  
        i = UnityEngine.Random.Range(1, 4);
        if (i == 1)
        {
            comer();
        }
        else if (i == 2)
        {
            andar();
        }
        else if (i==3)
        {
            correr();
        }
    }

    private void andar()
    {
        timePeso = 0.7f;
        agent.speed = velocidadeAndar;
        animator.SetInteger("status", 1); 
    }

    private void correr()
    {
        timePeso = 2.5f;
        agent.speed = velocidadeCorrer;
        animator.SetInteger("status",-1);

    }

    private void comer()
    {
        timePeso = 1f;
        agent.speed = 0;
        animator.SetInteger("status", 0);

    }
}

