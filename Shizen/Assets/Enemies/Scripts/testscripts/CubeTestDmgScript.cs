using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTestDmgScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Damaged Player: Enter");
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        //Debug.Log("Damaged Player: Stay");
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("No Damage player: Exit");
        }
    }
}
