using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerStats playerStats;
    private Animator animator;
    public Vector2 Inertia { get; private set; }

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerStats.isFreezed)
        {
            animator.SetBool("isFreezed", true);
            playerStats.MakeInvulnerable(1.5f);
            return;
        }

        animator.SetBool("isFreezed", false);
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        Vector2 direction = new Vector2(h, v).normalized;
        Inertia = direction;

        float speed = playerStats.GetStat(PlayerStats.StatType.Speed);
        transform.Translate(direction * (0.5f * speed * Time.deltaTime), Space.World);

        UpdateAnimations(h, v);
    }

    private void UpdateAnimations(float h, float v)
    {
        animator.SetBool("MovingUp", v > 0);
        animator.SetBool("MovingDown", v < 0);
        animator.SetBool("MovingSide", h != 0);

        if (h < 0) transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (h > 0) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}