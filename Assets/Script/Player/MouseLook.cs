using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;

    private void Start()
    {
        instance = this;
    }
}
