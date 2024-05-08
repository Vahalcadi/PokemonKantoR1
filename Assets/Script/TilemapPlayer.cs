using UnityEngine;
using UnityEngine.SceneManagement;

public class TilemapPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public bool engagedInCombat;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.K))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private void Movement()
    {
        /*float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");*/

        //Debug.Log(xAxis);
        //Debug.Log(yAxis);

        Vector2 direction = InputManager.Instance.Movement().normalized;

        anim.SetFloat("RightDir", direction.x);
        anim.SetFloat("UpDir", direction.y);

        rb.velocity = direction * speed;
    }


}
