using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _shieldsText;
    [SerializeField]
    private Text _ammoText;
    public bool _isGameOver = false;
    [SerializeField]
    private GameObject _gameController;
    [SerializeField]
    private Slider _boostSlider;
    [SerializeField]
    private Text _boostChargeText;
    

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        
    }

    void Update()
    {

    }

    public void UpdateScoreDisplay(int score)
	{
        _scoreText.text = "Score: " + score;
	}

    public void UpdateLives(int currentLives)
	{
        _livesImage.sprite = _livesSprites[currentLives];
		if (currentLives < 1)
		{
            GameOverSequence();
		}
	}

    void GameOverSequence()
	{
        _gameController.GetComponent<GameManager>().GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlicker());
    }

    public void UpdateShieldText(int shieldStrength)
	{
        int strengthPercentage;
        
		switch (shieldStrength)
		{
            case 3:
                strengthPercentage = 100; //should never be true, but including the option in case of unforseen circumstances to prevent bugs
                break;
            case 2:
                strengthPercentage = 66;
                break;
            case 1:
                strengthPercentage = 33;
                break;
            default:
                strengthPercentage = 0;
                break;
		}
        _shieldsText.text = "Shield Strength: " + strengthPercentage + "%";
	}

    public void EnableShieldText()
	{
        _shieldsText.text = "Shield Strength: 100%";
    }

    public void ShowAmmo(int _remainingAmmo)
	{
        _ammoText.text = "Ammo: " + _remainingAmmo;
	}

    IEnumerator GameOverTextFlicker()
	{
		while (true)
		{
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }        
	}

    public void UpdateBoostSlider(float value)
	{

        _boostSlider.value = value;
        _boostChargeText.text = Mathf.Round(_boostSlider.value * 100) + "%";     
	}
}
