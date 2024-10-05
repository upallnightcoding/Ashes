using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCntrl : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 offset;
    private float smoothFactor = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = player.position - offset;

        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
    }
}
