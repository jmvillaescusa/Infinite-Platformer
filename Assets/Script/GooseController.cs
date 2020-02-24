using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseController : MonoBehaviour
{
    public GameManager m_gameManager;
    //public PauseMenuScript m_pauseMenuScript;
    public Rigidbody2D m_rigidbody;
    public Animator m_animator;
    public HonkAnimation m_honkAnimation;

    public float spawnPosX = -7.5f;
    public float spawnPosY = -1.73f;

    private float m_frozenPosX = 0.0f;

    public bool m_isRunning = false;

    static float m_DEFAULT_SPEED = 3;
    public float m_moveSpeed;

    public float m_jumpPower = 0;
    
    public int m_numberOfJumps = 2;
    public bool m_canDoubleJump;

    public bool m_isAlive;
    public bool m_frozen;
    public bool m_obstacleCollision;

    public LayerMask groundLayer;

    public bool GroundCheck()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if(hit.collider != null)
        {
            return true;
        }
        return false;
    }

    void setSpeed()
    {
        if (!GroundCheck())
        {
            m_animator.SetBool("Airborne", true);
        }
        else
        {
            m_animator.SetBool("Airborne", false);
        }

        if (m_isRunning)
        {
            m_animator.SetBool("Walking", false);
            m_animator.SetBool("Running", true);
            m_moveSpeed = m_DEFAULT_SPEED + 3;
            
        }
        else
        {
            m_animator.SetBool("Walking", true);
            m_animator.SetBool("Running", false);
            m_moveSpeed = m_DEFAULT_SPEED;
        }
    }

    void Jump()
    {
        if (m_numberOfJumps > 0)
        {
            m_jumpPower = 7;
            m_rigidbody.AddForce(new Vector2(m_moveSpeed, m_jumpPower), ForceMode2D.Impulse);
            m_numberOfJumps--;
        }
        else if (m_numberOfJumps == 0)
        {
            Debug.Log("Double Jump used");
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_isAlive = true;
        m_frozen = false;
        m_obstacleCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isAlive)
        {
            if (!m_gameManager.m_isPaused && !m_obstacleCollision)
            {
                // Check if Goose is running or not
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    m_isRunning = true;
                }
                else
                {
                    m_isRunning = false;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

                if (Input.GetKeyDown(KeyCode.V))
                {
                    m_honkAnimation.Honk();
                }

                m_gameManager.m_gameUI.count += m_moveSpeed;
                if (m_isAlive)
                {
                    if (!m_isRunning)
                    {
                        if (m_gameManager.m_gameUI.count >= 180)
                        {
                            m_gameManager.m_gameUI.AddDistance();
                            m_gameManager.m_gameUI.count = 0;
                        }
                    }
                    else
                    {
                        if (m_gameManager.m_gameUI.count >= 190)
                        {
                            m_gameManager.m_gameUI.AddDistance();
                            m_gameManager.m_gameUI.count = 0;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_gameManager.m_isPaused)
                {
                    m_gameManager.UnpauseGame();
                }
                else if (!m_gameManager.m_isPaused)
                {
                    m_gameManager.PauseGame();
                }
            }
        }
        else
        {
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            m_gameManager.GameOver();

            m_animator.speed = 0;

            if (Input.GetKeyDown(KeyCode.R) && !m_isAlive)
            {
                m_gameManager.ResetGame();
            }
            if (Input.GetKeyDown(KeyCode.Q) && !m_isAlive)
            {
                // Closes game
                Debug.Log("Game Closed.");
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_isAlive && !m_obstacleCollision)
        {
            setSpeed();

            m_rigidbody.velocity = new Vector2(m_moveSpeed, m_jumpPower);

            if (!GroundCheck())
            {
                m_jumpPower -= 0.4f;
                if (m_jumpPower <= -9.8f)
                {
                    m_jumpPower = -9.7f;
                }
                   

                if (m_numberOfJumps == 2)
                {
                    m_numberOfJumps = 1;
                }
            }

            if (transform.position.x >= m_frozenPosX)
            {
                m_frozen = true;
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                m_rigidbody.constraints = RigidbodyConstraints2D.None;
            }
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (m_frozen == true && transform.position.x < 0)
            {
                m_frozenPosX = transform.position.x;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Block" && collision.gameObject.transform.position.y <= transform.position.y - 0.1f)
        {
            m_jumpPower = 0;
            m_numberOfJumps = 2;
        }

        if(collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("*Dies*");
            m_obstacleCollision = true;
            m_frozen = false;
            m_animator.speed = 0;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            m_isAlive = false;
        }
    }


    public void ResetGoose()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        transform.position = new Vector3(spawnPosX, spawnPosY, 0);
        m_isAlive = true;
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_animator.speed = 1;
        m_frozen = false;
        m_obstacleCollision = false;
        m_frozenPosX = 0.0f;

        m_animator.SetBool("Walking", true);
        m_animator.SetBool("Running", false);
    }
}