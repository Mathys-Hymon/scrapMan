using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] private float typeSpeed = 0.05f;

    private GameObject shopDialogue;
    private GameObject shop;

    [SerializeField] private TextMeshProUGUI shopDialogueTxt;

    bool hasFinishedTalking, isTalking, canTalk;

    int index = 0;
    float timer;

    private string[] talkTexts = { "Ceci est le texte 1", "Ceci est le texte 2", "Ceci est le texte 3" };
    void Start()
    {
        instance = this;
        shopDialogue = transform.GetChild(0).gameObject;
        shop = transform.GetChild(1).gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && shop.activeSelf)
        {
            HideShop();
        }
        if (hasFinishedTalking)
        {
            timer += Time.deltaTime;
        }
        if (hasFinishedTalking && index < talkTexts.Length && timer > 1 && canTalk)
        {
            hasFinishedTalking = false;
            timer = 0;
            StopAllCoroutines();
            StartCoroutine(TypeText(talkTexts[index]));
            index += 1;
            if(index == talkTexts.Length)
            {
                isTalking = false;
            }
        }
    }

    public void EnterShopDialogue()
    {
        canTalk = false;
        shopDialogue.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StopAllCoroutines();
        StartCoroutine(TypeText("Welcome my friend"));
    }

    public void EnterShop()
    {
        canTalk = false;
        StopAllCoroutines();
        CancelInvoke();
        StartCoroutine(TypeText("Très bien, voici ce que j'ai à te proposer."));
        Invoke(nameof(ShowShop), 3f);
    }

    public void HideShop()
    {
        shop.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(TypeText("Que puis-je faire d'autre pour toi?"));
    }

    private void ShowShop()
    {
        shop.SetActive(true);
    }

    public void Talk()
    {
        CancelInvoke();
        hasFinishedTalking = true;
        isTalking = true;
        canTalk = true;
        timer = 1;
        index = 0;
    }

    public void QuitShop()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canTalk = false;
        PlayerMovement.instance.SetIsInShop(false);
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(TypeText("Goodbye my friend"));
        Invoke(nameof(HideShopDialogue), 2f);
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
        if (isTalking)
        {
            hasFinishedTalking = true;
        }
    }
}
