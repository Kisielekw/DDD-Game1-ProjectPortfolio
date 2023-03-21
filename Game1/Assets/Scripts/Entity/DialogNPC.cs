using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogNPC : MonoBehaviour
{
    [Header("Speech")]
    public Dialog[] Dialogs;
    [Header("UI")]
    public GameObject Indicater;

    private bool dialogInRange;

    private void Update()
    {
        if (!DialogManager.Instance().inDilaog && dialogInRange)
        {
            Indicater.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogManager.Instance().EnterDialog(Dialogs[Random.Range(0, Dialogs.Length - 1)]);
            }
        }
        else
        {

            Indicater.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            dialogInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dialogInRange && collision.gameObject.tag == "Player")
        {
            dialogInRange = false;
        }
    }
}
