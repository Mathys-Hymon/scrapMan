using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;

    [SerializeField] private float mouseSensitivity = 10;
    [SerializeField] private float offset;
    private Vector2 input;

    private void Start()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if (!PlayerMovement.instance.getIsInShop())
        {
            input.y += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            input.x += -Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            input.x = Mathf.Clamp(input.x, 0, offset);

            transform.rotation = Quaternion.Euler(input);
        }
    }
}
