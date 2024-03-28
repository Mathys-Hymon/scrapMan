using UnityEngine;

public class OreScript : MonoBehaviour
{
    [SerializeField] private int life = 5;
    [SerializeField] private GameObject scrapRef;

    private bool shake;
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Pickaxe")
        {
            if (life > 0 && PlayerMovement.instance.isPlayerMining())
            {
                life--;
                shake = true;
                Invoke(nameof(ResetAnim), 0.4f);
            }
            else if(life <= 0 && PlayerMovement.instance.isPlayerMining())
            {
                BreakOre();
            }
        }
    }

    private void BreakOre()
    {
        int ore = Random.Range(1, 4);

        for(int i = 0; i < ore; i++)
        {
            var scrap = Instantiate(scrapRef, transform.position + new Vector3(i, i, i), Quaternion.identity);
            scrap.transform.parent = null;
        }

        Destroy(gameObject);
    }

    private void ResetAnim()
    {
        shake = false;
    }

    private void Update()
    {
        if(shake)
        {
            transform.localRotation = Quaternion.Euler(Random.Range(-2f,2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        }
    }
}
