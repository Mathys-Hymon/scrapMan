using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private GameObject distanceGO;

    private GameObject shopDialogue;
    private GameObject shop;

    [SerializeField] private TextMeshProUGUI shopDialogueTxt;

    bool hasFinishedTalking, isTalking, canTalk;

    [SerializeField] Button[] buyBtn;
    [SerializeField] int[] itemCost;

    int index = 0;
    float timer;

    private string[] talkTexts = { "It’s not always easy to work here, y’know... Those folks aren't talkative, they just shoot their minerals on me and I’m supposed to give them the gear. What am I supposed to do? I’m a sturdy guy y’know, I can handle it but yeah it’s a pain in the *ss.",
        "I mean you’re kind of a cool guy, you actually talked to me and it means I can finally express myself y’know. Oh by the way, yesterday I found something interesting in the mine as I was beating some rocks out. I found a teeny tiny insect that looked like it had a rough day. Like really rough, it was bleeding and all.",
        "So I decided to take it with me and try to save its life y’know. I said to myself “That’s something I should do because it seems kinda cool”. So back home I put the thingy on the table to try to save him y’know? The thing is, I’M NOT A DOCTOR.",
        "So I took the only thing that was in my fridge, Grug, and fed it with it. Don’t do that at home kids. By the way, where did those kids come from? Anyway, it worked, I think. It’s still alive so I think it worked. I called it “Molly” because the little thingy was carrying tiny bits of minerals on its back when I found it.",
        "Maybe I’ll take it at work one day, ‘could be refreshing for it. Did you know that Grug could heal wounds? I know that’s crazy! Next you get hurt by whatever bad thing is injuring you, just grab some Grug and drink it like if you were dying, I mean it should be the cause so… Just drink it mate.",
        "Anyway, you wanted to buy something? I’m here to sell you some goodies so please, do it. Do it?"
    };

    void Start()
    {
        instance = this;
        shopDialogue = transform.GetChild(0).gameObject;
        shop = transform.GetChild(1).gameObject;
    }
    private void Update()
    {
        DistanceFromPlayer();

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
        StartCoroutine(TypeText("Here’s what I have for ya! Y’need some mining power? Look at this awesome shiny pickaxe! Tired of getting hit by a rock on your head? Get this sturdy helmet! If you’re just hungry then you can buy this pretty sandwich!"));
        Invoke(nameof(ShowShop), 11f);
    }

    public void HideShop()
    {
        shop.SetActive(false);
        PlayerMovement.instance.ShowHideCamera(false);
        StopAllCoroutines();
        StartCoroutine(TypeText("Well, well, well... What else can I do for you?"));
    }

    private void ShowShop()
    {
        PlayerMovement.instance.ShowHideCamera(true);
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
        StartCoroutine(TypeText("Okay mate, see ya!"));
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

    public void BuyItem(int index)
    {
        if(PlayerMovement.instance.GetMoney() > itemCost[index])
        {
            var btn = buyBtn[index];
            btn.enabled = false;
            var buttoncolor = btn.colors;
            buttoncolor.disabledColor = Color.red;
            PlayerMovement.instance.ShowItem(index, itemCost[index]);
        }
    }

    public void DistanceFromPlayer()
    {
        float Distance = Vector3.Distance(distanceGO.transform.position, PlayerMovement.instance.transform.position);

        distanceGO.transform.LookAt(PlayerMovement.instance.transform.position);
        distanceGO.transform.localScale = new Vector3(0.05f * Distance/20, 0.05f * Distance / 20, 0.05f * Distance / 20);
        distanceGO.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(((int)Distance).ToString() + "m");
    }
}
