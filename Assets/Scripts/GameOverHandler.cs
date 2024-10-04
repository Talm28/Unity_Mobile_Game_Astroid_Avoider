using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverDisplay;
    [SerializeField] private AstroidSpawner _astroidSpawner;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private ScoreSystem _scoreSystem;
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _player;
    
    public void EndGame()
    {
        _astroidSpawner.enabled = false;

        int finalScore = _scoreSystem.EndTimer();
        _gameOverText.text = $"Your Score: {finalScore}";
        
        _gameOverDisplay.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueButton()
    {
        AdManager.Instance.ShowAd(this);

        _continueButton.interactable = false;

    }

    public void ContinueGame()
    {
        _scoreSystem.StartTimer();
        
        _player.transform.position = Vector3.zero;
        _player.SetActive(true);
        _player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        _astroidSpawner.enabled = true;

        _gameOverDisplay.gameObject.SetActive(false);
    }
}
