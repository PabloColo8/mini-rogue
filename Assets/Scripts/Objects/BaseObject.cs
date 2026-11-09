using UnityEngine;

public class BaseObject : MonoBehaviour
{

    [SerializeField] protected ItemData data;
    public virtual string ObjectDescription => data.description;

    private PlayerStats Player;

    // TIMER PARA CUANDO AGARRAS EL OBJETO
    float AnimationTime = 1;
    float Timer = 0;
    bool IsCollected = false;

    // VARIABLES LEVITACION
    private Vector3 InicialPosition;
    public float FloatingSpeed = 0.1f;
    public float MaxAltitude = 0.15f;
    private bool MovingUp = true;

    // INTERFAZ
    public float NotificationTime = 3;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player = collision.gameObject.GetComponent<PlayerStats>();
                // Da la estadistica correspondiente
                Player.AddItem(data);

                // Pone el objeto sobre el jugador y lo congela
                Player.isFreezed = true;
                Vector3 newPosition = collision.transform.position + new Vector3(0, 1.5f, 0);
                transform.position = newPosition;
                Timer += Time.deltaTime;
                GetComponent<Collider2D>().enabled = false; // desactiva las colisiones del objeto 
                IsCollected = true;

                // Mostrar Interfaz
                UIManager.Instance.ShowNotification(ObjectDescription);


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player = collision.gameObject.GetComponent<PlayerStats>();
            Player.AddItem(data);
            Destroy(gameObject); // Destruye el Objeto
        }
    }

    private void Update()
    {
        if (IsCollected == false) // Que flote cuando no esta agarrado
        {
            if (MovingUp == true)
            {
                transform.Translate(Vector3.up * FloatingSpeed * Time.deltaTime);
                if (transform.position.y >= InicialPosition.y + MaxAltitude) // Chequea que no este en la altura maxima
                {
                    MovingUp = false;
                }
            }
            else // Si no esta yendo hacia arriba que vaya hacia abajo
            {
                transform.Translate(Vector3.down * FloatingSpeed * Time.deltaTime);
                if (transform.position.y <= InicialPosition.y - MaxAltitude) //Chequea que no este en la altura minima restando en lugar de sumando
                {
                    MovingUp = true;
                }
            }
        }
        else if (IsCollected == true) // Que desaparezca cuando lo agarras
        {
            Timer += Time.deltaTime;
            if (Timer >= AnimationTime)
            {
                Player.isFreezed = false; // Descongela al jugador
                GetComponent<SpriteRenderer>().enabled = false; // Desactiva el sprite para que no se vea como si hubiera sido destruido
            }

            if (Timer >= NotificationTime)
            {
                UIManager.Instance.HideNotification();
                Destroy(gameObject); // Destruye el Objeto
            }
        }
        
    }

    private void Start()
    {
        InicialPosition = transform.position;
    }
}
