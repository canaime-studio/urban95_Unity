using MobileVRInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBegin : MonoBehaviour
{
    public bool iniciada = false;
    public GameObject player;
    Transform minhaPosicao;
    public MissaoRondaCheckPoints scriptMRCP;
    public VRInventoryExampleSceneController1 exampleController1;
    private AudioSource m_AudioSource;
    public AudioClip missao01;

    public MissaoController msc;
    public MissaoDetalhes md;

    void Start()
    {
        minhaPosicao = GetComponent<Transform>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void iniciarQuest()
    {
        if (msc.missaoAtual.ID == md.ID)
        {
            float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);
            if (distancia < 3 && iniciada == false)
            {
                m_AudioSource.PlayOneShot(missao01);
                exampleController1.ShowMessage("Saudações recruta, faça a ronda no forte!!");
                iniciada = true;
                scriptMRCP.IniciaMissao();
            }
        }
        
    }    
}



