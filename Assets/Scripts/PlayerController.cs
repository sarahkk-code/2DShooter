
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;

    private bool gameEnded = false;
    private float nextFireTime = 0f;

    void Update()
    {
        if (gameEnded)
            return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveX, moveY);

        transform.position += (Vector3)(
            movement.normalized * moveSpeed * Time.deltaTime
        );

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null)
            return;

        Vector3 spawnPosition = transform.position + Vector3.up * 0.6f;

        Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded || !other.CompareTag("Enemy"))
            return;

        gameEnded = true;

        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.SaveGameResults();
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetFloat("SurvivalTime", Time.timeSinceLevelLoad);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("Exit");
    }
}
