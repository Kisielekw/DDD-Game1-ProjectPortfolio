using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogNPC : Interactable
{
    [Header("Speech")]
    public Dialog[] Dialogs;
    [Header("UI")]
    public GameObject Indicator;

    private bool dialogInRange;

    private void Update()
    {
        if (!DialogManager.Instance().InDialog && dialogInRange)
        {
            Indicator.SetActive(true);
        }
        else
        {

            Indicator.SetActive(false);
        }
    }

    protected override void Interact(Player player)
    {
        DialogManager.Instance().EnterDialog(Dialogs[Random.Range(0, Dialogs.Length - 1)], player);
    }
}
