using UnityEngine;

public class TilemapPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        //Debug.Log(xAxis);
        //Debug.Log(yAxis);

        Vector2 direction = new(xAxis, yAxis);
        rb.velocity = direction * speed;

        Debug.Log(rb.velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 contact = collision.GetContact(0).point - new Vector2(transform.position.x, transform.position.y);

        if (collision.collider.CompareTag("jumpable"))
        {
            Debug.Log(Vector2.Dot(contact.normalized, Vector2.down));

            if (Vector3.Dot(contact.normalized, Vector2.down) > 0.65)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 2);
            }

            /*if (collision.GetContact(0).point.y < transform.position.y)
                transform.position = new Vector2(transform.position.x, transform.position.y - 2);*/
        }
    }
}
