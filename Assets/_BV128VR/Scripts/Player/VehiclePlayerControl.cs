using System.Collections;
using UnityEngine;

public class VehiclePlayerControl : MonoBehaviour
{

    public Vehicle vehicle;
    public bool p_drivining;
    public bool p_isRide;
    public bool p_canExit;
    public bool p_canDrive;
    public bool p_canGetRide;

    public Canvas guiControlsPlayer;
    public Canvas guiControlsVehicle;

    public GameObject buttonDrive;
    public GameObject buttonGetRide;
    public GameObject buttonExitVehicle;

    private bool isVR;




    private void Start()
    {
        isVR = GameManager.gameMode.Equals(GameMode.VR) ? true : false;
        if(!isVR)
        {
            if (!p_drivining)
            {
                Debug.Log("Ta passando aqui nao");
                guiControlsVehicle.enabled = false;
            }
        }  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isVR)
        {
            if (vehicle != null)
            {

                ControlsPC();

#if MOBILE_INPUT
                Controls3rdPerson();
#endif
            }
            else
            {
                DisableOptionsButtons();

            }
        }


    }

    public void ControlsVR()
    {
        if (vehicle != null)
        {
            if (!p_drivining)
            {

                if ((p_canGetRide) && vehicle.canGetRideVehicle) // Carona
                {
                    GetRide();
                }
                else if (p_canExit && vehicle.canExitVehicle)
                {
                    Debug.Log("Vamo tentar sair 1");
                    ExitVehicle();
                }
            }
        }


    }
    public void Controls3rdPerson()
    {
        if (!p_drivining)
        {
            //Debug.Log("aqui");
            if ((p_canDrive && !p_canGetRide) && vehicle.canDriveVehicle)
            {
                Debug.Log("Pode Dirigir, mas nao Carona");
                buttonDrive.SetActive(true);
                buttonGetRide.SetActive(false);
                buttonExitVehicle.SetActive(false);
            }
            else if ((p_canDrive && p_canGetRide) && vehicle.canDriveVehicle)
            {
                Debug.Log("Pode Dirigir, e pegar Carona");
                buttonDrive.SetActive(true);
                buttonGetRide.SetActive(true);
                buttonExitVehicle.SetActive(false);
            }
            else if ((p_canGetRide) && vehicle.canGetRideVehicle) // Carona
            {
                buttonGetRide.SetActive(true);
                buttonExitVehicle.SetActive(false);
                buttonDrive.SetActive(false);
            }
            else if (p_canExit && vehicle.canExitVehicle)
            {
                buttonExitVehicle.SetActive(true);
                buttonGetRide.SetActive(false);
                buttonDrive.SetActive(false);
            }
        }
        else if (p_canExit && vehicle.canExitVehicle)
        {
            buttonDrive.SetActive(false);
            buttonGetRide.SetActive(false);
            buttonExitVehicle.SetActive(true);
        }
    }

    public void ControlsPC()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!p_drivining)
            {
                if ((p_canDrive && !p_canGetRide) && vehicle.canDriveVehicle)
                {
                    Drive();
                }
                else if ((p_canDrive && p_canGetRide) && vehicle.canDriveVehicle)
                {
                    Drive();
                }
                else if ((p_canGetRide) && vehicle.canGetRideVehicle) // Carona
                {
                    GetRide();
                }
                else if (p_canExit && vehicle.canExitVehicle)
                {
                    ExitVehicle();
                }
            }
            else if (p_canExit && vehicle.canExitVehicle)
            {
                ExitVehicle();
            }
        }
    }


    public void GetRide()
    {
        StartCoroutine("CoroutineGetRide");
    }
    public void ExitVehicle()
    {
        buttonDrive.SetActive(false);
        StartCoroutine("CoroutineExitVehicle");
    }
    public void Drive()
    {
        StartCoroutine("CoroutineDriveVehicle");
    }
    IEnumerator CoroutineDriveVehicle()
    {
        vehicle.Drive(true);
        EnableCanvas(true); // Vehicle Enabled
        //guiControlsPlayer.enabled = false; // Primeiro desativa pra nao dar conflitos ---- 
        //guiControlsVehicle.enabled = true;
        p_drivining = true;
        p_canDrive = false;
        p_canGetRide = false;
        p_isRide = false;
        yield return new WaitForSeconds(3f);
        p_canExit = true;
    }
    IEnumerator CoroutineExitVehicle()
    {

        Debug.Log("Ta aqui no Exit");
        vehicle.Exit(true, true);

        p_drivining = false;
        p_canExit = false;
        p_isRide = false;
        EnableCanvas(false); // Player Enabled
        //guiControlsVehicle.enabled = false;
        //guiControlsPlayer.enabled = true;
        yield return new WaitForSeconds(2f);

        p_canDrive = true;
        p_canGetRide = true;
        //p_isRide = false;
        //p_canGetRide = true;
    }
    IEnumerator CoroutineGetRide()
    {
        //Debug.Log("Quero uma carona, na peixaa!!!");
        vehicle.GetRide(true);
        p_isRide = true;
        p_drivining = false;
        p_canGetRide = false;
        p_canDrive = false;
        EnableCanvas(false); // Player Enabled
        //guiControlsVehicle.enabled = false;
        //guiControlsPlayer.enabled = true;
        yield return new WaitForSeconds(2f);
        p_canExit = true;


    }

    public void EnableCanvas(bool active)
    {
        if(!isVR)
        {
            guiControlsVehicle.enabled = active;
            guiControlsPlayer.enabled = !active;
        }
    }

    public void DisableOptionsButtons()
    {
        buttonDrive.SetActive(false);
        buttonExitVehicle.SetActive(false);
        buttonGetRide.SetActive(false);
    }
}


/*
 * 


 * //else if(!p_canDrive && p_canGetRide)
                //{
                //    Debug.Log("Pode so pegar carona");
                //    //StartCoroutine("GetRide");                    
                //}
                //if (p_drivining || p_isRide)
                //{
                //    StartCoroutine("ExitVehicle");
                //    //Debug.Log("Saindo do Carro");
                //}
  if (p_canExit && p_drivining || p_canExit && p_isRide)
        {
            //Debug.Log("Pode Sair");
            vehicle.SendMessage("ExitVehicle");
            StartCoroutine("ExitVehicle");
        }
        if (p_canDrive)
        {
            if (vehicle != null)
            {
                if(p_drivining)
                {

                } else
                {
                    vehicle.SendMessage("Drive");

                    StartCoroutine("DriveVehicle");
                    DriveVehicle();
                    Debug.Log("Agora dirigindo");
                }                 
            }                
        } else if(p_canGetRide)
        {
            if (vehicle != null && !p_drivining)
            {                    
                vehicle.SendMessage("GetRide");
                StartCoroutine("GetRide");                    
            }
        }           
        } 
 */
