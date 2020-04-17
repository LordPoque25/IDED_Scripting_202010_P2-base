using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    private const float TIME_TO_DESTROY = 10F;

    [SerializeField]
    private int maxHP = 1;

    private int currentHP;

    [SerializeField]
    private int scoreAdd = 10;

    //Eventos

    public delegate void OnPlayerMod(int _Scoreadd);
    public static event OnPlayerMod OnPlayerScoreChanged;
    public delegate void OnPlayerAction();
    public static event OnPlayerAction OnPlayerHit;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collidedObjectLayer = collision.gameObject.layer;

        if (collidedObjectLayer.Equals(Utils.BulletLayer))
        {
            collision.gameObject.SetActive(false);

            currentHP -= 1;

            if (currentHP <= 0)
            {
                OnPlayerScoreChanged(scoreAdd);
                /*Player player = FindObjectOfType<Player>();

                if (player != null)
                {
                    player.Score += scoreAdd;
                }*/

                gameObject.SetActive(false);
            }
        }
        else if (collidedObjectLayer.Equals(Utils.PlayerLayer) ||
            collidedObjectLayer.Equals(Utils.KillVolumeLayer))
        {
            OnPlayerHit();
            /*
                Player player = FindObjectOfType<Player>();

            if (player != null)
            {
                player.Lives -= 1;

                if (player.Lives <= 0 && player.OnPlayerDied != null)
                {
                    player.OnPlayerDied();
                }
            }
            */

            gameObject.SetActive(false);
        }
    }
}