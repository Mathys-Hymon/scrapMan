using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    [SerializeField] float playerAcceleration;
    [SerializeField] float playerSpeed;

    private Vector2 input;
    private List<GameObject> scraps = new List<GameObject>();
    private Rigidbody rb;
    private bool grounded;
    private float height;
    private Vector3 cameraOffset;

    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        height = GetComponent<Collider>().bounds.size.y;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, height);

        if (input.magnitude < 0.1f) return;
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        transform.GetChild(1).transform.rotation = Quaternion.Lerp(transform.GetChild(1).transform.rotation, targetRotation, Time.deltaTime * 10f);

    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            input.x = -1;
            rb.velocity -= new Vector3(MouseLook.instance.transform.right.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.right.z * playerAcceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            input.x = 1;
            rb.velocity += new Vector3(MouseLook.instance.transform.right.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.right.z * playerAcceleration * Time.deltaTime);
        }
        else
        {
            input.x = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            input.y = -1;
            rb.velocity -= new Vector3(MouseLook.instance.transform.forward.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.forward.z * playerAcceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            input.y = 1;
            rb.velocity += new Vector3(MouseLook.instance.transform.forward.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.forward.z * playerAcceleration * Time.deltaTime);
        }
        else
        {
            input.y = 0;
        }

        rb.velocity += new Vector3(input.y * MouseLook.instance.transform.forward.x * playerAcceleration * Time.deltaTime, 0, input.y * MouseLook.instance.transform.forward.z * playerAcceleration * Time.deltaTime);
        rb.velocity += new Vector3(input.x * MouseLook.instance.transform.right.x * playerAcceleration * Time.deltaTime, 0, input.x * MouseLook.instance.transform.right.z * playerAcceleration * Time.deltaTime);

        if (input.magnitude == 0)
        {
            if (grounded)
            {
                rb.drag = 4;
            }

        }
        else
        {
            if (grounded)
            {
                rb.drag = 0.4f;
            }
            else
            {
                rb.drag = 0.2f;
            }
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, playerSpeed);

        for (int i = 0; i < scraps.Count; i++)
        {
            scraps[i].transform.position = Vector3.Slerp(scraps[i].transform.localPosition, transform.GetChild(1).GetChild(0).transform.position + Vector3.up * i / 2, (100 / (i + 1)) * Time.deltaTime);
        }
        Camera.main.gameObject.transform.localPosition = Vector3.Slerp(Camera.main.gameObject.transform.localPosition, cameraOffset, 10 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ScrapScript>() != null)
        {
            if(!scraps.Contains(collision.gameObject.GetComponent<ScrapScript>().gameObject))
            {
                scraps.Add(collision.gameObject.GetComponent<ScrapScript>().gameObject);
                collision.gameObject.GetComponent<ScrapScript>().gameObject.GetComponent<Collider>().enabled = false;
                collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Destroy(collision.gameObject.transform.GetChild(1).gameObject);
                cameraOffset = new Vector3(0,0,Camera.main.gameObject.transform.localPosition.z - Vector3.Distance(scraps[scraps.Count-1].transform.position, transform.GetChild(1).GetChild(0).transform.position)/2);
            }
        }
    }
}
