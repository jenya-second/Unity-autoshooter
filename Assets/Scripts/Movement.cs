using Unity.Burst.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator anim;

    public float clamp = 20;
    public float cameraClamp = 15;
    float cameraZ;
    Vector3 joystickDirection;

    private void Start()
    {
        cameraZ = transform.GetChild(0).transform.position.z;
    }

    void Update()
    {
        Vector3 shift = new Vector3(0, 0, 0);
        if (Progress.instance.Desktop)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                shift += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                shift += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                shift += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                shift += new Vector3(1, 0, 0);
            }
            shift = shift.normalized;
        }
        else
        {
            shift = joystickDirection;
        }
        Move(shift);
    }

    public void Move(Vector3 shift)
    {
        float speed = gameObject.GetComponent<PlayerBehavior>().speed;
        if (shift == Vector3.zero)
        {
            anim.SetBool("Idle", true);
            return;
        }
        anim.SetBool("Idle", false);
        Vector3 newPos = gameObject.transform.position + shift * Progress.Func(speed) * Time.deltaTime;
        Vector3 pos = new Vector3();
        pos.y = newPos.y;
        pos.x = Mathf.Clamp(newPos.x, -1 * clamp, clamp);
        pos.z = Mathf.Clamp(newPos.z, -1 * clamp, clamp);
        Vector3 cameraPos = transform.GetChild(0).transform.position;
        cameraPos.x = Mathf.Clamp(newPos.x, -1 * cameraClamp, cameraClamp);
        cameraPos.z = Mathf.Clamp(newPos.z, -1 * cameraClamp, cameraClamp) + cameraZ;
        transform.GetChild(0).transform.position = cameraPos;
        transform.position = pos;
        transform.GetChild(3).transform.rotation = Quaternion.LookRotation(shift.normalized);
    }

    public void SetJoystickDirection(Vector3 newVector)
    {
        joystickDirection = newVector;
    }
}
