using UnityEngine;


public class EnemyController : BaseEnemy
{
    private float speed;

    protected override void SetStats()
    {
        health = 40;
        speed = 2;
        damage = 1;
        if (playerScript.GetStat(PlayerStats.StatType.Difficulty) > 1)
        {
            switch (playerScript.GetStat(PlayerStats.StatType.Difficulty))
            {
                case 2:
                    health = 50;
                    speed = 2.5f;
                    break;
                case 3:
                    health = 75;
                    speed = 3.5f;
                    break;
                default:
                    health = 50 * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    speed = 2 + (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    damage = 2;
                    break;
            }
        }
    }
    protected override void Attack()
    {
        Vector2 direction = playerPosition.position - transform.position;
        direction = direction.normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

}