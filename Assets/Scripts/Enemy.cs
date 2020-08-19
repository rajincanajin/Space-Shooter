﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private Player _player;
	[SerializeField]
	private Animator _animator;
	private AudioSource _audioSource;

	private void Start()
	{
		_player = GameObject.Find("Player").GetComponent<Player>();
		if (_player == null)
		{
			Debug.LogError("_player is NULL");
		}
		_animator = GetComponent<Animator>();
		if (_animator == null)
		{
			Debug.LogError("_animator is NULL");
		}
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			Debug.LogError("_audioSource (Enemy) is NULL");
		}
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
            Player player = other.GetComponent<Player>();
			if (player != null)
			{
				_audioSource.Play();
				_animator.SetTrigger("OnEnemyDeath");
				_speed = 0;
                player.Damage();
                Destroy(this.gameObject, 2.35f);
            }
            else if(player == null)
			{
                Debug.LogError("player is null");
			}
		}
        else if (other.tag == "Laser")
		{
			_audioSource.Play();
			_speed = 0;
			_animator.SetTrigger("OnEnemyDeath");
            Destroy(other.gameObject);

			if (_player != null)
			{
				_player.UpdateScore(10);
			}

			Destroy(this.gameObject, 2.35f);
		}
	}
}
