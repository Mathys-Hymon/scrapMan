using UnityEngine;

public class ScrapScript : MonoBehaviour
{
    private GameObject trail;
    private void Start()
    {
        trail = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if(trail != null)
        {
            trail.transform.LookAt(transform.position + Vector3.up);
        }
        
    }
}
