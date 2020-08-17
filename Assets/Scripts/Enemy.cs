using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;


    void Start()
    { 
        
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
		if (transform.position.y < -7.5f)
		{
            float randomX = Random.Range(-9.25f, 9.25f);
            transform.position = new Vector3(randomX, 9.5f, 0);
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
            Player player = other.GetComponent<Player>();
			if (player != null)
			{
                player.Damage();
                Destroy(this.gameObject);
            }           
		}
        else if (other.tag == "Laser")
		{
            Destroy(other.gameObject);
            Destroy(this.gameObject);
		}
	}
}
