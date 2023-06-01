using UnityEngine;
using TMPro;
using System;

public class ParkingSlotText : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public event Action<TriggerMessage> OnTriggerEnterAction;
    public event Action<TriggerMessage> OnTriggerExitAction;

    private void Awake()
    {
        if (Text != null) 
        {
            Text = GetComponent<TextMeshProUGUI>();
        }

        OnTriggerEnterAction += ParkingSlotText_OnTriggerEnterAction;
        OnTriggerExitAction += ParkingSlotText_OnTriggerExitAction;

    }

    private void ParkingSlotText_OnTriggerEnterAction(TriggerMessage triggerData)
    {
        Text.color = Color.blue;
        Text.text= "Bus: " + triggerData.BusName + 
            " arriving to " + triggerData.ParkingSlotName + 
            " (" + triggerData.ParkingSlotType.ToString() + ") " +
            "at: " + triggerData.DateTime.ToShortTimeString();
    }

    private void ParkingSlotText_OnTriggerExitAction(TriggerMessage triggerData)
    {
        Text.color = Color.green;
        Text.text = "Bus: " + triggerData.BusName +
            " leaving from " + triggerData.ParkingSlotName +
            " (" + triggerData.ParkingSlotType.ToString() + ") " +
            "at: " + triggerData.DateTime.ToShortTimeString();
    }

    public void InvokeOnTriggerEnterAction(TriggerMessage triggerData)
    {
        OnTriggerEnterAction?.Invoke(triggerData);
    }

    public void InvokeOnTriggerExitAction(TriggerMessage triggerData)
    {
        OnTriggerExitAction?.Invoke(triggerData);
    }
}
