using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageDealer
{
    protected float health;
    protected float damage;
    public float Damage => damage;
    protected Transform playerPosition;
    protected PlayerStats playerScript;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject orb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerStats>();
        playerPosition = player.transform;
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        if (health <= 0)
        {
            int random = Random.Range(0, 100);
            if (random <= 10)
            {
                Instantiate(orb, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    protected virtual void SetStats() { }

    protected virtual void Attack() { }

    void TakeDamage(float damage)
    {
        spriteRenderer.color = Color.red; // Lo vuelve rojo para feedback visual
        health -= damage;
        Invoke("HideFeedback", 0.1f);
    }

    void HideFeedback()
    {
        spriteRenderer.color = Color.white; // Lo devuelve a la normalidad
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            Bullet scriptBullet = collider.gameObject.GetComponent<Bullet>(); // guarda el script de la bala
            float DamageTaken = scriptBullet.Damage; // guarda el daño de la bala 
            TakeDamage(DamageTaken);
            scriptBullet.Deactivate(); 
        }
    }
}

