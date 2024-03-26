using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerAcceleration;
    [SerializeField] float playerSpeed;


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            if((Vector3.Scale(rb.velocity, MouseLook.instance.transform.right)).magnitude < playerSpeed)
            {
                rb.velocity -= MouseLook.instance.transform.right * playerAcceleration * Time.deltaTime;
            }
            
        }
        else
        {
        }
        if(Input.GetKey(KeyCode.D))
        {
            if ((Vector3.Scale(rb.velocity, MouseLook.instance.transform.right)).magnitude > -playerSpeed)
            {
                rb.velocity += MouseLook.instance.transform.right * playerAcceleration * Time.deltaTime;
            }
        }

        if ( Input.GetKey(KeyCode.S))
        {
            if ((Vector3.Scale(rb.velocity, MouseLook.instance.transform.forward)).magnitude < playerSpeed)
            {
                rb.velocity -= MouseLook.instance.transform.forward * playerAcceleration * Time.deltaTime;
            }
        }

        if ( Input .GetKey(KeyCode.W))
        {
            if ((Vector3.Scale(rb.velocity, MouseLook.instance.transform.right)).magnitude > -playerSpeed)
            {
                rb.velocity += MouseLook.instance.transform.forward * playerAcceleration * Time.deltaTime;
            }
        }

    }
}
