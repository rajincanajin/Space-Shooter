using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedModifier = 1.0f;
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
    private bool _tripleShotEnabled = false;
    private bool _shieldsEnabled = false;
    [SerializeField]
    private GameObject _shieldsVisualizer;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _uiManager;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserClip;
    [SerializeField]
    private AudioClip _explosionClip;
    [SerializeField]
    private AudioClip _outOfAmmoClip;
    [SerializeField]
    private AudioClip _repairClip;
    private SpriteRenderer _renderer;
    [SerializeField]
    private GameObject _thrusters;
    private bool _speedBoostActive = false;
    private int _shieldStrength;
    [SerializeField]
    private int _remainingAmmo = 15;
    [SerializeField]
    private AudioClip _reloadClip;
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private float _boostCooldown = 1;
    [SerializeField]
    private bool _boostActive;
    [SerializeField]
    private bool _wShotActive;
    

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("_spawnManager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
            Debug.LogError("_audioSource (Player) is NULL");
		}

        _renderer = GetComponent<SpriteRenderer>();
        //_boostActive = true;
        _boostCooldown = 1;
    }

    void Update()
    {
        CalculateMovement();
       
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if(_remainingAmmo > 0)
			{
                FireLaser();
            }
			else
			{
                _audioSource.clip = _outOfAmmoClip;
                _audioSource.Play();
			}
        }
    }

    void CalculateMovement()
	{
		if (!_speedBoostActive)
		{
            if (Input.GetKey(KeyCode.LeftShift) && _boostCooldown > 0)
            {
                StopCoroutine(ChargeBoost());
                _speedModifier = 2f;
                _boostCooldown = .0001f;
                _uiManager.GetComponent<UIManager>().UpdateBoostSlider(_boostCooldown);
                _boostActive = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _boostActive)
			{
                _boostActive = false;
                StartCoroutine(ChargeBoost());
			}
            else
            {
                _speedModifier = 1;
            }
        }
       
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);
        transform.Translate(direction * _speed * _speedModifier * Time.deltaTime);

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
        _remainingAmmo--;
        _uiManager.GetComponent<UIManager>().ShowAmmo(_remainingAmmo);
        _canFire = Time.time + _fireRate;

        if (_tripleShotEnabled)
        {           
            GameObject tripleShot =  Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, .8f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
			if (_wShotActive)
			{
                Instantiate(_laserPrefab, transform.position + new Vector3(0.8f, -.25f, 0), Quaternion.AngleAxis(20f, new Vector3(0,0,-20)));
                Instantiate(_laserPrefab, transform.position + new Vector3(-.8f, -.25f, 0), Quaternion.AngleAxis(20f, new Vector3(0, 0, 20)));
			}
        }

        _audioSource.clip = _laserClip;
        _audioSource.Play();
    }

    public void Damage()
	{
		if (_shieldsEnabled)
		{
            _shieldStrength--;
            _uiManager.GetComponent<UIManager>().UpdateShieldText(_shieldStrength);
			if (_shieldStrength < 1)
			{
                _shieldsEnabled = false;
                _shieldsVisualizer.SetActive(false);
                return;
            }
            return;
           
		}
        _lives--;
        StartCoroutine(CameraShake());
        
        _uiManager.GetComponent<UIManager>().UpdateLives(_lives);
		switch (_lives)
		{
            case 2:
                _leftEngine.SetActive(true);
                break;
            case 1:
                _rightEngine.SetActive(true);
                break;
            default:
                break;
		}

		if (_lives < 1)
		{
            _renderer.enabled = false;
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
            _thrusters.SetActive(false);
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            _uiManager.GetComponent<UIManager>()._isGameOver = true;
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, 2f);
		}
	}

    public void EnableTripleShot()
	{
        _tripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDown());
	}

    public void EnableSpeedBoost() 
	{		
        _speedModifier = 3f;
        _speedBoostActive = true;    
        StartCoroutine(SpeedBoostPowerDown());     
	}

    public void EnableShields()
	{
        _uiManager.GetComponent<UIManager>().EnableShieldText();
        _shieldStrength = 3;
        _shieldsEnabled = true;
        _shieldsVisualizer.SetActive(true);
	}

    public void EnableWShot()
	{
        _wShotActive = true;
        StartCoroutine(WShotPowerDown());
	}

    public void UpdateScore(int points)
	{
        _score += points;
        _uiManager.GetComponent<UIManager>().UpdateScoreDisplay(_score);
	}

    private void ResetSpeed()
	{
        _speedModifier = 1;
	}

    public void ReloadAmmo()
	{
        _remainingAmmo = 15;
        _uiManager.GetComponent<UIManager>().ShowAmmo(_remainingAmmo);
        _audioSource.clip = _reloadClip;
        _audioSource.Play();
	}

    public void RepairDamage()
	{
		if (_lives < 3)
		{
            _lives++;
            _uiManager.GetComponent<UIManager>().UpdateLives(_lives);
			switch (_lives)
			{
                case 2:
                    _rightEngine.SetActive(false);
                    break;
                case 3:
                    _leftEngine.SetActive(false);
                    break;
                default:
                    break;
			}
        }

        _audioSource.clip = _repairClip;
        _audioSource.Play();
	}

    IEnumerator CameraShake()
	{
        Debug.Log("Starting Camera Shake");
        int i = 0;
        while(i < 20)
		{
            float randomX = Random.Range(-.20f, 0.30f);
            float randomY = Random.Range(0.75f, 1.35f);
            Vector3 randomV3 = new Vector3(randomX, randomY, -10);
            _mainCamera.transform.position = randomV3;
            i++;
            yield return new WaitForSeconds(0.01f);
        }
    }
        
    IEnumerator TripleShotPowerDown()
	{
        yield return new WaitForSeconds(5.0f);
        _tripleShotEnabled = false;
	}

    IEnumerator SpeedBoostPowerDown()
	{
        yield return new WaitForSeconds(10.0f);
        _speedBoostActive = false;
        _speedModifier = 1f;
	}

    IEnumerator WShotPowerDown()
	{
        yield return new WaitForSeconds(5.0f);
        _wShotActive = false; 
    }

    IEnumerator ChargeBoost()
	{
        _boostCooldown = 0;
        while (_boostCooldown < 1 && !_boostActive)
        {
            _uiManager.GetComponent<UIManager>().UpdateBoostSlider(_boostCooldown);
			if (_boostCooldown <1)
			{
                _boostCooldown += 0.001f;
            }
            
            yield return new WaitForSeconds(0.005f);
        }

    }
}
