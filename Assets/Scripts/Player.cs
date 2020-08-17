using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _tripleShotEnabled = false;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("_spawnManager is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();
       
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
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

    void FireLaser()
	{
        _canFire = Time.time + _fireRate;

        if (_tripleShotEnabled)
        {           
            GameObject tripleShot =  Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, .8f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
        }
    }

    public void Damage()
	{
        _lives--;

		if (_lives < 1)
		{
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
		}
	}

    public void EnableTripleShot()
	{
        _tripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDown());
	}

    IEnumerator TripleShotPowerDown()
	{
        yield return new WaitForSeconds(5.0f);
        _tripleShotEnabled = false;
	}
}
