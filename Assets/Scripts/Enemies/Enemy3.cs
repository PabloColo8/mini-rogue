using UnityEngine;

public class Enemy3 : BaseEnemy
{
    private float shotSpeed;
    private float fireRate;
    private float attackDistance;
    private float range;
    private float speed;

    [SerializeField] private GameObject bullet;
    private float shootTimer;
    Vector3 offset;

    protected override void SetStats()
    {
        health = 40;
        shotSpeed = 3;
        fireRate = 2;
        attackDistance = 8;
        range = 99;
        speed = 0.5f;
        damage = 1;
        if (playerScript.GetStat(PlayerStats.StatType.Difficulty) > 1)
        {
            switch (playerScript.GetStat(PlayerStats.StatType.Difficulty))
            {
                case 2:
                    health = 50;
                    shotSpeed = 4;
                    fireRate = 1f;
                    attackDistance = 9;
                    speed = 1f;
                    break;
                case 3:
                    health = 75;
                    shotSpeed = 5;
                    fireRate = 0.7f;
                    attackDistance = 10;
                    speed = 1.5f;
                    break;
                default:
                    health = 40 * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    shotSpeed = 3f * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    fireRate = 1f / (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    attackDistance = 99;
                    speed = 2.5f;
                    damage = 2;
                    break;
            }
        }
    }


    protected override void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);
        if (distanceToPlayer <= attackDistance) // se fija si el jugador esta cerca 
        {
            Vector2 direction = playerPosition.position - transform.position;
            direction = direction.normalized;

            // Girar Sprite
            if (direction.x < 0)
            {
                // Apunta a la izquierda: Invertimos la escala en X
                transform.localScale = new Vector3(-0.45f, 0.45f, 0.45f);
                offset = new (-0.6f, -0.1f, 0); 
            }
            else if (direction.x > 0)
            {
                // Apunta a la derecha: Lo devolvemos a su escala normal
                transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                offset = new(0.6f, -0.1f, 0);
            }

            transform.Translate(direction * speed * Time.deltaTime, Space.World); // se mueve hacia el jugador

            shootTimer += Time.deltaTime;

            if (shootTimer >= fireRate)
            {
                GameObject bulletClon = Instantiate(bullet, transform.position + offset, Quaternion.identity);
                EnemyBullet bulletScript = bulletClon.GetComponent<EnemyBullet>(); // Busca el script de la bala 
                bulletScript.SetBulletStats(damage, shotSpeed, range, direction, false); //Le pasa las stats del enemigo a la bala
                shootTimer = 0;
            }
        }
    }

}
