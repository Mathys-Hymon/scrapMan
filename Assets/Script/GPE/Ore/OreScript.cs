using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision);
    }
}
