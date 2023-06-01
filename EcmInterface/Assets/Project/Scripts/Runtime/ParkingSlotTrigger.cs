using System;
using UnityEngine;

public class ParkingSlotTrigger : MonoBehaviour
{
    public ParkingSlotType ParkingSlotType = ParkingSlotType.Charging;
    public ParkingSlotText parkingSlotText;

    private void OnTriggerEnter(Collider other)
    {
        parkingSlotText?.InvokeOnTriggerEnterAction(new TriggerMessage
        {
            ParkingSlotType = ParkingSlotType,
            ParkingSlotName = name,
            BusName = other.transform.name,
            DateTime= DateTime.Now
        });
    }

    private void OnTriggerExit(Collider other)
    {
        parkingSlotText?.InvokeOnTriggerExitAction(new TriggerMessage
        {
            ParkingSlotType = ParkingSlotType,
            ParkingSlotName = name,
            BusName = other.transform.name,
            DateTime = DateTime.Now
        });
    }
}
