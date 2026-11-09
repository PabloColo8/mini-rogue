using System.Collections;
using UnityEngine;

public class GambleMachine : MonoBehaviour
{
    [Header("Indicador Visual")]
    [SerializeField] private GameObject interactPrompt;

    [Header("Componentes Visuales de la Máquina")]
    [SerializeField] private SpriteRenderer machineRenderer;      // El SpriteRenderer de la máquina
    [SerializeField] private SpriteRenderer capsuleItemRenderer;  // SpriteRenderer hijo dentro del tubo de vidrio
    [SerializeField] private Sprite brokenSprite;                 // Sprite de la máquina rota
    [SerializeField] private Sprite normalSprite;

    [Header("Lógica de Ítems")]
    private Vector3[] SpawnPoints = new Vector3[]
    {
        new Vector3(51.5f, 1.5f, 0),
        new Vector3(55.5f, 1.5f, 0)
    };
    [SerializeField] private GameObject[] itemPrefabs;

    private static bool hasSeenRecyclerTutorial = false;
    private bool isPlayerNearby = false;
    private bool isBroken;
    private bool isProcessing = false; // Evita spamear la 'E' mientras hace la animación
    private PlayerStats playerScript;
    private bool hasItemLoaded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactPrompt.SetActive(false);
        capsuleItemRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (isBroken || isProcessing) return;

        if (!hasSeenRecyclerTutorial)
        {
            hasSeenRecyclerTutorial = true;
            UIManager.Instance.ShowNotification2("Gamble your last item for a chance of 2 new items");
            Invoke(nameof(HideNotification), 3f);
            return;
        }

        if (!hasItemLoaded)
        {
            if (playerScript == null || playerScript.ItemCount == 0)
            {
                UIManager.Instance.ShowNotification2("You have no items to gamble!");
                Invoke(nameof(HideNotification), 1.5f);
                return;
            }

            LoadItemIntoCapsule();
            return;
        }

        // 3. PASO 2: Si el ítem ya está adentro, activa la ruleta
        if (!isProcessing)
        {
            StartCoroutine(GambleRoutine());
        }
    }

    private void LoadItemIntoCapsule()
    {
        // Sacamos el último ítem de la pila
        ItemData sacrificedItem = playerScript.SacrifyLastItem();

        // Mostramos el sprite del ítem en la cápsula
        if (capsuleItemRenderer != null && sacrificedItem.icon != null)
        {
            capsuleItemRenderer.sprite = sacrificedItem.icon;
            capsuleItemRenderer.enabled = true;
        }

        // Mostramos la notificación con la descripción negativa del ItemData
        UIManager.Instance.ShowNotification(sacrificedItem.negativeDescription);
        UIManager.Instance.ShowNotification2("Item inserted! Press [E] to start");
        hasItemLoaded = true;
        Invoke(nameof(HideNotification), 4f);
    }

    private IEnumerator GambleRoutine()
    {
        playerScript.isFreezed = true;
        isProcessing = true;
        interactPrompt.SetActive(false);

        Color originalColor = machineRenderer.color;

        // Fase 1: Titileo de advertencia (va acelerando)
        for (int i = 0; i < 5; i++)
        {
            machineRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.25f);
            machineRenderer.color = originalColor;
            yield return new WaitForSeconds(0.25f);
        }

        // Fase 2: Gran destello blanco
        machineRenderer.color = Color.white * 3f; // Destello brillante
        yield return new WaitForSeconds(0.6f);

        // Fase 3: La máquina se rompe
        isBroken = true;
        machineRenderer.color = originalColor;
        machineRenderer.sprite = brokenSprite;

        // El ítem de la cápsula se consume/desaparece
        if (capsuleItemRenderer != null)
        {
            capsuleItemRenderer.enabled = false;
        }
        hasItemLoaded = false;

        // Fase 4: 50/50 de probabilidad
        bool wonGamble = Random.value >= 0.5f;

        if (wonGamble)
        {
            // Éxito: Instancia 2 ítems al azar en los puntos de salida
            foreach (Vector3 spawnPos in SpawnPoints)
            {
                int randomIndex = Random.Range(0, itemPrefabs.Length);
                Instantiate(itemPrefabs[randomIndex], spawnPos, Quaternion.identity);
            }
            UIManager.Instance.ShowNotification2("JACKPOT! 2 items spawned!");
        }
        else
        {
            // Fallo: No da nada
            UIManager.Instance.ShowNotification2("The machine collapsed... Nothing happened.");
        }

        playerScript.UpdateStats();
        Invoke(nameof(HideNotification), 2.5f);
        isProcessing = false;
        playerScript.isFreezed = false;
    }

    public void RepairMachine()
    {
        machineRenderer.sprite = normalSprite;
        isBroken = false;
    }

    private void HideNotification()
    {
        UIManager.Instance.HideNotification2();
        UIManager.Instance.HideNotification();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBroken)
        {
            isPlayerNearby = true;
            playerScript = collision.GetComponent<PlayerStats>();
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactPrompt.SetActive(false);
        }
    }
}
