using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public enum StatType { Difficulty, Health, Damage, Speed, ShotSpeed, Range, FireRate }

    [Header("Referencias UI")]
    [SerializeField] private Image lifeBar;

    [Header("Componentes Visuales")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Dictionary<StatType, float> stats = new Dictionary<StatType, float>()
    {
        { StatType.Difficulty, 1f },
        { StatType.Health, 6f },
        { StatType.Damage, 10f },
        { StatType.Speed, 7f },
        { StatType.ShotSpeed, 7f },
        { StatType.Range, 4f },
        { StatType.FireRate, 1.5f }
    };

    // Método de SOLO LECTURA 
    public float GetStat(StatType type)
    {
        if (stats.TryGetValue(type, out float value))
        {
            return value;
        }

        Debug.LogWarning($"Stat {type} no encontrada en PlayerStats.");
        return 0f;
    }

    //  Método controlado para modificar stats respetando reglas
    public void ModifyStat(StatType type, float amount)
    {
        if (stats.ContainsKey(type))
        {
            stats[type] += amount;

            // Si tienes topes definidos, los aplicas acá
            if (MaxStats.ContainsKey(type))
            {
                stats[type] = Mathf.Min(stats[type], MaxStats[type]);
            }
        }
    }

    // Diccionarios estáticos
    private static readonly Dictionary<StatType, float> BaseStats = new Dictionary<StatType, float>()
    {
        { StatType.Damage, 10f }, { StatType.Speed, 7f }, { StatType.ShotSpeed, 7f },
        { StatType.Range, 4f }, { StatType.FireRate, 1.5f }
    };

    private static readonly Dictionary<StatType, float> MaxStats = new Dictionary<StatType, float>()
    {
        { StatType.Difficulty, 100f }, { StatType.Health, 6f }, { StatType.Damage, 500f },
        { StatType.Speed, 20f }, { StatType.ShotSpeed, 25f }, { StatType.Range, 15f }, { StatType.FireRate, 20f }
    };

    private Stack<ItemData> itemStack = new Stack<ItemData>();
    public int ItemCount => itemStack.Count;

    private bool invulnerable = false;
    private float invulnerabilityTimer;
    public bool isFreezed = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateLifeBar();
    }

    private void Update()
    {
        HandleInvulnerability();
    }

    public void TakeDamage(float damage)
    {
        if (invulnerable) return;

        stats[StatType.Health] -= damage;
        UpdateLifeBar();

        if (stats[StatType.Health] <= 0)
        {
            Die();
            return;
        }

        animator.SetBool("TakeDamage", true);
        spriteRenderer.color = Color.red;
        MakeInvulnerable(1.5f);
    }

    public void MakeInvulnerable(float duration)
    {
        invulnerable = true;
        invulnerabilityTimer = duration;
    }

    private void HandleInvulnerability()
    {
        if (!invulnerable) return;

        invulnerabilityTimer -= Time.deltaTime;

        if (invulnerabilityTimer <= 1.2f)
        {
            spriteRenderer.color = Color.white;
            animator.SetBool("TakeDamage", false);
        }

        if (invulnerabilityTimer <= 0f)
        {
            invulnerable = false;
        }
    }

    public void AddItem(ItemData item)
    {
        if (!item.isBig) stats[item.targetStat] += item.modifierAmount;
        else itemStack.Push(item);

        UpdateStats();
    }
    public ItemData SacrifyLastItem()
    {
        if (itemStack.Count == 0) return null;

        ItemData sacrificed = itemStack.Pop();
        UpdateStats();
        return sacrificed;
    }

    public void UpdateStats()
    {
        foreach (var stat in BaseStats) stats[stat.Key] = stat.Value;
        foreach (ItemData item in itemStack) stats[item.targetStat] += item.modifierAmount;
        foreach (var cap in MaxStats) stats[cap.Key] = Mathf.Min(stats[cap.Key], cap.Value);
        UpdateLifeBar();
    }

    private void UpdateLifeBar()
    {
        if (lifeBar != null) lifeBar.fillAmount = stats[StatType.Health] / MaxStats[StatType.Health];
    }

    private void Die()
    {
        UIManager.Instance.OnPlayerDeath(stats[StatType.Difficulty]);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageDealer>(out var dealer))
        {
            TakeDamage(dealer.Damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet") && collision.TryGetComponent<EnemyBullet>(out var bullet))
        {
            TakeDamage(bullet.Damage);
        }
    }
}