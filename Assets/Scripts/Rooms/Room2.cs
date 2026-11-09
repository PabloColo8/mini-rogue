using UnityEngine;

public class Room2 : BaseRoom
{
    public override Vector3 GetCameraPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    protected override Vector3[] ObjectSpawn => new Vector3[]
    {
        new Vector3(10.5f, 2.75f, 0)
    };

    protected override Vector3[] SpawnPoints => new Vector3[]
    {
        new Vector3(20, 2.5f, 0),
        new Vector3(20, -2.5f, 0),
        new Vector3(25, 0, 0)
    };

    protected override GameObject TutorialEnemy => Enemies[0];

}
