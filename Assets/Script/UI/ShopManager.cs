using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] private float typeSpeed = 0.05f;

    private GameObject shopDialogue;

    [SerializeField] private TextMeshProUGUI shopDialogueTxt;
    void Start()
    {
        instance = this;
        shopDialogue = transform.GetChild(0).gameObject;
    }

    public void QuitShop()
    {
        StopAllCoroutines();
        StartCoroutine(TypeText("Goodbye my friend"));
        Invoke(nameof(HideShopDialogue), 3f);
    }

    private void HideShopDialogue()
    {
        shopDialogue.SetActive(false);
    }

    IEnumerator TypeText(string fullText)
    {
        string typedText = "";
        foreach (char letter in fullText)
        {
            typedText += letter;
            shopDialogueTxt.text = typedText;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

}
