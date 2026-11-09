using UnityEngine;

public class Boss : MonoBehaviour, IDamageDealer
{
    // STATS
    private float health;
    private float shotSpeed;
    private float fireRate;
    private float range;
    private float speed;
    public float damage;
    public float Damage => damage;

    //CONFIGURACION DEL DASH
    private Transform playerPosition;
    private float minTimeBetweenDashes;
    private float maxTimeBetweenDashes;
    private float currentWaitTime;
    private float dashSpeed;        // La velocidad del dash
    private float flashSpeed;     // Qué tan rápido titila

    private float dashCooldownTimer = 0f;
    private float flashTimer = 0f;
    private int flashCount = 0;          // Cuenta cuántas veces cambió de color

    private bool isPreparingDash = false;
    private bool isDashing = false;
    private Vector2 dashTarget;
    private float maxDashTime = 1f; // Tiempo máximo que durará el dash 
    private float dashDurationTimer = 0f;
    private Rigidbody2D rb;

    // ANIMACION
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // LOOT AL MORIR
    [SerializeField] private GameObject SpeedItem;
    [SerializeField] private GameObject DamageItem;
    [SerializeField] private GameObject ShotSpeedItem;
    [SerializeField] private GameObject RangeItem;
    [SerializeField] private GameObject FireRateItem;
    [SerializeField] private GameObject Portal;
    [SerializeField] private GameObject orb;
    Vector3 portalSpawn = new Vector3(76f, 0.25f, 0);
    Vector3 objectSpawn = new Vector3(75f, -3f, 0);

    //BARRA DE VIDA
    private float maxHealth;

    // OTROS
    private PlayerStats playerScript;
    [SerializeField] private GameObject bullet;
    private float shootTimer;
    Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Dash
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerPosition = player.transform;
        SetStats();

        // BARRA DE VIDA
        maxHealth = health;
        // Buscamos la barra en el UIManager y la activa
        UIManager.Instance.ShowBossBar();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        if (health <= 0)
        {
            int random = Random.Range(1, 6);
            switch (random) // Genera un objeto aleatorio en la esquina derecha
            {
                case 1:
                    Instantiate(SpeedItem, objectSpawn, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(DamageItem, objectSpawn, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(FireRateItem, objectSpawn, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(RangeItem, objectSpawn, Quaternion.identity);
                    break;
                case 5:
                    Instantiate(DamageItem, objectSpawn, Quaternion.identity); //duplicamos las chances de Daño para aumentar la satisfaccion al matar un jefe
                    break;
            }
            Instantiate(Portal, portalSpawn, Quaternion.identity);
            Instantiate(orb, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
            Instantiate(orb, transform.position, Quaternion.identity);
            UIManager.Instance.HideBossBar();
            Destroy(gameObject);
        }
    }

    void SetStats()
    {
        health = 300;
        shotSpeed = 2;
        fireRate = 2;
        range = 99;
        speed = 0.75f;
        damage = 2;
        minTimeBetweenDashes = 6f;
        maxTimeBetweenDashes = 12f;
        dashSpeed = 20f;
        flashSpeed = 0.3f;
        currentWaitTime = Random.Range(minTimeBetweenDashes, maxTimeBetweenDashes);

        if (playerScript.GetStat(PlayerStats.StatType.Difficulty) > 1)
        {
            switch (playerScript.GetStat(PlayerStats.StatType.Difficulty))
            {
                case 2:
                    health = 500;
                    shotSpeed = 3f;
                    fireRate = 1.5f;
                    speed = 1f;
                    minTimeBetweenDashes = 4f;
                    maxTimeBetweenDashes = 10f;
                    dashSpeed = 25f;
                    flashSpeed = 0.2f;
                    break;
                case 3:
                    health = 750;
                    shotSpeed = 3f;
                    fireRate = 1f;
                    speed = 1.5f;
                    minTimeBetweenDashes = 2f;
                    maxTimeBetweenDashes = 6f;
                    dashSpeed = 30f;
                    flashSpeed = 0.1f;
                    break;
                default:
                    health = 500 * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    shotSpeed = 2f * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    fireRate = 2f / (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    speed = 1.5f * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    minTimeBetweenDashes = 1f;
                    maxTimeBetweenDashes = 4f;
                    dashSpeed = 30f;
                    flashSpeed = 0.07f;
                    break;
            }
        }
    }

    void Attack()
    {
        // ---------------------------------------------------------------------
        // ESTADO 1; MOVIMIENTO Y DISPARO NORMAL
        // ---------------------------------------------------------------------
        if (isPreparingDash == false && isDashing == false)
        {
            dashCooldownTimer += Time.deltaTime; //inicia el timer
            if (dashCooldownTimer >= currentWaitTime) // activa la preparacion en caso de haber pasado el tiempo
            {
                isPreparingDash = true;
                dashCooldownTimer = 0;
                dashTarget = playerPosition.position; // Guarda la posicion del jugador
                flashCount = 0; flashTimer = 0;
            }
            else //si el tiempo no paso sigue disparadno y moviendose normal
            {
                Vector2 direction = playerPosition.position - transform.position;
                direction = direction.normalized;

                // Girar Sprite
                if (direction.x < 0)
                {
                    // Apunta a la izquierda: Invertimos la escala en X
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    offset = new(-2.3f, -0.3f, 0);
                }
                else if (direction.x > 0)
                {
                    // Apunta a la derecha: Lo devolvemos a su escala normal
                    transform.localScale = new Vector3(1, 1f, 1f);
                    offset = new(2.3f, -0.3f, 0);
                }

                transform.Translate(direction * speed * Time.deltaTime, Space.World); // se mueve hacia el jugador

                shootTimer += Time.deltaTime;

                if (shootTimer >= fireRate)
                {
                    GameObject bulletClon = Instantiate(bullet, transform.position + offset, Quaternion.identity);
                    EnemyBullet bulletScript = bulletClon.GetComponent<EnemyBullet>(); // Busca el script de la bala 
                    bulletScript.SetBulletStats(damage, shotSpeed, range, direction, true); //Le pasa las stats del enemigo a la bala
                    shootTimer = 0;
                }
            }
        }
        // ---------------------------------------------------------------------
        // ESTADO 2: PREPARA EL DASH 
        // ---------------------------------------------------------------------
        else if (isPreparingDash == true)
        {
            animator.SetBool("Stop", true);
            flashTimer += Time.deltaTime;
            if (flashTimer >= flashSpeed)
            {

                flashTimer = 0;
                flashCount++;
                if (spriteRenderer.color == Color.white) // Lo hace azul y blanco sucesivamente
                {
                    spriteRenderer.color = Color.blue;
                }
                else
                {
                    spriteRenderer.color = Color.white;
                }

                if (flashCount >= 6)
                {
                    isDashing = true;
                    isPreparingDash = false;
                    spriteRenderer.color = Color.white; // Por las dudas
                    dashDurationTimer = 0f;
                }

            }
        }
        // ---------------------------------------------------------------------
        // ESTADO 3: DASH 
        // ---------------------------------------------------------------------
        else if (isDashing == true)
        {
            dashDurationTimer += Time.deltaTime;

            Vector2 dashDirection = (dashTarget - (Vector2)transform.position).normalized;

            rb.linearVelocity = dashDirection * dashSpeed; // Mueve el RigidBody para que no se trabe en la pared

            if (Vector2.Distance(transform.position, dashTarget) < 0.1f || dashDurationTimer >= maxDashTime) // Detiene el Dash al llegar al target o cumplirse el tiempo limite
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero; //Para el RigidBody en seco
                animator.SetBool("Stop", false); // Devuelve la animacion
                currentWaitTime = Random.Range(minTimeBetweenDashes, maxTimeBetweenDashes);
            }
        }

    }

    void TakeDamage(float damage)
    {
        spriteRenderer.color = Color.red; // Lo vuelve rojo para feedback visual
        health -= damage;
        UIManager.Instance.UpdateBossHealth(health, maxHealth);
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