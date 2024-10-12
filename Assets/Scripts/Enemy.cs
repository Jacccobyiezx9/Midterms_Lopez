using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform playerTransform;
    private int enemyColorValue;

    public int colorValue = 0;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        enemyColorValue = colorValue;
        GetComponent<SpriteRenderer>().color = GetColorFromValue(enemyColorValue);
    }

    void Update()
    {
        MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    public int GetColorValue()
    {
        return enemyColorValue;
    }

    Color GetColorFromValue(int colorValue)
    {
        switch (colorValue)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            default: return Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
