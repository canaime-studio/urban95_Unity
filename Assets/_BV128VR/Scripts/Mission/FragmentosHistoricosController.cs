using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EventoHistorico
{
    public string titulo, descricao, ano, contextoHistorico;
    public Texture imagem;
    public AudioClip audioDescricao;
    public Transform posicao;
    public bool fixo;
}
[RequireComponent(typeof(AudioSource))]
public class FragmentosHistoricosController : MonoBehaviour
{ 
    public int qtdColetado;
    public int qtdTotal;
    public int pontos;

    private FragmentoHistoricoUI GUI_Fragmento;
    public FragmentoHistoricoUI fragmentoUIPrefab;

    //public Transform fragmentoTransform;

    

    public FragmentoHistoricoObjeto[] fragmento;
    public List<Transform> posicoesFragmentos;
    public int qtdFragmentos;
    //public List<string> descricaoFragmentos;
    public List<EventoHistorico> eventoHistoricos;

    public AudioSource audio;
    private GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        if (eventoHistoricos.Count == 0)
        {
            Debug.LogError("Não Existem Eventos Cadastrados");
        } else
        {
            if (GameManager.gameMode == GameMode.ThirdPerson)
            {
                Player player = gameManager.playerAtivo;

                GUI_Fragmento = Instantiate(fragmentoUIPrefab, player.GetComponent<PlayerUI>().rootGameUI);

                if (audio == null) audio = GetComponent<AudioSource>();

                GUI_Fragmento.painelFragmentos.SetActive(false);
                var fragPos = GetComponentsInChildren<FragmentoContainer>();
                foreach (var frag in fragPos)
                {
                    if(!frag.fixo)
                    {
                        posicoesFragmentos.Add(frag.gameObject.transform);
                    }
                }
                //posicoesFragmentos = GetComponentsInChildren<Transform>();
                Debug.LogError(posicoesFragmentos.Count + " Disponiveis");
                foreach (EventoHistorico evento in eventoHistoricos)
                {
                    int tipoFragmento = Random.Range(0, fragmento.Length);
                    
                    //int indexDescricao = Random.Range(0, eventoHistoricos.Count);

                    

                    var fragmentoItem = new GameObject();

                    if (evento.fixo)
                    {
                        fragmentoItem = Instantiate(fragmento[tipoFragmento].gameObject, evento.posicao);
                    }
                    else
                    {
                        int posicaoFramengo = Random.Range(0, posicoesFragmentos.Count);
                        Debug.Log("Posicao Sorteada: " + posicaoFramengo);
                        fragmentoItem = Instantiate(fragmento[tipoFragmento].gameObject, posicoesFragmentos[posicaoFramengo]);
                        posicoesFragmentos.RemoveAt(posicaoFramengo);
                    }

                    

                    fragmentoItem.GetComponent<FragmentoHistoricoObjeto>().descricao = evento.descricao;
                    fragmentoItem.GetComponent<FragmentoHistoricoObjeto>().ano = evento.ano;
                    fragmentoItem.GetComponent<FragmentoHistoricoObjeto>().contextoHistorico = evento.contextoHistorico;
                    fragmentoItem.GetComponent<FragmentoHistoricoObjeto>().titulo = evento.titulo;
                    fragmentoItem.GetComponent<FragmentoHistoricoObjeto>().imagem = evento.imagem;
                    fragmentoItem.GetComponent<FragmentoHistoricoObjeto>().audioDescricao = evento.audioDescricao;
                    fragmentoItem.gameObject.name = evento.titulo;

                    //Debug.LogError("Removendo Posicao: " + posicoesFragmentos[posicaoFramengo].name + " | Agora tem: " + posicoesFragmentos.Count);
                    //posicoesFragmentos.Remove(posicoesFragmentos[posicaoFramengo]);
                    
                }
                //for (int i = 0; i < eventoHistoricos.Count; i++)
                //{
                //    //int tipoFragmento = Random.Range(0, fragmento.Length);
                //}
                GUI_Fragmento.TxtProntos.text = "0/" + qtdFragmentos;
            }
        }        
    }

    public void ColetarFragmento(GameObject fragmento)
    {
        var f = fragmento.GetComponent<FragmentoHistoricoObjeto>();
        pontos = pontos + f.ptsRecompensa;
        StartCoroutine(painelInformativo(f, true, pontos.ToString(), ""));
    }


    IEnumerator painelInformativo(FragmentoHistoricoObjeto fragmento, bool ativo, string pontuacao, string coracao)
    {
        GUI_Fragmento.painelFragmentos.SetActive(ativo);
        GUI_Fragmento.TxtProntos.text = pontuacao +"/"+qtdFragmentos;
        GUI_Fragmento.txt_titulo.text = fragmento.titulo;
        GUI_Fragmento.txt_ano.text = fragmento.ano;
        GUI_Fragmento.txt_descricao.text = fragmento.descricao;
        GUI_Fragmento.txt_contexto.text = fragmento.contextoHistorico;
        GUI_Fragmento.imagem.texture = fragmento.imagem;
        audio.clip = fragmento.audioDescricao;
        audio.Play();
        yield return new WaitForSeconds(40);
        GUI_Fragmento.painelFragmentos.SetActive(false);
    }
}
