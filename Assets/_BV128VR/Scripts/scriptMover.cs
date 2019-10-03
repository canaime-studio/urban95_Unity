using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMover : MonoBehaviour {
    public GameObject player;
    VRAutoWalk scriptAnda;
    Transform esseAlvo;
    public MeshRenderer render;
    private Transform minhaPosicao;
    SphereCollider collider;

    // Use this for initialization
    void Start () {
        scriptAnda = player.GetComponent<VRAutoWalk>();
        esseAlvo = GetComponent<Transform>();
        minhaPosicao = GetComponent<Transform>();
        collider = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);

        if (distancia < 13)
        {
            collider.enabled = true;
            ativarRender();
        }
        else
        {
            collider.enabled = false;

            desativarRender();
        }

    }
    public void moverParaK()
    {
        scriptAnda.alvo = esseAlvo;
    }
    public void ativarRender()
    {
        render.enabled=true;
    }    public void desativarRender()
    {
        render.enabled=false;
    }
}
