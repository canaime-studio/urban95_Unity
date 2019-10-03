using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissaoDetalhes : MonoBehaviour
{
    public int ID;
    public int sequencia;
    public bool ativa;
    [TextArea]
    public string descricao;
    [TextArea]
    public string objetivo;
    [TextArea]
    public string informacaoExtra;
    public int recompensa;
    public int requisitos;
    public Texture imagem;

    public CharacterNPC assistente;
    //public Acompanhante assistente;
 //   public Transform destinoNpc;
    public npcPadrao NPCPadrao;

    public AudioClip audioDescricao;
    

    


    //public Object missao;

    public MissaoController missaoController;

    public bool iniciarAltomaticamente;
    public MissaoRondaCheckPoints missao_checkpoints;

    public TimelinePlaybackManager timelinePlaybackManager;

    IEnumerator PlayCinematic()
    {
        if (timelinePlaybackManager != null)
        {
            timelinePlaybackManager.gameObject.SetActive(true);
            timelinePlaybackManager.PlayTimeline();
            Debug.Log("Comecou a tocar");
            yield return new WaitWhile(() => timelinePlaybackManager.timelinePlaying);
            Debug.Log("Parou de tocar");
        }

        missao_checkpoints.IniciaMissao();
        missaoController.mudarMissao(ID);
        if (NPCPadrao != null)
        {
            NPCPadrao.moverNPC();

        }
    }
    public void eventoAtivador()
    {
        StartCoroutine(PlayCinematic());

        
        
        
    }

    public void verificarStatusAssistente()
    {
        if (ativa == true)
        {
            if (assistente != null)
            {
                assistente.seguirPlayer = true;
            }


        }
    }
    public void desativaAssistente()
    {
        if (assistente != null)
        {
            assistente.seguirPlayer = false;
        }
    }
}
