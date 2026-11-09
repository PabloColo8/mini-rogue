using UnityEngine;

public class Room1 : BaseRoom
{
    public override Vector3 GetCameraPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    protected override Vector3[] ObjectSpawn => new Vector3[]
    {
        new Vector3(7.5f, -3.5f, 0)
    };

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerScript = collision.gameObject.GetComponent<PlayerStats>();

            // Chequeamos esto ANTES de llamar al base, porque el base lo pasa a true
            if (hasPlayerEntered == false && playerScript != null)
            {
                if (playerScript.GetStat(PlayerStats.StatType.Difficulty) == 2)
                {
                    UIManager.Instance.ShowNotification2("Enemies are now random");
                    Invoke("HideNotification", 2f);
                }
                if (playerScript.GetStat(PlayerStats.StatType.Difficulty) == 4)
                {
                    UIManager.Instance.ShowNotification2("Enemies now deal boss damage");
                    Invoke("HideNotification", 2f);
                }
            }
        }

        // Ejecuta todo el código del padre: centra la cámara, spawnea ítems y enemigos
        base.OnTriggerEnter2D(collision);
    }

    void HideNotification()
    {
        UIManager.Instance.HideNotification2();
    }


}
