using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private int bulletColorValue;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void SetColorValue(int colorValue)
    {
        bulletColorValue = colorValue;
        GetComponent<SpriteRenderer>().color = GetColorFromValue(colorValue);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {

                if (bulletColorValue == enemy.GetColorValue())
                {
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    public void SetTarget(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
