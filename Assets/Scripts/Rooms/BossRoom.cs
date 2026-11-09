using UnityEngine;

public class BossRoom : MonoBehaviour, ICameraZone
{
    public bool hasPlayerEntered = false; //chequear si es la primera vez que entra

    [SerializeField] private GameObject Boss;

    public Vector3 GetCameraPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private void ApplyCameraMovement()
    {
        Vector3 targetPos = GetCameraPosition();
        Camera.main.transform.position = targetPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyCameraMovement();

            PlayerStats playerScript = collision.gameObject.GetComponent<PlayerStats>();

            if (hasPlayerEntered == false)
            {
                hasPlayerEntered = true;
                Vector3 spawnBoss = new Vector3(76, 0, 0);
                Instantiate(Boss, spawnBoss, Quaternion.identity);

            }
        }
    }
}
    
