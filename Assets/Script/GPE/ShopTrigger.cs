using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject pressE;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ShopManager.instance.EnterShop();
                pressE.SetActive(false);
            }
        }
    }
}
