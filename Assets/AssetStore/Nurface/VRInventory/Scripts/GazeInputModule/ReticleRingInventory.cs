// Copyright 2015 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using UnityEngine;
using UnityEngine.UI;

public enum GazeType
{
    WALK,
    TALK,
    INFO,
    DOOR,
    VEHICLE,
    EMPTY
}


/// Draws a circular reticle in front of any object that the user gazes at.
/// The circle dilates if the object is clickable.
[RequireComponent(typeof(Renderer))]
public class ReticleRingInventory: MonoBehaviour, GazePointerInventory {
    /// Number of segments making the reticle circle.
    public int reticleSegments = 20;

    /// Growth speed multiplier for the reticle/
    public float reticleGrowthSpeed = 8.0f;

    // Private members
    private Material materialComp;
    private GameObject targetObj;

    // Current inner angle of the reticle (in degrees).
    private float reticleInnerAngle = 0.0f;
    // Current outer angle of the reticle (in degrees).
    private float reticleOuterAngle = 0.5f;
    // Current distance of the reticle (in meters).
    private float reticleDistanceInMeters = 10.0f;

    // Minimum inner angle of the reticle (in degrees).
    private const float kReticleMinInnerAngle = 0.0f;
    // Minimum outer angle of the reticle (in degrees).
    private const float kReticleMinOuterAngle = 0.5f;
    // Angle at which to expand the reticle when intersecting with an object
    // (in degrees).
    private const float kReticleGrowthAngle = 1.5f;

    // Minimum distance of the reticle (in meters).
    private const float kReticleDistanceMin = 0.45f;
    // Maximum distance of the reticle (in meters).
    private const float kReticleDistanceMax = 10.0f;

    // Current inner and outer diameters of the reticle,
    // before distance multiplication.
    private float reticleInnerDiameter = 0.0f;
    private float reticleOuterDiameter = 0.0f;

    public Image iconGaze;
    public Canvas canvas;
    public Image[] Icons;
    
    public GazeType gazeType;
    public VRAutoWalk PlayerVRAutoWalk;

    [Header("Icons")]
    public Sprite iconInfo;
    public Sprite iconTalk, iconWalk, iconDoor, iconVehicle;
    public Animator animator;

    // Informacao Tela
    public bool exibindoInfo;
    public bool buscarInformacao;

    void Awake()
    {
        animator = iconGaze.GetComponent<Animator>();
        PlayerVRAutoWalk = GameObject.FindObjectOfType<VRAutoWalk>();       
    }

    void PlayerAndar(bool andar)
    {
        
        if (andar)
        {
            
            //Debug.LogError("Pode Andar: " + reticleDistanceInMeters);
            PlayerVRAutoWalk.podeAndar = true;
        }
        else
        {
           // Debug.LogError("Nao pode andar agora" + reticleDistanceInMeters);
            PlayerVRAutoWalk.podeAndar = false;
            PlayerVRAutoWalk.moveForward = false;
        }

    }

    void Start () {
        CreateReticleVertices();
        Renderer myRenderer = gameObject.GetComponent<Renderer>();
        materialComp = myRenderer.material;
        myRenderer.sortingLayerName = "Reticle";
        materialComp.renderQueue = -1;
        //iconGaze.enabled = false;
    }

    void OnEnable() {
        GazeInputModuleInventory.gazePointer = this;
    }

    void OnDisable() {
        if ((Object)GazeInputModuleInventory.gazePointer == this) {
            GazeInputModuleInventory.gazePointer = null;
        }
    }

    void Update() {
        UpdateDiameters();
    }

    /// This is called when the 'BaseInputModule' system should be enabled.
    public void OnGazeEnabled() {
    }

    /// This is called when the 'BaseInputModule' system should be disabled.
    public void OnGazeDisabled() {
    }

    /// Called when the user is looking on a valid GameObject. This can be a 3D
    /// or UI element.
    ///
    /// The camera is the event camera, the target is the object
    /// the user is looking at, and the intersectionPosition is the intersection
    /// point of the ray sent from the camera on the object.
    public void OnGazeStart(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive) {
    //    Debug.Log("Obejto: " + targetObject.layer.ToString());
        //Debug.Log("Vizualizando o Tela: " + targetObj.layer.ToString());
        if (isInteractive)
        {
            
            if(targetObject.gameObject.layer == 14)
            {
                PlayerVRAutoWalk.podeAndar = false;
            }
            iconGaze.enabled = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            if (targetObject.gameObject.layer == 11)
            {                
                changeGaze(targetObject.gameObject.layer);

            } else
            {
                gazeType = GazeType.EMPTY;
            }
            
            if(targetObject.CompareTag("Personagens"))
            {
                targetObject.GetComponent<InformacaoPerosonagem>().ShowUI();
            }
            //CarregaInformacoes();
            if(buscarInformacao)
            {
                
                Debug.Log("Pode Buscar Informacao");
                if (targetObject.CompareTag("ItemInformativo"))
                {                    
                     exibindoInfo = true;
                    //gazeType = GazeType.EMPTY;                    
                    //HideGaze();
                    targetObject.GetComponent<InformationItem>().GetInformacaoItem();
                }
            }
            
        } else if(targetObject.gameObject.layer == 11)
        {
            //Debug.Log("Piso");
            MudarCorGaze(1);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            iconGaze.GetComponent<Image>().enabled = true;
        }


        
        SetGazeTarget(intersectionPosition, isInteractive);
        if (isInteractive || targetObject.gameObject.layer == 11)
        {   
            
            changeGaze(targetObject.gameObject.layer);
        } else
        {
            gazeType = GazeType.EMPTY;
        }
    }

    /// Called every frame the user is still looking at a valid GameObject. This
    /// can be a 3D or UI element.
    ///
    /// The camera is the event camera, the target is the object the user is
    /// looking at, and the intersectionPosition is the intersection point of the
    /// ray sent from the camera on the object.
    public void OnGazeStay(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive) {
        if (buscarInformacao)
        {
            if (targetObject.CompareTag("ItemInformativo"))
            {
                targetObject.GetComponent<InformationItem>().GetInformacaoItem();
            }
            buscarInformacao = false;
            // PlayerVRAutoWalk.podeAndar = false;
            Debug.Log("Buscou Já era");

        }
        SetGazeTarget(intersectionPosition, isInteractive);     
    }

    /// Called when the user's look no longer intersects an object previously
    /// intersected with a ray projected from the camera.
    /// This is also called just before **OnGazeDisabled** and may have have any of
    /// the values set as **null**.
    ///
    /// The camera is the event camera and the target is the object the user
    /// previously looked at.
    public void OnGazeExit(Camera camera, GameObject targetObject) {
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        materialComp.SetColor("_Color", Color.white);
        reticleDistanceInMeters = kReticleDistanceMax;
        reticleInnerAngle = kReticleMinInnerAngle;
        reticleOuterAngle = kReticleMinOuterAngle;
        //iconGaze.enabled = false;
        if (targetObject.CompareTag("Personagens"))
        {
            targetObject.GetComponent<InformacaoPerosonagem>().HideUI();
        }

        // Implementar Logica para fechar Informativo
        if (targetObject.CompareTag("PainelInformatigo_Trigger"))
        {
           // PlayerVRAutoWalk.FecharInformacao();
        }

        HideGaze();


    }

    /// Called when a trigger event is initiated. This is practically when
    /// the user begins pressing the trigger.
    public void OnGazeTriggerStart(Camera camera) {
        // Put your reticle trigger start logic here :)
        //Debug.Log("Cliquei No Evento");
    }

    /// Called when a trigger event is finished. This is practically when
    /// the user releases the trigger.
    public void OnGazeTriggerEnd(Camera camera) {
        // Put your reticle trigger end logic here :)
    }

    public void GetPointerRadius(out float innerRadius, out float outerRadius) {
        float min_inner_angle_radians = Mathf.Deg2Rad * kReticleMinInnerAngle;
        float max_inner_angle_radians = Mathf.Deg2Rad * (kReticleMinInnerAngle + kReticleGrowthAngle);

        innerRadius = 2.0f * Mathf.Tan(min_inner_angle_radians);
        outerRadius = 2.0f * Mathf.Tan(max_inner_angle_radians);
    }

    private void CreateReticleVertices() {
        Mesh mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;

        int segments_count = reticleSegments;
        int vertex_count = (segments_count+1)*2;

        #region Vertices

        Vector3[] vertices = new Vector3[vertex_count];

        const float kTwoPi = Mathf.PI * 2.0f;
        int vi = 0;
        for (int si = 0; si <= segments_count; ++si) {
            // Add two vertices for every circle segment: one at the beginning of the
            // prism, and one at the end of the prism.
            float angle = (float)si / (float)(segments_count) * kTwoPi;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            vertices[vi++] = new Vector3(x, y, 0.0f); // Outer vertex.
            vertices[vi++] = new Vector3(x, y, 1.0f); // Inner vertex.
        }
        #endregion

        #region Triangles
        int indices_count = (segments_count+1)*3*2;
        int[] indices = new int[indices_count];

        int vert = 0;
        int idx = 0;
        for (int si = 0; si < segments_count; ++si) {
            indices[idx++] = vert+1;
            indices[idx++] = vert;
            indices[idx++] = vert+2;

            indices[idx++] = vert+1;
            indices[idx++] = vert+2;
            indices[idx++] = vert+3;

            vert += 2;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateBounds();
        ;
    }

    private void UpdateDiameters() {
        reticleDistanceInMeters =
            Mathf.Clamp(reticleDistanceInMeters, kReticleDistanceMin, kReticleDistanceMax);

        if (reticleInnerAngle < kReticleMinInnerAngle) {
            reticleInnerAngle = kReticleMinInnerAngle;
        }

        if (reticleOuterAngle < kReticleMinOuterAngle) {
            reticleOuterAngle = kReticleMinOuterAngle;
        }

        float inner_half_angle_radians = Mathf.Deg2Rad * reticleInnerAngle * 0.5f;
        float outer_half_angle_radians = Mathf.Deg2Rad * reticleOuterAngle * 0.5f;

        float inner_diameter = 2.0f * Mathf.Tan(inner_half_angle_radians);
        float outer_diameter = 2.0f * Mathf.Tan(outer_half_angle_radians);

        reticleInnerDiameter =
            Mathf.Lerp(reticleInnerDiameter, inner_diameter, Time.deltaTime * reticleGrowthSpeed);
        reticleOuterDiameter =
            Mathf.Lerp(reticleOuterDiameter, outer_diameter, Time.deltaTime * reticleGrowthSpeed);

        materialComp.SetFloat("_InnerDiameter", reticleInnerDiameter * reticleDistanceInMeters);
        materialComp.SetFloat("_OuterDiameter", reticleOuterDiameter * reticleDistanceInMeters);
        materialComp.SetFloat("_DistanceInMeters", reticleDistanceInMeters);
        canvas.GetComponent<Canvas>().planeDistance = reticleDistanceInMeters - 0.2f;
    }

    private void SetGazeTarget(Vector3 target, bool interactive) {
        Vector3 targetLocalPosition = transform.InverseTransformPoint(target);

        reticleDistanceInMeters =
            Mathf.Clamp(targetLocalPosition.z, kReticleDistanceMin, kReticleDistanceMax);
        if (interactive) {
            MudarCorGaze(reticleDistanceInMeters);
            reticleInnerAngle = kReticleMinInnerAngle + kReticleGrowthAngle;
            reticleOuterAngle = kReticleMinOuterAngle + kReticleGrowthAngle;
            //Debug.LogError(reticleDistanceInMeters);
        } else {
            materialComp.SetColor("_Color", Color.white);
            reticleInnerAngle = kReticleMinInnerAngle;
            reticleOuterAngle = kReticleMinOuterAngle;
        }


        //verifica distancia
        switch (gazeType)
        {
            case GazeType.TALK:
                {
                    if (reticleDistanceInMeters < 1.5f)
                        PlayerAndar(false);
                    break;
                }
            case GazeType.INFO:
                {
                    if (reticleDistanceInMeters < 1.5f)
                        PlayerAndar(false);
                    break;
                }
            case GazeType.DOOR:
                {
                    if (reticleDistanceInMeters < 0.5f)
                        PlayerAndar(false);
                    break;
                }
            case GazeType.VEHICLE:
                {
                    if (reticleDistanceInMeters < 0.2f)
                        PlayerAndar(false);
                    break;
                }
            default: break;
        }
        //Debug.Log(reticleDistanceInMeters + " | " + gazeType.ToString());

    }
    public void MudarCorGaze(float distancia)
    {                
        if (distancia < 3)
        {
            animator.SetBool("ativo", true);
        } else
        {
            animator.SetBool("ativo", false);
        }
    }
    public void CarregaInformacoes()
    {

    }
    public void changeGaze(LayerMask layer)
    {
        //Debug.Log(layer.value);
        switch(layer.value)
        {
            case 10:
                gazeType = GazeType.TALK;
                break;
            case 11:
                Debug.Log("No Piso");
                gazeType = GazeType.WALK;
                break;
            case 12:
                gazeType = GazeType.INFO;
                break;
            case 13:
                gazeType = GazeType.DOOR;
                break;
            case 14:
                this.gameObject.GetComponent<MeshRenderer>().enabled = true;
                gazeType = GazeType.EMPTY;            
                break;
            case 16:                
                //this.gameObject.GetComponent<MeshRenderer>().enabled = true;
                gazeType = GazeType.VEHICLE;
                break;
            case 0:
                gazeType = GazeType.EMPTY;
                break;
        }        

        switch (gazeType)
        {
            case GazeType.WALK:
                //Debug.Log("Pode andar");
                PlayerAndar(true);                
                iconGaze.GetComponent<Image>().sprite = iconWalk;
                break;
            case GazeType.INFO:
                
                iconGaze.GetComponent<Image>().sprite = iconInfo;
                break;
            case GazeType.TALK:
                //Debug.LogError(reticleDistanceInMeters);
                
                iconGaze.GetComponent<Image>().sprite = iconTalk;
                break;
            case GazeType.DOOR:
                
                iconGaze.GetComponent<Image>().sprite = iconDoor;
                break;
            case GazeType.VEHICLE:
                iconGaze.GetComponent<Image>().sprite = iconVehicle;
                break;
            case GazeType.EMPTY:
                HideGaze();
                break;            
        }
                
    }
    void HideGaze()
    {
        iconGaze.enabled = false;        
    }
}
