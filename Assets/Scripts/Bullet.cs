using GLTFast.Schema;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private IObjectPool<Bullet> pool;
    private float damage;
    public float Damage => damage;
    private float range;
    private Vector2 finalVelocity;

    private Vector2 startPosition;


    void Update()
    {
        MoveBullet();
        CheckRange();
    }

    public void SetBulletStats(float damage, float speed, float range, Vector2 direction, Vector2 inertia, IObjectPool<Bullet> poolRef)
    {
        this.pool = poolRef;

        this.damage = damage;
        this.range = range;

        startPosition = transform.position;

        finalVelocity = (direction * speed) + (inertia * 2f);
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
            Deactivate();
        }
    }

    public void Deactivate()
    {
        pool.Release(this); 
    }

    private void OnTriggerEnter2D(Collider2D collision) // Borrar la bala al chocar con algo que no sea un enemigo, el jugador u otra bala
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Room") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Object") && !collision.gameObject.CompareTag("EnemyBullet"))
        {
            Deactivate();
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            EnemyBullet scriptBullet = collision.gameObject.GetComponent<EnemyBullet>();
            if (scriptBullet.ShootedByBoss == true)
            {
                Deactivate();
            }
        }
    }

}
