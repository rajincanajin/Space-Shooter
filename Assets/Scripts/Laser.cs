using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    public bool _isEnemyLaser = false; 

    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
		else
		{
            MoveDown();
		}
    }

    void MoveDown()
	{
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.25f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveUp()
	{
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 7.25f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void SetEnemyLaser()
	{
        _isEnemyLaser = true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && _isEnemyLaser == true)
		{
            Player player = other.GetComponent<Player>();

			if (player != null)
			{
                player.Damage();
                Transform parent = transform.parent;
                foreach(Transform child in parent)
				{
                    GameObject.Destroy(child.gameObject);
				}
                Destroy(this.gameObject);
            }         
		}
	}
}
