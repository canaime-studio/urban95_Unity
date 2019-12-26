using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentoHistoricoObjeto : MonoBehaviour
{

    public FragmentosHistoricosController fragmentoController;
    public int valor;
    public int ptsRecompensa;
    public string titulo, descricao, ano, contextoHistorico;
    public Texture imagem;
    public AudioClip audioDescricao;


    private void Start()
    {
        if (fragmentoController == null)
        {
            fragmentoController = GetComponentInParent<FragmentosHistoricosController>();
            //fragmentoController = FindObjectOfType<FragmentosHistoricosController>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ColetarFragmento();
        }
    }

    public void ColetarFragmento()
    {
        fragmentoController.ColetarFragmento(gameObject);
        Destroy(gameObject);
    }


}
