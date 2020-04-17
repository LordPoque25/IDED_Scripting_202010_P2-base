using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Player playerRef;

    [SerializeField]
    private Image[] lifeImages;

    [SerializeField]
    private Text scoreLabel;

    [SerializeField]
    private Button restartBtn;

    [SerializeField]
    private float tickRate = 0.2F;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    private void Start()
    {
        Target.OnPlayerHit += UpdateUI;
        Target.OnPlayerScoreChanged += UpdateScore;
        Player.OnPlayerDied += UpdateUI;
        ToggleRestartButton(false);

        playerRef = FindObjectOfType<Player>();

        if (playerRef != null && lifeImages.Length == Player.PLAYER_LIVES)
        {
            InvokeRepeating("UpdateUI", 0F, tickRate);
        }
    }

    private void ToggleRestartButton(bool val)
    {
        if (restartBtn != null)
        {
            restartBtn.gameObject.SetActive(val);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (lifeImages[i] != null && lifeImages[i].enabled)
            {
                lifeImages[i].gameObject.SetActive(playerRef.Lives >= i + 1);
            }
        }

        if (scoreLabel != null)
        {
            scoreLabel.text = playerRef.Score.ToString();
        }

        if (playerRef.Lives <= 0)
        {
            CancelInvoke();

            if (scoreLabel != null)
            {
                scoreLabel.text = "Game Over";
            }

            ToggleRestartButton(true);
        }
    }
    public void UpdateScore(int _donothing)
    {
        UpdateUI();
    }
}