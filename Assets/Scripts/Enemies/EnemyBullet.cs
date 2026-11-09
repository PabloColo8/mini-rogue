using UnityEngine;

public class EnemyBullet : MonoBehaviour, IDamageDealer
{
    private float damage;
    public float Damage => damage;
    private float range;
    private Vector2 finalVelocity;
    private SpriteRenderer spriteRenderer;
    private Vector2 startPosition;
    private bool shootedByBoss = false;
    public bool ShootedByBoss => shootedByBoss;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CheckRange();
    }

    public void SetBulletStats(float damage, float speed, float range, Vector2 direction, bool ShootedByBoss)
    {
        this.damage = damage;
        this.range = range;

        finalVelocity = (direction * speed);
        shootedByBoss = ShootedByBoss;
    }


    void MoveBullet()
    {
        transform.Translate(finalVelocity * Time.deltaTime);
    }

    void CheckRange()
    {
        float distance = Vector2.Distance(startPosition, transform.position);

        if (distance >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Borrar la bala al chocar con algo que no sea un enemigo, el jugador u otra bala
    {
        if (shootedByBoss == false)
        {
            if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Room") && !collision.gameObject.CompareTag("EnemyBullet") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Object"))
            {
                Destroy(gameObject);
            }
        } else if  (shootedByBoss == true)
        {
            if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Room") && !collision.gameObject.CompareTag("EnemyBullet") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Object"))
            {
                Destroy(gameObject);
            }
        }


    }

}
