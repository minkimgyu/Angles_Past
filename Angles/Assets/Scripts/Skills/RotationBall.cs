using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBall : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject[] balls;
    public int ballCount = 2;
    public float speed = 70;
    public float distanceFromPlayer = 2f;

    Transform playerTransform;


    private void OnEnable()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        transform.SetParent(playerTransform);

        for (int i = 0; i < ballCount; i++)
        {
            float angle = (360.0f * i) / ballCount;

            Vector3 offset = Vector3.up * distanceFromPlayer;

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            GameObject ball = Instantiate(ballPrefab, transform);
            ball.transform.localPosition = rotatedOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 axis = new Vector3(0, 0, 1);
        transform.RotateAround(playerTransform.position, axis, Time.deltaTime * speed);
    }
}
