using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Interativo), typeof(SphereCollider), typeof(EventTrigger))]

public class InformationItem : MonoBehaviour
{
    public Texture img;
    [TextArea]
    public string title;
    [TextArea]
    public string description;
    [TextArea]
    public string description_plus;
    public InformativoUI ui;
    public InformativoUI ui3p;
    public bool chamarInformacaoMissao;
    public AudioClip audioDescricao;



    private void Awake()
    {
        // Tira a necessidade de Registrar no unity o event trigger
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        //print("Preparando Event");
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);

        gameObject.tag = "ItemInformativo";
        gameObject.layer = 12;


    }

    // Chama o Event Trigger
    public void OnPointerClick(PointerEventData data)
    {
        if (chamarInformacaoMissao)
        {
            //GetComponent<VRAutoWalk>().Informacao();
        }
    }


    public void ExibirInformativoTP()
    {

        ui3p.tpGUI_TextoTitulo.text = title;
        ui3p.tpGUI_TextoDescricao.text = description;
        ui3p.tpGUI_TextoDescricaoPlus.text = description_plus;
        ui3p.tpGUI_imgRaw.GetComponent<RawImage>().texture = img;
        ui3p.tpCanvas.enabled = true;
        ui3p.audio.clip = audioDescricao;
        ui3p.audio.Play();

    }
    public void GetInformacaoItem()
    {
        ui.GUI_TextoTitulo.text = title;
        ui.GUI_TextoDescricao.text = description;
        ui.GUI_TextoDescricaoPlus.text = description_plus;
        ui.GUI_imgRaw.GetComponent<RawImage>().texture = img;
        ui.OpenCanvasInformation();
        ui.audio.clip = audioDescricao;
        ui.audio.Play();
        //ui.canvasEnabled = true;
        //Debug.Log(title);
        //Debug.Log(description);
        //Debug.Log(description_plus);
        //Debug.Log("Funcionando Até Aqui");
        //ui.canvas.enabled = true;

    }
}
