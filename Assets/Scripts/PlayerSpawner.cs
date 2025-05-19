using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        Progress.instance.player = Instantiate(Progress.instance.typeOfCharacter, new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
    }
}
