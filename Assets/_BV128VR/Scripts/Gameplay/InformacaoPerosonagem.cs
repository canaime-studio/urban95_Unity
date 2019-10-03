using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CapsuleCollider), typeof(EventTrigger))]
public class InformacaoPerosonagem : MonoBehaviour
{

    public GameObject playerNameUI;
    public Text playerNameUIText;
    public string playerName;
    public string informacoes;
    public bool chamarInformacaoMissao;

    void Start()
    {
        HideUI();
        playerNameUIText.text = playerName;   
    }

    private void Awake()
    {
        // Tira a necessidade de Registrar no unity o event trigger
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    // Chama o Event Trigger
    public void OnPointerClick(PointerEventData data)
    {
        if(chamarInformacaoMissao)
        {
            GetComponent<CharacterNPC>().chamarCanvasMissao();
        } else
        {

        }
        
    }


    void Update()
    {
        if(playerNameUI.activeSelf)
        {
            playerNameUI.transform.LookAt(Camera.main.transform);
        }
        
    }
    public void ShowUI()
    {
        playerNameUI.gameObject.SetActive(true);
    }
    public void HideUI()
    {
        playerNameUI.gameObject.SetActive(false);
    }
}
