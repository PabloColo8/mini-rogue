using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool destroyAfterUse = false;
    private float timer = 0;
    private PlayerStats playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyAfterUse == true)
        {
            timer += Time.deltaTime;

            if (timer > 2f)
            {
                playerScript.isFreezed = false; //descongela al jugador
                Destroy(gameObject); // destruye el portal
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // guardamos el script del jugador y activamos que se destruya luego de descongelarlo
            playerScript = collision.gameObject.GetComponent<PlayerStats>();
            destroyAfterUse = true;

            // Aumentamos la dificultad
            playerScript.ModifyStat(PlayerStats.StatType.Difficulty, 1);
            if (playerScript.GetStat(PlayerStats.StatType.Difficulty) == 4)
            {
                UIManager.Instance.OnWin();
            }

            // Reiniciamos todas las salas
            Room1 room1 = FindAnyObjectByType<Room1>();
            Room2 room2 = FindAnyObjectByType<Room2>();
            Room3 room3 = FindAnyObjectByType<Room3>();
            Room4 room4 = FindAnyObjectByType<Room4>();
            BossRoom bossRoom = FindAnyObjectByType<BossRoom>();
            room1.hasPlayerEntered = false;
            room2.hasPlayerEntered = false;
            room3.hasPlayerEntered = false;
            room4.hasPlayerEntered = false;
            bossRoom.hasPlayerEntered = false;
            GambleMachine GambleMachine = FindAnyObjectByType<GambleMachine>();
            GambleMachine.RepairMachine();

            // Teletransportamos al jugador a 00 y lo congela para darle un efecto mas natural
            collision.gameObject.transform.position = Vector3.zero;
            playerScript.isFreezed = true;
            UIManager.Instance.UpdateDifficultyDisplay(playerScript.GetStat(PlayerStats.StatType.Difficulty));

        }
    }
}
