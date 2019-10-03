using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [Header("Triggers")]
    public bool canExitVehicle;
    public bool canDriveVehicle;
    public bool canGetRideVehicle;
    public bool isControlled;
    public bool isPulled;
    public bool isRide;


    public void Drive(bool canExit)
    {
        if (!isControlled)
        {
            if (canDriveVehicle)
            {
                Debug.LogError("Esse Veiculo pode Ser dirigido");
                isControlled = true;
                isRide = false;
                canExitVehicle = canExit;

                this.gameObject.SendMessage("DriveVehicle");
                canGetRideVehicle = false;
                canDriveVehicle = false;
            }
            else
            {
                Debug.LogError("Nao Pode dirigir este veiculo");
            }
        } else
        {
            Debug.Log("Já esta sendo controlada");
        }
        
        
    }
    public void GetRide(bool canExit)
    {        
        if (canGetRideVehicle)
        {
            Debug.Log("Pegando uma carona, na peixaa");
            isRide = true;
            canExitVehicle = canExit;

            this.gameObject.SendMessage("WagonSetupRide");
        }        
    }
    public void Exit(bool canDrive, bool canGetRide)
    {
        if (canExitVehicle)
        {
            Debug.Log(canDrive);

            isControlled = false;
            isRide = false;
            canDriveVehicle = canDrive;
            canGetRideVehicle = canGetRide;
            this.gameObject.SendMessage("ExitVehicle");
            canExitVehicle = false;

            //IdleVehicle(canDrive, canGetRide);
        } else
        {
            Debug.LogError("Nao Pode Sair do Veiculo");
        }
    }

    public void IdleVehicle(bool canDrive, bool canGetRide)
    {        
        canExitVehicle = false;
        isControlled = false;
        isRide = false;
        canDriveVehicle = canDrive;
        canGetRideVehicle = canGetRide;
        this.gameObject.SendMessage("ExitVehicle");
    }

}
