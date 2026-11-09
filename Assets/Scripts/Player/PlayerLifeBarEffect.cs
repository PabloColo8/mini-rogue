using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeBarEffect : MonoBehaviour
{
    private Image redEffect;
    private PlayerStats playerScript;

    public float slowEffect = 0.5f; 
    public float fastEffect = 0.15f;
    private Color red = new Color(1f, 0f, 0f, 0.3f);
    private float timer = 0f;

    void Start()
    {
        redEffect = GetComponent<Image>();

        // Buscamos al jugador en la escena para leer su vida
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        float health = playerScript.GetStat(PlayerStats.StatType.Health);
        
        if (health > 3) 
        {
            redEffect.color = Color.white;
        }
        if (health == 2) // 2 de vida = lo activa
        {
            startEffect(slowEffect);
        }
        else if (health == 1) // 1 de vida = lo acelera
        {
            startEffect(fastEffect);
        }
        else if (health == 0) // voy a probar a ver como queda si lo dejo negro
        {
            redEffect.color = new Color(0f, 0f, 0f, 0.8f);
        }
    }

    void startEffect(float flashSpeed)
    {
        timer += Time.deltaTime;

        if (timer >= flashSpeed)
        {
            // lo activa y desactiva
            if (redEffect.color == red)
            {
                redEffect.color = new Color(1f, 0f, 0f, 0f);
            } else {
                redEffect.color = red;
            }

            // Reiniciamos el cronómetro
            timer = 0f;
        }
    }
}