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
    public bool _isGameOver = false;
    [SerializeField]
    private GameObject _gameController;
    

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
}
