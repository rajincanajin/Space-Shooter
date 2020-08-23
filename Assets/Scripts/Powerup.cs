using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	[SerializeField]
	private float _speed = 3.0f;
	[SerializeField]
	private int _powerupID; //0 = triple shot, 1 = speed, 2 = shields, 3 = ammo, 4 = repair, 5 = W-Shot
	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			Debug.LogError("_audioSource (Powerup) is NULL");
		}
	}

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
			_audioSource.Play();
			switch (_powerupID)
			{
				case 0:
					other.GetComponent<Player>().EnableTripleShot();
					break;
				case 1:
					other.GetComponent<Player>().EnableSpeedBoost();
					break;
				case 2:
					other.GetComponent<Player>().EnableShields();
					break;
				case 3:
					other.GetComponent<Player>().ReloadAmmo();
					break;
				case 4:
					other.GetComponent<Player>().RepairDamage();
					break;
				case 5:
					other.GetComponent<Player>().EnableWShot();
					break;
				default:
					Debug.LogWarning("Invalid Powerup ID. No powerup implemented");
					break;
			}
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			Destroy(this.gameObject, 1);
		}
	}
}
