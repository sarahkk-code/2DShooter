
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        int enemySpeed = PlayerPrefs.GetInt("EnemySpeed", 2);

        switch (enemySpeed)
        {
            case 1:
                moveSpeed = 1f;
                break;

            case 2:
                moveSpeed = 2f;
                break;

            case 3:
                moveSpeed = 3f;
                break;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }
}
