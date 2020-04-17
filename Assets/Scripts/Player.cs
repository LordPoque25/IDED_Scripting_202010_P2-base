using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody[] bullets;
    [SerializeField] int maxAmmo;
    private int ammo = 0;
    public bool dead = false;

    public const int PLAYER_LIVES = 3;

    private const float PLAYER_RADIUS = 0.4F;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 1F;

    private float hVal;

    // Eventos

    public delegate void OnPlayerStateChange();
    public static event OnPlayerStateChange OnPlayerDied;



    #region Bullet

    [Header("Bullet")]
    [SerializeField]
    private Rigidbody bullet;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private float bulletSpeed = 3F;

    #endregion Bullet

    #region BoundsReferences

    private float referencePointComponent;
    private float leftCameraBound;
    private float rightCameraBound;

    #endregion BoundsReferences

    #region StatsProperties

    public int Score { get; set; }
    public int Lives { get; set; }

    #endregion StatsProperties

    #region MovementProperties

    public bool ShouldMove
    {
        get =>
            (hVal != 0F && InsideCamera) ||
            (hVal > 0F && ReachedLeftBound) ||
            (hVal < 0F && ReachedRightBound);
    }

    private bool InsideCamera
    {
        get => !ReachedRightBound && !ReachedLeftBound;
    }

    private bool ReachedRightBound { get => referencePointComponent >= rightCameraBound; }
    private bool ReachedLeftBound { get => referencePointComponent <= leftCameraBound; }

    private bool CanShoot { get => bulletSpawnPoint != null && bullet != null; }
    public bool Dead { get => Dead1; set => Dead1 = value; }
    public bool Dead1 { get => dead; set => dead = value; }

    #endregion MovementProperties

    // Start is called before the first frame update
    private void Start()
    {
        Target.OnPlayerScoreChanged += AddScore;
        leftCameraBound = Camera.main.ViewportToWorldPoint(new Vector3(
            0F, 0F, 0F)).x + PLAYER_RADIUS;

        rightCameraBound = Camera.main.ViewportToWorldPoint(new Vector3(
            1F, 0F, 0F)).x - PLAYER_RADIUS;

        Lives = PLAYER_LIVES;
        Target.OnPlayerHit += SustractLives;

        bullets = new Rigidbody[maxAmmo];

        for (int i = 0; i < bullets.Length ; i++)
        {
            GameObject clone = Instantiate(bullet.gameObject, Vector3.zero, Quaternion.identity);
            bullets[i] = clone.GetComponent<Rigidbody>();
            clone.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Lives <= 0)
        {
            this.enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            hVal = Input.GetAxis("Horizontal");

            if (ShouldMove)
            {
                transform.Translate(transform.right * hVal * moveSpeed * Time.deltaTime);
                referencePointComponent = transform.position.x;
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                && CanShoot)
            {
                Shoot();
            }
        }
    }
    public void AddScore(int _ScoreToAdd)
    {
        Score += _ScoreToAdd;
    }
    public void SustractLives()
    {
        Lives--;
        if (Lives<1)
        {
            OnPlayerDied();
        }
        if (Lives <= 0)
        {
            Dead = true;
        }
    }

    public void Shoot()
    {
        if (ammo >= maxAmmo)
        {
            ammo = 0;
        }
        if (bullets[ammo].gameObject.activeInHierarchy == false)
        {
            bullets[ammo].gameObject.SetActive(true);
        }

        bullets[ammo].velocity = Vector3.zero;
        bullets[ammo].gameObject.transform.position = transform.position;
        bullets[ammo].AddForce(transform.up * bulletSpeed, ForceMode.Impulse);

        ammo += 1;
    }

}