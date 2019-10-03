using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationItem : MonoBehaviour
{
    public Texture img;
    public string title;
    public string description;
    public string description_plus;
    public InformativoUI ui;
    public InformativoUI ui3p;





    public void ExibirInformativoTP()
    {

        ui3p.tpGUI_TextoTitulo.text = title;
        ui3p.tpGUI_TextoDescricao.text = description;
        ui3p.tpGUI_TextoDescricaoPlus.text = description_plus;
        ui3p.tpGUI_imgRaw.GetComponent<RawImage>().texture = img;
        ui3p.tpCanvas.enabled = true;


    }
    public void GetInformacaoItem()
    {
        ui.GUI_TextoTitulo.text = title;
        ui.GUI_TextoDescricao.text = description;
        ui.GUI_TextoDescricaoPlus.text = description_plus;
        ui.GUI_imgRaw.GetComponent<RawImage>().texture = img;
        ui.OpenCanvasInformation();
        //ui.canvasEnabled = true;
        //Debug.Log(title);
        //Debug.Log(description);
        //Debug.Log(description_plus);
        //Debug.Log("Funcionando Até Aqui");
        //ui.canvas.enabled = true;

    }
}
