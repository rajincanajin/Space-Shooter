using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField]
    private float _speed = 3.5f;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
	{
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //define and enforce position boundries (y)
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.85f, 0), 0);
        //define and enforce position wrap (x)
        if (transform.position.x > 11.35f)
        {
            transform.position = new Vector3(-11.35f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.35f)
        {
            transform.position = new Vector3(11.35f, transform.position.y, 0);
        }
    }
}
