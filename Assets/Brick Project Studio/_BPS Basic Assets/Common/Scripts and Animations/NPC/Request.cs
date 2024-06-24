using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
    public string[] order;
    public bool isCorrect = true;
    public BoxCollider trigger;
    public GameObject player;
    public GameObject register;
    public GameObject box;
    public GameObject pedidosPanel;
    public TextMeshProUGUI scoreText;
    public List<string> randomOrder;
    public LayerMask layer;
    public int score;
    private bool enableTrigger = true;
    public GameObject currentNPC;
    public GameObject itemPrefab;
    public Sprite[] itemSprites;
    public GameObject time;
    public Image timebar;
    public AudioSource cashierSource;
    public float maxTime = 100.0f;
    private float currentTime;
    private bool isRunning = false;
    private DinheiroManager dinheiro;

    void Start()
    {
        trigger.enabled = true;
        time.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && randomOrder != null && randomOrder.Count > 0)
        {
            float distance = Vector3.Distance(player.transform.position, register.transform.position);
            if (distance <= 1)
            {
                getObjects();
                ClearPedidosPanel();
                enableTrigger = true;
                time.SetActive(false);
                cashierSource.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "trigger" && enableTrigger)
        {
            enableTrigger = false;
            Debug.Log("O trigger ativou");
            randomOrder = GenerateOrder();
            Debug.Log("Novo pedido gerado: " + string.Join(", ", randomOrder));
            DisplayOrder(randomOrder);

            //Audio de pedido
            other.GetComponent<AudioSource>().Play();

            //Iniciar o timer
            currentTime = maxTime;
            isRunning = true;

            //Mostar o tempo
            timebar.fillAmount = 1;
            time.SetActive(true);

            StartCoroutine(DeliveryTimer());
        }
    }

    void DisplayOrder(List<string> order)
    {
        //criar os pedidos
        foreach (string item in order)
        {
            GameObject newItem = Instantiate(itemPrefab, pedidosPanel.transform);
            newItem.SetActive(true);
            Image itemImage = newItem.transform.Find("ItemImage").GetComponent<Image>();
            TextMeshProUGUI itemName = newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();

            itemName.text = item;
            itemImage.sprite = GetItemSprite(item);

            itemName.enabled = true;
            itemImage.enabled = true;
        }
    }

    Sprite GetItemSprite(string itemName)
    {
        //procurar os sprites respetivos
        foreach (Sprite sprite in itemSprites)
        {
            if (sprite.name == itemName)
            {
                return sprite;
            }
        }
        return null;
    }

    void ClearPedidosPanel()
    {
        foreach (Transform child in pedidosPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    List<string> GenerateOrder()
    {
        int numItens = Random.Range(2, 4);
        List<string> itens = new List<string>();

        for (int i = 0; i < numItens; i++)
        {
            int index = Random.Range(0, order.Length);
            itens.Add(order[index]);
        }

        return itens;
    }

    void getObjects()
    {
        RaycastHit[] hits = Physics.SphereCastAll(box.transform.position, 0.5f, Vector3.up, Mathf.Infinity, layer);
        Debug.Log("produto: " + hits.Length);
        List<string> objects = new List<string>();
        foreach (RaycastHit hit in hits)
        {
            string productName = hit.collider.gameObject.tag;
            objects.Add(productName);
        }

        checkOrder(objects);
    }

    void checkOrder(List<string> detectedProducts)
    {
        isCorrect = true; // Reset isCorrect for each check
        List<GameObject> deliveredObjects = new List<GameObject>();

        foreach (string item in randomOrder)
        {
            if (!detectedProducts.Contains(item))
            {
                isCorrect = false;
            }
        }

        if (isCorrect)
        {
            score += 100;
            Debug.Log("pedido correto");
        }
        else
        {
            score -= 50;
            Debug.Log("pedido errado");
        }
        // Adicionar todos os objetos detectados à lista de objetos entregues
        foreach (string item in randomOrder)
        {
            foreach (RaycastHit hit in Physics.SphereCastAll(box.transform.position, 0.5f, Vector3.up, Mathf.Infinity, layer))
            {
                deliveredObjects.Add(hit.transform.gameObject);
            }
        }
        // Destruir os objetos entregues
        foreach (GameObject obj in deliveredObjects)
        {
            Destroy(obj);
        }
        scoreText.SetText("Pontuação: " + score);
        randomOrder.Clear();
        StopTimer();
        ClearPedidosPanel();
        Exit();
    }

    IEnumerator DeliveryTimer()
    {
        while (isRunning && currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;

            // Atualizar a barra de tempo
            float normalizedTime = currentTime / maxTime;
            timebar.fillAmount = normalizedTime;

            //Mostrar o tempo restante
            Debug.Log("Tempo restante: " + currentTime + " segundos");
        }

        if (currentTime <= 0)
        {
            // O tempo acabou
            HandleFailedDelivery();
        }
    }

    void HandleFailedDelivery()
    {
        isRunning = false;
        score -= 50;
        scoreText.SetText("Pontuação: " + score);
        Debug.Log("Tempo esgotado. Pedido falhou.");

        // Limpar o pedido 
        ClearPedidosPanel();
        enableTrigger = true;
        time.SetActive(false);
        Exit();
    }

    void Exit()
    {
        if (currentNPC != null)
        {
            currentNPC.GetComponent<MovimentoNPC>().Exit();
        }
    }

    void StopTimer()
    {
        isRunning = false;
        time.SetActive(false);
    }
}
