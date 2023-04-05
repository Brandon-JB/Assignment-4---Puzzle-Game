using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float speed = 10f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;
    public bool PlayerCode = true;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] GameObject GameOverScreen;
    public GameObject AudioObject;
    

    [Header ("FirstPuzzle")]

    public GameObject firstPlate;
    public GameObject secondPlate;
    public GameObject thirdPlate;
    private bool correctOrder;
    public GameObject firstCheck;
    public GameObject secondCheck;
    public GameObject thirdCheck;
    public bool plateOneTouched;
    public bool plateTwoTouched;
    public bool firstPuzzleComplete;

    [Header("SecondPuzzle")]

    public GameObject levelTwoSteps;
    public GameObject levelTwoDoor;

    private float timeWithBuff;
    public float buffTimeLimit;
    public GameObject wiring;
    private bool hasBuff = false;

    public GameObject sightTrigger;
    public SpriteRenderer firstColor;
    public GameObject FirstConnect;
    private bool firstConnected = false;
    public SpriteRenderer secondColor;
    public GameObject SecondConnect;
    private bool secondConnected = false;
    public SpriteRenderer thirdColor;
    public GameObject ThirdConnect;
    private bool thirdConnected = false;
    public SpriteRenderer fourthColor;
    public GameObject fourthConnect;
    private bool fourthConnected = false;
    public SpriteRenderer endColor;
    public GameObject EndConnect;

    public bool secondPuzzleSolved;

    void Start()
    {
        correctOrder = false;
        firstPuzzleComplete = false;
        levelTwoDoor.SetActive(true);
        levelTwoSteps.SetActive(false);
        secondPuzzleSolved = false;
        wiring.SetActive(false);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown("escape"))
        {
            GameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }

        flip();

        if (correctOrder == false)
        {
            firstCheck.SetActive(false);
            secondCheck.SetActive(false);
            thirdCheck.SetActive(false);
        }

        if (firstPuzzleComplete == true)
        {
            levelTwoDoor.SetActive(false);
            levelTwoSteps.SetActive(true);
        }

        if (hasBuff == true)
        {
            timeWithBuff += Time.deltaTime;
            wiring.SetActive(true);
        }

        if (timeWithBuff >= buffTimeLimit)
        {
            timeWithBuff = 0f;
            hasBuff = false;
            wiring.SetActive(false);
        }
    }

    private void fixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundlayer);
    }

    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;

            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;

            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spikes")
        {
            GameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        FirstPuzzle(collision);

        SecondPuzzle(collision);
    }

    public void FirstPuzzle(Collider2D collision)
    {
        if (collision.gameObject == firstPlate && Input.GetKeyDown(KeyCode.B))
        {
            correctOrder = true;
            firstCheck.SetActive(true);
            plateOneTouched = true;
        }

        if (collision.gameObject == secondPlate && plateOneTouched == true && Input.GetKeyDown(KeyCode.B))
        {
            secondCheck.SetActive(true);
            plateTwoTouched = true;
        }
        else if (collision.gameObject == secondPlate && plateOneTouched == false && Input.GetKeyDown(KeyCode.B))
        {
            correctOrder = false;
        }

        if (collision.gameObject == thirdPlate && plateTwoTouched == true && Input.GetKeyDown(KeyCode.B))
        {
            thirdCheck.SetActive(true);
            firstPuzzleComplete = true;
        }
        else if (collision.gameObject == thirdPlate && plateTwoTouched == false && Input.GetKeyDown(KeyCode.B))
        {
            plateOneTouched = false;
            correctOrder = false;
        }
    }

    public void SecondPuzzle(Collider2D collision)
    {
        if (collision.gameObject == sightTrigger && Input.GetKeyDown(KeyCode.B))
        {
            hasBuff = true;
        }

        if (collision.gameObject == FirstConnect && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            firstColor.color = Color.green;
            firstConnected = true;
        }

        if (collision.gameObject == SecondConnect && firstConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            secondColor.color = Color.green;
            secondConnected = true;
        }

        if (collision.gameObject == ThirdConnect && secondConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            thirdColor.color = Color.green;
            thirdConnected = true;
        }
        else if (collision.gameObject == ThirdConnect && secondConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            firstConnected = false;
            firstColor.color = Color.red;
        }

        if (collision.gameObject == fourthConnect && thirdConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            fourthColor.color = Color.green;
            fourthConnected = true;
        }
        else if (collision.gameObject == fourthConnect && thirdConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            firstConnected = false;
            firstColor.color = Color.red;
            secondConnected = false;
            secondColor.color = Color.red;
        }

        if (collision.gameObject == EndConnect && fourthConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            endColor.color = Color.green;
            secondPuzzleSolved = true;
        }
        else if (collision.gameObject == EndConnect && fourthConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff)
        {
            firstConnected = false;
            firstColor.color = Color.red;
            secondConnected = false;
            secondColor.color = Color.red;
            thirdConnected = false;
            thirdColor.color = Color.red;
        }
    }
}
