using UnityEngine;

public abstract class BaseRoom : MonoBehaviour, ICameraZone
{
    public bool hasPlayerEntered = false; //chequear si es la primera vez que entra

    // OBJETOS
    [SerializeField] private GameObject[] items;

    // ENEMIGOS
    [SerializeField] protected GameObject[] Enemies;
    [SerializeField] private GameObject tutorial;

    // Especificaciones de cada sala
    protected virtual Vector3[] SpawnPoints => null;
    protected virtual GameObject TutorialEnemy => null;
    protected virtual Vector3[] ObjectSpawn => null;

    [SerializeField] protected bool isTutorial = false;


    private void ApplyCameraMovement()
    {
        Vector3 targetPos = GetCameraPosition();
        Camera.main.transform.position = targetPos;
    }

    public abstract Vector3 GetCameraPosition();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyCameraMovement();
            PlayerStats playerScript = collision.gameObject.GetComponent<PlayerStats>();

            if (hasPlayerEntered == false)
            {
                if (ObjectSpawn != null)
                {
                    if (isTutorial == true)
                    {
                        tutorial.SetActive(true);
                        foreach (var spawn in ObjectSpawn)
                        {
                            Instantiate(items[0], spawn, Quaternion.identity);
                        }
                        isTutorial = false;
                    }
                    else
                    {
                        foreach (var spawn in ObjectSpawn)
                        {
                            int randomIndex = Random.Range(0, items.Length);
                            Instantiate(items[randomIndex], spawn, Quaternion.identity);
                        }
                    }
                }

                if (SpawnPoints != null)
                {
                    // TUTORIAL: Solo aparece el Enemigo 1, uno por spawn
                    if (playerScript.GetStat(PlayerStats.StatType.Difficulty) == 1)
                    {
                        foreach (Vector3 spawn in SpawnPoints)
                        {
                            Instantiate(TutorialEnemy, spawn, Quaternion.identity);
                        }

                    }

                    // DIFICULTAD DEL 2 AL INFINITO
                    else
                    {
                        foreach (Vector3 spawn in SpawnPoints) // Recorre los spawns
                        {
                            int minEnemies = (int)(1 + (playerScript.GetStat(PlayerStats.StatType.Difficulty) / 3f)); // hasta llegar al modo infinito el minimo es siempre 1

                            int maxEnemies = (int)playerScript.GetStat(PlayerStats.StatType.Difficulty); // Como maximo 2 en nivel 2 y asi

                            int randomEnemiesCount = Random.Range(minEnemies, maxEnemies + 1); // en Random.Range de enteros el máximo es exclusivo por eso suma + 1

                            for (int i = 0; i < randomEnemiesCount; i++)
                            {
                                //elije el enemigo al azar
                                int randomIndex = Random.Range(0, Enemies.Length);
                                GameObject enemyToSpawn = Enemies[randomIndex];

                                //da un offset al azar para que no choquen
                                float randomX = Random.Range(-0.8f, 0.8f);
                                float randomY = Random.Range(-0.8f, 0.8f);
                                Vector3 offset = new Vector3(randomX, randomY, 0);

                                // instancia al enemigo
                                Instantiate(enemyToSpawn, spawn + offset, Quaternion.identity);
                            }
                        }
                    }
                }
                hasPlayerEntered = true;
            }
        }
    }
}