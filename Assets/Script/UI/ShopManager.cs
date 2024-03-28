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
    [SerializeField] private TextMeshProUGUI inShopTxt;

    bool hasFinishedTalking, isTalking, canTalk;

    [SerializeField] Button[] buyBtn;
    [SerializeField] int[] itemCost;

    int index = 0;
    float timer;

    private string[] talkTexts = { "It’s not always easy to work here, y’know... Those folks aren't talkative, they just shoot their minerals on me and I’m supposed to give them the gear. What am I supposed to do? I’m a sturdy guy y’know, I can handle it but yeah it’s a pain in the *ss.",
        "I mean you’re kind of a cool guy, you actually talked to me and it means I can finally express myself y’know. I may be a truck, but I’m a green truck! Y’know green trucks are a special kind of truck.",
        "They call us the “Motorized Grass”, or something like that… I still don’t know why they call us that, I mean I’m not made of grass and I run on gasoline so I’m not the eco friendly kind of guy. Always ready to beat some tar!",
        "Anyway the thing is, I’m a talking truck that sells things to weird miners every day and I’m getting paid with minerals. Literal minerals! And I don’t even know what to do with those weird looking rocks! Also why do those trees look like that? What a freaking shame!",
        "Oh I have to tell ya something. A hell of a story mate! So yesterday I was chilling on the road when suddenly I saw something on the ground. I immediately stopped, we never know what treasure we can find on the ground sometimes, y’know? But sadly it wasn’t a treasure, it was a freaking agonizing insect!",
        "I couldn’t let it die here, so I took it with me. Back at the garage I gave him the only drinkable thing I had: Grug! And ya know what? It worked! The little thingy was back alive! I called him “Teeny tiny little thingy that was dying on the ground but revived after drinking some Grug”, cool name huh?",
        "Yeah, I know! I take it with me everywhere I go now, it seems to like the road, like me! Anyway, that was the cool part, now I have to clean its little poops every minute or so… Can you even poop? Sorry, cringy question. I can, if you’re wondering. I won’t tell you how however.",
        "Sorry, maybe you wanted to buy something? I can still sell ya the goodies if you want! Just ask me, I won't move! Just ask mate!\r\n",
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
            StartCoroutine(TypeText(talkTexts[index], shopDialogueTxt));
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
        StartCoroutine(TypeText("Welcome my friend", shopDialogueTxt));
    }

    public void EnterShop()
    {
        canTalk = false;
        ShowShop();
        StopAllCoroutines();
        CancelInvoke();
        StartCoroutine(TypeText("Here’s what I have for ya! Y’need some mining power? Look at this awesome shiny pickaxe! Tired of getting hit by a rock on your head? Get this sturdy helmet! If you’re just hungry then you can buy this pretty sandwich!", inShopTxt));
    }

    public void HideShop()
    {
        shop.SetActive(false);
        PlayerMovement.instance.ShowHideCamera(false);
        StopAllCoroutines();
        StartCoroutine(TypeText("Well, well, well... What else can I do for you?", shopDialogueTxt));
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
        StartCoroutine(TypeText("Okay mate, see ya!", shopDialogueTxt));
        Invoke(nameof(HideShopDialogue), 2f);
    }

    private void HideShopDialogue()
    {
        shopDialogue.SetActive(false);
    }

    IEnumerator TypeText(string fullText, TextMeshProUGUI textBox)
    {
        string typedText = "";
        foreach (char letter in fullText)
        {
            typedText += letter;
            textBox.text = typedText;
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
            btn.interactable = false;
            var buttoncolor = btn.colors;
            buttoncolor.disabledColor = Color.red;
            PlayerMovement.instance.ShowItem(index, itemCost[index]);
        }
    }

    public void DistanceFromPlayer()
    {
        float Distance = Vector3.Distance(distanceGO.transform.position, PlayerMovement.instance.transform.position);

        distanceGO.transform.LookAt(PlayerMovement.instance.transform.position);
        distanceGO.transform.localScale = new Vector3(0.05f * Distance/15, 0.05f * Distance / 15, 0.05f * Distance / 15);
        distanceGO.transform.localScale = new Vector3(Mathf.Clamp(distanceGO.transform.localScale.x, 0.05f, 10f), Mathf.Clamp(distanceGO.transform.localScale.y, 0.05f, 10f), Mathf.Clamp(distanceGO.transform.localScale.z, 0.05f, 10f));
        distanceGO.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(((int)Distance).ToString() + "m");
    }
}
