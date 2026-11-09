using UnityEngine;

public class Room3 : BaseRoom
{
    public override Vector3 GetCameraPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    protected override Vector3[] ObjectSpawn => new Vector3[]
    {
        new Vector3(36f, -2.5f, 0)
    };

    protected override Vector3[] SpawnPoints => new Vector3[]
    {
        new Vector3(30, 2.5f, 0),
        new Vector3(36, 2.5f, 0),
        new Vector3(42, 2.5f, 0)
    };

    protected override GameObject TutorialEnemy => Enemies[1];

}
