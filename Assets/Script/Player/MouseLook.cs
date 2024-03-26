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
        input.y += Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity * 100;
        input.x += -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity * 100;

        input.x = Mathf.Clamp(input.x, -offset, offset);

        transform.rotation = Quaternion.Euler(input);
    }
}
