using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	[SerializeField]
	private float _speed = 3.0f;
	[SerializeField]
	private int _powerupID; //0 = triple shot, 1 = speed, 2 = shields

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
		if (transform.position.y < -6.0f)
		{
            Destroy(this.gameObject);
		}
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			switch (_powerupID)
			{
				case 0:
					other.GetComponent<Player>().EnableTripleShot();
					break;
				case 1:
					other.GetComponent<Player>().EnableSpeedBoost();
					break;
				case 3:
					Debug.Log("Shields Enabled");
					break;
				default:
					Debug.LogWarning("Invalid Powerup ID. No powerup implemented");
					break;
			}
			Destroy(this.gameObject);
		}
	}
}
