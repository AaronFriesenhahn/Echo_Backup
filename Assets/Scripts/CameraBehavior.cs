using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //object camera will follow
    public GameObject playerToFollow;
    //offset to save, used for camera readjustment
    Vector3 cameraOffset;
    public PlayerController Player;

    private void Start()
    {
        //calculate offset
        cameraOffset = transform.position - playerToFollow.transform.position;

    }

    private void LateUpdate()
    {
        //readjust camera position based off of player + offset position
        if (playerToFollow == null)
        {
            playerToFollow = GameObject.Find("Player");
        }
        transform.position = playerToFollow.transform.position + cameraOffset;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 origianlPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, origianlPosition.z);
            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(1f);
        }
        transform.localPosition = origianlPosition;
    }
}
