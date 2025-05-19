using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        float x = Mathf.MoveTowards(transform.position.x, 0, Time.deltaTime * 4f);
        float y = Mathf.MoveTowards(transform.position.y, 5, Time.deltaTime * 4f);
        float z = Mathf.MoveTowards(transform.position.z, 0, Time.deltaTime * 4f);
        float rot = Mathf.MoveTowards(transform.rotation.x, 0, Time.deltaTime);
        transform.position = new Vector3(x, y, z);
        transform.rotation = new Quaternion(rot, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }
}
