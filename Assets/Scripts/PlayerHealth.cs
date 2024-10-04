using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler;

    public void Cresh()
    {
        gameOverHandler.EndGame();
        
        gameObject.SetActive(false);
    }

}
