using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Fases : MonoBehaviour {
    //public string fase;   

    public MissaoController mc;
    public Text txt_BotaoMissao;
    public GameObject mapaMissoes;

    public void Awake()
    {
        
    }
    public void Start()
    {
        if(mc == null)
        {
            mc = FindObjectOfType<MissaoController>();
            if (mc == null)
            {
                //Debug.LogError("Missao Controller Nao encontrado");
                this.gameObject.SetActive(false);
                return;
            }
        }
        mapaMissoes = mc.mapaMissao;

        if(mapaMissoes != null)
        {
            mapaMissoes.SetActive(false);
        }
        

    }
    public void CarregarFase(string fase)
    {
        SceneManager.LoadScene(fase);
    }
    public void ExibirMapa()
    {
        //if(mapaMissoes == null)
        //{
        //    mapaMissoes = GameObject.Find("MapaFase");
        //}
        mapaMissoes.SetActive(true);
    }
    public void AtivarMissoes()
    {
        if (mc == null)
        {
            mc = (MissaoController) FindObjectOfType(typeof(MissaoController));
            //mc = FindObjectOfType<MissaoController>();
        }
        if (mc != null)
        {
            mc.GetComponent<MissaoController>().AtivarMissao();
            if (mc.carregarMissaoController)
            {
                txt_BotaoMissao.text = "Desativar Missões";
                
            } else
            {
                txt_BotaoMissao.text = "Ativar Missões";
            }
        }
    }
}
