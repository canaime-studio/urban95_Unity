
using MobileVRInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePorta : MonoBehaviour {

    public GameManager gameManager;    

    public Animator abriPorta;
    public bool statusPorta=false;
    private Transform minhaPosicao;
    public GameObject player;
    float distancia;
    private AudioSource audioSource;
    public AudioPortas audiosPortas;
    public bool trancada=true;
    public int scoreNecessario=0;
    public VRInventoryExampleSceneController1 exampleController1;

    private npcPadrao scriptNPCPadrao;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = GameManager.FindObjectOfType<GameManager>();
    }

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        minhaPosicao = GetComponent<Transform>();
        
    }
    private void FixedUpdate()
    {
        distancia = Vector3.Distance(minhaPosicao.transform.position, gameManager.playerAtivo.transform.position);

        if (exampleController1.score >= scoreNecessario)
        {
            trancada = false;
        }
        else
        {
            trancada = true;
        }

        if(distancia>3 && statusPorta)
        {
            abriPorta.SetTrigger("Fechar");
            SomPortaFechando();
            statusPorta = false;
            gameManager.VRPlayer.GetComponent<Animator>().SetBool("OpenDoor", false);
        }


    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Personagens" )
        {
            scriptNPCPadrao=other.gameObject.GetComponent<npcPadrao>();
            if (scriptNPCPadrao.passandoPelaPorta == false)
            {             
                AbrirPortaNPC();
            }
        }
    }

    public void AbrirPortaNPC()
    {
        StartCoroutine(AnimacaoAbrirEFecharPortaNPC()); 
    }
    IEnumerator AnimacaoAbrirEFecharPortaNPC()
    {
        scriptNPCPadrao.passandoPelaPorta=true;

        SomPortaAbrindo();
        yield return new WaitForSeconds(1.0f);
        abriPorta.SetTrigger("Abrir");
        yield return new WaitForSeconds(2.5f);
      //  abriPorta.SetTrigger("Fechar");
        SomPortaFechando();
        yield return new WaitForSeconds(1.0f);

        abriPorta.SetTrigger("Fechar");

        yield return new WaitForSeconds(5.0f);

        scriptNPCPadrao.passandoPelaPorta = false;

    }

    public void AbrirPorta()
    {
        if(distancia<2 && !statusPorta&&!trancada)
        {
            Debug.Log("Pode Abrir a porta");
            StartCoroutine(AnimacaoAbrirPorta());
            
            statusPorta = true;
        }
    }
    IEnumerator AnimacaoAbrirPorta()
    {
        if(GameManager.gameMode.Equals(GameMode.VR))
        {
            gameManager.VRPlayer.GetComponent<VRAutoWalk>().podeAndar = false;
            gameManager.VRPlayer.GetComponent<VRAutoWalk>().moveForward = false;
            gameManager.VRPlayer.GetComponent<VRAutoWalk>().StopWalk();
            //player.GetComponent<VRAutoWalk>().podeExibirInventario = false;
            gameManager.VRPlayer.GetComponent<Animator>().SetBool("OpenDoor", true);

            SomPortaAbrindo();
            yield return new WaitForSeconds(1.0f);
            gameManager.VRPlayer.GetComponent<Animator>().SetBool("OpenDoor", false);
            abriPorta.SetTrigger("Abrir");
            yield return new WaitForSeconds(1.5f);
            gameManager.VRPlayer.GetComponent<VRAutoWalk>().podeAndar = true;
            //player.GetComponent<VRAutoWalk>().podeExibirInventario = true;
        }
        else if (GameManager.gameMode.Equals(GameMode.ThirdPerson))
        {
            SomPortaAbrindo();
            yield return new WaitForSeconds(1.0f);
            abriPorta.SetTrigger("Abrir");
            yield return new WaitForSeconds(1.5f);
        }

            
    }

    public void SomPortaAbrindo()
    {
        //AudioClip somPortaAbrindo = sonsPortasAbrindo[Random.Range(0, sonsPortasAbrindo.Length)];        
        AudioClip somPortaAbrindo = audiosPortas.sonsPortasAbrindo[Random.Range(0, audiosPortas.sonsPortasAbrindo.Length)];
        audioSource.clip = somPortaAbrindo;
        audioSource.Play();
    }
    public void SomPortaFechando()
    {
        //AudioClip somPortaFechando = sonsPortasFechando[Random.Range(0, sonsPortasFechando.Length)];
        AudioClip somPortaFechando = audiosPortas.sonsPortasFechando[Random.Range(0, audiosPortas.sonsPortasFechando.Length)];
        audioSource.clip = somPortaFechando;
        audioSource.Play();
    }
}
