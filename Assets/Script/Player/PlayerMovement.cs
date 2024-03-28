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
    private bool grounded, hasPickaxe, isMining, isInShop;
    private float height;
    private Vector3 cameraOffset;

    [SerializeField] private GameObject pressE;

    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        height = GetComponent<Collider>().bounds.size.y;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, height);
        if (hasPickaxe)
        {
            transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            if (Input.GetMouseButton(0) && !isInShop)
            {
                isMining = true;
                float rotationValue = Mathf.PingPong(Time.time * 8f, 1.0f) * 120 - 35;
                Vector3 newRotation = new Vector3(rotationValue, 180, 0);
                transform.GetChild(1).GetChild(2).localEulerAngles = newRotation;
            }
            else
            {
                isMining = false;
                transform.GetChild(1).GetChild(2).localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        }

        if (pressE.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            isInShop = true;
            ShopManager.instance.EnterShopDialogue();
            pressE.SetActive(false);
        }

        if (transform.GetChild(2).gameObject.activeSelf)
        {
            transform.GetChild(1).transform.rotation *= Quaternion.Euler(0, Time.deltaTime * 50, 0);
        }

        if (input.magnitude < 0.1f) return;

        if(rb.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
            transform.GetChild(1).transform.rotation = Quaternion.Lerp(transform.GetChild(1).transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) && !isInShop)
        {
            input.x = -1;
            rb.velocity -= new Vector3(MouseLook.instance.transform.right.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.right.z * playerAcceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && !isInShop)
        {
            input.x = 1;
            rb.velocity += new Vector3(MouseLook.instance.transform.right.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.right.z * playerAcceleration * Time.deltaTime);
        }
        else
        {
            input.x = 0;
        }

        if (Input.GetKey(KeyCode.S) && !isInShop)
        {
            input.y = -1;
            rb.velocity -= new Vector3(MouseLook.instance.transform.forward.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.forward.z * playerAcceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W) && !isInShop)
        {
            input.y = 1;
            rb.velocity += new Vector3(MouseLook.instance.transform.forward.x * playerAcceleration * Time.deltaTime, 0, MouseLook.instance.transform.forward.z * playerAcceleration * Time.deltaTime);
        }
        else
        {
            input.y = 0;
        }
        if (!isInShop)
        {
            rb.velocity += new Vector3(input.y * MouseLook.instance.transform.forward.x * playerAcceleration * Time.deltaTime, 0, input.y * MouseLook.instance.transform.forward.z * playerAcceleration * Time.deltaTime);
            rb.velocity += new Vector3(input.x * MouseLook.instance.transform.right.x * playerAcceleration * Time.deltaTime, 0, input.x * MouseLook.instance.transform.right.z * playerAcceleration * Time.deltaTime);
        }

        if (input.magnitude == 0)
        {
            if (grounded)
            {
                rb.drag = 4;
                rb.mass = 1;
            }
            else
            {
                rb.drag = 0;
                rb.mass = 50;
            }
        }
        else
        {
            if (grounded)
            {
                rb.drag = 0.4f;
                rb.mass = 1;
            }
            else
            {
                rb.drag = 0f;
                rb.mass = 50;
            }
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, playerSpeed);

        for (int i = 0; i < scraps.Count; i++)
        {
            scraps[i].transform.position = Vector3.Slerp(scraps[i].transform.localPosition, transform.GetChild(1).GetChild(0).transform.position + Vector3.up * i / 2, (100 / (i + 1)) * Time.deltaTime);
        }

        if(cameraOffset.magnitude > 0 && !isInShop)
        {
            Camera.main.gameObject.transform.localPosition = Vector3.Slerp(Camera.main.gameObject.transform.localPosition, cameraOffset, 10 * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ScrapScript>() != null && scraps.Count <= 15)
        {
            if(!scraps.Contains(collision.gameObject.GetComponent<ScrapScript>().gameObject))
            {
                scraps.Add(collision.gameObject.GetComponent<ScrapScript>().gameObject);
                collision.gameObject.GetComponent<ScrapScript>().gameObject.GetComponent<Collider>().enabled = false;
                collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                collision.gameObject.transform.GetChild(0).gameObject.layer = gameObject.layer;
                collision.gameObject.transform.GetChild(0).GetChild(0).gameObject.layer = gameObject.layer;
                Destroy(collision.gameObject.transform.GetChild(1).gameObject);
                cameraOffset = new Vector3(0,0,Camera.main.gameObject.transform.localPosition.z - Vector3.Distance(scraps[scraps.Count-1].transform.position, transform.GetChild(1).GetChild(0).transform.position)/2);
            }
        }
    }

    public void SetColorMoney(int cost)
    {
        if(cost >= scraps.Count)
        {
            for (int i = 0; i < scraps.Count; i++)
            {
                scraps[i].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                scraps[i].transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        else
        {
            for (int i = 0; i < cost; i++)
            {
                scraps[i].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
                scraps[i].transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }

    }

    public void ResetColorMoney()
    {
        foreach(GameObject scrap in scraps)
        {
            scrap.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
            scrap.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }

    public bool isPlayerMining()
    {
        return isMining;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            pressE.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            pressE.SetActive(false);
        }
    }

    public void SetIsInShop(bool active)
    {
        isInShop=active;
    }

    public void ShowHideCamera(bool active)
    {
        transform.GetChild(2).gameObject.SetActive(active);
    }

    public bool getIsInShop() { return isInShop; }

    public int GetMoney() { return scraps.Count; }

    public void ShowItem(int index, int cost)
    {
        switch (index)
        {
            case 0:
                transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                hasPickaxe = true;
                break;
            case 1:
                transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
                break;  
            case 2:
                transform.GetChild(1).GetChild(4).gameObject.SetActive(true);
                break;
        }

        for (int i = 0; i < cost; i++)
        {
            Destroy(scraps[i].gameObject);
            scraps.RemoveAt(i);
        }
    }

}
