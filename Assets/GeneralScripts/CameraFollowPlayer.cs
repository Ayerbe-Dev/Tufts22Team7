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
        transform.position = offsetTemp;
    }
}
