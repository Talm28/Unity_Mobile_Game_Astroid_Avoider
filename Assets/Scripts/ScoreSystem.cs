using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private float _scoreMultiplier;

    private float _score;
    private bool _shouldCount = true;

    // Update is called once per frame
    void Update()
    {
        if(!_shouldCount) return;

        _score += Time.deltaTime * _scoreMultiplier;

        _scoreText.text = Mathf.FloorToInt(_score).ToString();    
    }

    public int EndTimer()
    {
        _shouldCount = false;

        _scoreText.text = string.Empty;

        return Mathf.FloorToInt(_score);
    }

    public void StartTimer()
    {
        _shouldCount = true;
    }
}
