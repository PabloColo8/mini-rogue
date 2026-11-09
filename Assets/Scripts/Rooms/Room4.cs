using UnityEngine;

public class Room4 : BaseRoom
{
    public override Vector3 GetCameraPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    protected override Vector3[] ObjectSpawn => new Vector3[]
    {
        new Vector3(59f, 0, 0)
    };

    protected override Vector3[] SpawnPoints => new Vector3[]
    {
        new Vector3(59, 3f, 0), 
        new Vector3(59, -3f, 0), 
        new Vector3(55, 0, 0)
    };

    protected override GameObject TutorialEnemy => Enemies[2];

}
