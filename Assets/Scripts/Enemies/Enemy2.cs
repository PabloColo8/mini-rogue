using UnityEngine;

public class Enemy2 : BaseEnemy
{
    private float timeTillNextMove;
    private float attackDistance;
    private float attackSpeed;
    float attackTimer;
    Vector2 targetPosition;
    bool isAttacking = false;

    protected override void SetStats()
    {
        health = 30;
        timeTillNextMove = 0.5f;
        attackDistance = 7;
        attackSpeed = 5;
        damage = 1;

        if (playerScript.GetStat(PlayerStats.StatType.Difficulty) > 1)
        {
            switch (playerScript.GetStat(PlayerStats.StatType.Difficulty))
            {
                case 2:
                    health = 40;
                    timeTillNextMove = 0.4f;
                    attackDistance = 7;
                    attackSpeed = 6;
                    damage = 1;
                    break;
                case 3:
                    health = 50;
                    timeTillNextMove = 0.3f;
                    attackDistance = 9;
                    attackSpeed = 7;
                    break;
                default:
                    health = 30 * (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 2f);
                    timeTillNextMove = 0.2f;
                    attackDistance = 20;
                    attackSpeed = 9;
                    damage = 2;
                    break;
            }
        }
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeTillNextMove) // si ya paso el tiempo para moverse
        {
            if (isAttacking == false) // primero se fija donde moverse
            {
                attackTimer += Time.deltaTime;
                float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);
                if (distanceToPlayer <= attackDistance) // se fija si el jugador esta cerca 
                {
                    targetPosition = playerPosition.position; // pone el target en el jugador
                    isAttacking = true; // se mueve hacia el jugador
                }
                else if (distanceToPlayer >= attackDistance)  // si no esta cerca
                {
                    float randomX = Random.Range(-3f, 3f);
                    float randomY = Random.Range(-3f, 3f);

                    targetPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY); // pone el target al azar
                    isAttacking = true;  // se mueve al azar
                }
                attackTimer = 0;
            }
            else if (isAttacking == true) // se mueve al target
            {
                attackTimer += Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, attackSpeed * Time.deltaTime); //moverse
                if (Vector2.Distance(transform.position, targetPosition) < 0.1f || attackTimer >= 2f) // revisa si ya llego O, si esta atascado se mueve al pasar el tiempo
                {
                    isAttacking = false; //vuelve a buscar target
                    attackTimer = 0; // reinicia el timer para moverse
                }
            }
        }
    }


}



