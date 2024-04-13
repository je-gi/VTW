using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogManager : MonoBehaviour
{
    #region Assign Canvas
    public GameObject PopUp;
    #endregion


    #region Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopUp.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopUp.SetActive(false);
        }
    }
    #endregion
}





