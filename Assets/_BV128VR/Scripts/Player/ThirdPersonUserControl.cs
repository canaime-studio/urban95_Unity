using Canaime.Players;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using Canaime.

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour, IMovementPlayer
{
    //public bool isMultiplayer;

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                    // the world-relative desired move direction, calculated from the camForward and user input.
    public Transform pivotCamera;


    public void isEnable(bool enabled)
    {
        this.enabled = enabled;
    }

    void setPivotDistance(Vector3 position)
    {
        if(pivotCamera != null)
        {
            pivotCamera.transform.localPosition = position;
        }
        
    }

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update()
    {

        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

    }

    /* public bool Agachar()
    {
        return true;
    }
    */
   
    public void Andar()
    {

    }
    public void StopWalk()
    {
        Debug.Log("Metodo Parar");

    }
    public void Sentar()
    {

    }
    public void Sentado(bool isSitting)
    {
        //if (isSitting)
        //{
        //    setPivotDistance(new Vector3(0, 2, 2));
        //} else
        //{
        //    setPivotDistance(new Vector3(0,2,0));
        //}
        
        //Debug.Log("Sentado Messege");
    }
    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {

        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);
        //(Agachar())
        //crouch = true;

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }

}
