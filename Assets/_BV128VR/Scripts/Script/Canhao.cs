using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canhao : MonoBehaviour {

    public GameObject balaCanhao;
    public Transform spawnBala;
    public Transform EixoCanhao;
    
    private AudioSource audioSource;
    public AudioClip[] clips;

    // Tiro Automatico
    public float delayTiroAutomatico;

    // Intervalo Tiro Player
    public float proxTiro = 2.0f;
    public float fireRate;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        //InvokeRepeating("Atirar", delayTiroAutomatico, fireRate);
    }

    public void AtirarAutomatico()
    {
        //print("Atirando");
        Instantiate(balaCanhao, spawnBala.position, spawnBala.rotation);        
        SomCanhao();
    }

    public void SomCanhao()
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void AtirarPlayer()
    {
        
        if(Time.time > proxTiro)
        {
            Debug.Log("Atirar");
            proxTiro = Time.time + fireRate;
            Instantiate(balaCanhao, spawnBala.position, spawnBala.rotation);          
            SomCanhao();
        } else
        {
            Debug.Log("Carregando");
        }
        
    }
    
}
