using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float speed = 10f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;
    public bool PlayerCode = true;
    public GameObject pauseMenu;
    private bool isPaused;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayer;
    public AudioSource audiosource;
    public AudioClip timeStart;
    public AudioClip timeEnd;
    public AudioClip Incorrect;
    public AudioClip correct;

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

    private float timeLeftWithBuff;
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
    public GameObject timeObject;
    public Image timerBar;

    public bool secondPuzzleSolved;

    [Header("ThirdPuzzle")]

    public GameObject levelThreeDoor;
    public GameObject levelThreePlatforms;

    void Start()
    {
        correctOrder = false;
        firstPuzzleComplete = false;
        levelTwoDoor.SetActive(true);
        levelTwoSteps.SetActive(false);
        secondPuzzleSolved = false;
        wiring.SetActive(false);
        levelThreeDoor.SetActive(true);
        levelThreePlatforms.SetActive(false);
        pauseMenu.SetActive(false);
        isPaused = false;
        timeLeftWithBuff = buffTimeLimit;
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

        if(secondPuzzleSolved == true)
        {
            levelThreeDoor.SetActive(false);
        }

        if (hasBuff == true)
        {
            timeObject.SetActive(true);  
            timeLeftWithBuff -= Time.deltaTime;
            wiring.SetActive(true);
            levelThreePlatforms.SetActive(true);
            timerBar.fillAmount = timeLeftWithBuff / buffTimeLimit;
        }

        if (timeLeftWithBuff <= 0f)
        {
            timeObject.SetActive(false);
            timeLeftWithBuff = buffTimeLimit;
            hasBuff = false;
            wiring.SetActive(false);
            levelThreePlatforms.SetActive(false);
            audiosource.PlayOneShot(timeEnd);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                isPaused = true;
                pauseMenu.SetActive(true);
            }
            else if (isPaused == true)
            {
                Time.timeScale = 1;
                isPaused = false;
                pauseMenu.SetActive(false);
            }
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
        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        FirstPuzzle(collision);

        SecondPuzzle(collision);

        if (collision.gameObject.name == "Secret" && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false)
        {
            SceneManager.LoadScene("Secret");
        }
    }

    public void FirstPuzzle(Collider2D collision)
    {
        if (collision.gameObject == firstPlate && Input.GetKeyDown(KeyCode.B) && isPaused == false && plateOneTouched == false)
        {
            correctOrder = true;
            firstCheck.SetActive(true);
            plateOneTouched = true;
            correctSound();
        }

        if (collision.gameObject == secondPlate && plateOneTouched == true && Input.GetKeyDown(KeyCode.B) && isPaused == false && plateTwoTouched == false)
        {
            secondCheck.SetActive(true);
            plateTwoTouched = true;
            correctSound();
        }
        else if (collision.gameObject == secondPlate && plateOneTouched == false && Input.GetKeyDown(KeyCode.B) && isPaused == false)
        {
            correctOrder = false;
            incorrectSound();
        }

        if (collision.gameObject == thirdPlate && plateTwoTouched == true && Input.GetKeyDown(KeyCode.B) && isPaused == false && firstPuzzleComplete == false)
        {
            thirdCheck.SetActive(true);
            firstPuzzleComplete = true;
            correctSound();
        }
        else if (collision.gameObject == thirdPlate && plateTwoTouched == false && Input.GetKeyDown(KeyCode.B) && isPaused == false)
        {
            plateOneTouched = false;
            correctOrder = false;
            incorrectSound();
        }
    }

    public void SecondPuzzle(Collider2D collision)
    {
        if (collision.gameObject == sightTrigger && Input.GetKeyDown(KeyCode.B) && isPaused == false && hasBuff == false)
        {
            hasBuff = true;
            audiosource.PlayOneShot(timeStart);
        }

        if (collision.gameObject == FirstConnect && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false && firstConnected == false)
        {
            firstColor.color = Color.green;
            firstConnected = true;
            correctSound();
        }

        if (collision.gameObject == SecondConnect && firstConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false && secondConnected == false)
        {
            secondColor.color = Color.green;
            secondConnected = true;
            correctSound();
        }
        else if (collision.gameObject == SecondConnect && firstConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false)
        {
            incorrectSound();
        }

        if (collision.gameObject == ThirdConnect && secondConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false && thirdConnected == false)
        {
            thirdColor.color = Color.green;
            thirdConnected = true;
            correctSound();
        }
        else if (collision.gameObject == ThirdConnect && secondConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false)
        {
            firstConnected = false;
            firstColor.color = Color.red;
            incorrectSound();
        }

        if (collision.gameObject == fourthConnect && thirdConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false && fourthConnected == false)
        {
            fourthColor.color = Color.green;
            fourthConnected = true;
            correctSound();
        }
        else if (collision.gameObject == fourthConnect && thirdConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false)
        {
            firstConnected = false;
            firstColor.color = Color.red;
            secondConnected = false;
            secondColor.color = Color.red;
            incorrectSound();
        }

        if (collision.gameObject == EndConnect && fourthConnected == true && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false && secondPuzzleSolved == false)
        {
            endColor.color = Color.green;
            secondPuzzleSolved = true;
            correctSound();
        }
        else if (collision.gameObject == EndConnect && fourthConnected == false && Input.GetKeyDown(KeyCode.B) && hasBuff && isPaused == false)
        {
            firstConnected = false;
            firstColor.color = Color.red;
            secondConnected = false;
            secondColor.color = Color.red;
            thirdConnected = false;
            thirdColor.color = Color.red;
            incorrectSound();
        }
    }

    public void correctSound()
    {
        audiosource.PlayOneShot(correct);
    }

    public void incorrectSound()
    {
        audiosource.PlayOneShot(Incorrect);
    }
}
