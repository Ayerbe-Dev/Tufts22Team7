using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private Vector3 offsetTemp;

    // Start is called before the first frame update
    void Start()
    {
        offsetTemp.Set(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        offsetTemp.x = player.transform.position.x + offset.x;
        if (offsetTemp.x < 0)
        {
            offsetTemp.x = 0;
        }
        if (offsetTemp.x > 92)
        {
            offsetTemp.x = 92;
        }
        transform.position = offsetTemp;
    }

    public bool is_on_camera(Vector2 pos, Vector2 offset) {
        double left_bound = player.transform.position.x - this.offset.x;
        double right_bound = player.transform.position.x + this.offset.x;
        return ((right_bound >= pos.x + offset.x && left_bound <= pos.x + offset.x)
        || (right_bound >= pos.x - offset.x && left_bound <= pos.x - offset.x));
    }
}
