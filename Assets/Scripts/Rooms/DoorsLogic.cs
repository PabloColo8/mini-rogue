using UnityEngine;

public class DoorsLogic : MonoBehaviour
{
    [SerializeField] private GameObject Doors;
    [SerializeField] private GameObject GambleMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (aliveEnemies > 0 )
        {
            Doors.SetActive( true );
            GambleMachine.SetActive( false );
        } else
        {
            Doors.SetActive( false );
            GambleMachine.SetActive( true );
        }
    }



}
