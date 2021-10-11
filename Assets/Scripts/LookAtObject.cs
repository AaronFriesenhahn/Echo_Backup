using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] Transform _ObjectToLookAt;
    [SerializeField] Transform _currentObjectPositon;

    Vector3 originalPosition;

    int x = 0;
    int y = 0;
    int z = 0;

    float speed = 1f;

    private void Awake()
    {
        originalPosition = _currentObjectPositon.position;
    }

    void Update()
    {
        if (gameObject.transform.position.x > _ObjectToLookAt.position.x)
        {
            if (x == 0)
            {
                //transform.Rotate(0, 45, 0);
                _currentObjectPositon.Translate(-.5f, 0, 0);

                x = 1;

            }
        }
        else if (gameObject.transform.position.x + 2.5 < _ObjectToLookAt.position.x)
        {
            if (y == 0)
            {
                //transform.Rotate(0, -45, 0);
                _currentObjectPositon.Translate(.5f, 0, 0);
                y = 1;
            }
        }
        else
        {
            x = 0;
            y = 0;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _currentObjectPositon.position = originalPosition;
        }
    }
}
