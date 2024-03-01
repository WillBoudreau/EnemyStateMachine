using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField]  private float speed = 12f;
    [SerializeField]  private float gravity = -9.81f;
    [SerializeField]  private float jumpHeight = 1f;
    [SerializeField]  private CharacterController CharacterCont;
    [SerializeField]  private Transform groundCheck;
    public Vector3 StartPOS;
    public Transform movingPlatform;
    public float groundDist = 0.4f;
    public LayerMask GroundMask;
    bool IsGrounded;
    bool IsPaused;
    Vector3 velocity;
    int count = 0;
    [Header("UI")]
    public TextMeshProUGUI countText;
    public GameObject PauseMenu;
    // Update is called once per frame
    void Start()
    {
        IsPaused=false;
        PauseMenu.SetActive(false);
        StartPOS = transform.position;
        CharacterCont = gameObject.GetComponent<CharacterController>();
        Debug.Log(StartPOS);
    }
    void Update()
    {
        //Movement
        IsGrounded = Physics.CheckSphere(groundCheck.position,groundDist,GroundMask);

        if(IsGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        CharacterCont.Move(move * Time.deltaTime * speed);
        
        velocity.y += gravity * Time.deltaTime;

        CharacterCont.Move(velocity * Time.deltaTime * speed);

        //Keybinds
        //Jump if spacebar is pressed
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight* -0.25f* gravity);
        }
        //Pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            PauseMenu.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            PauseMenu.SetActive(false);
        }
        //Kill box
        if(velocity.y <= -15)
        {   
            transform.position = StartPOS;
        }
    }
        //Triggers
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Checkpoint"))
            {
                Debug.Log("Checkpoint!");
                StartPOS = transform.position;
                Debug.Log(StartPOS);
            }
            if (other.gameObject.CompareTag("Collectable"))
            {
                Debug.Log("Hi");
                count++;
                countText.text = "Collectables Collected: " + count.ToString();
            }
        }
}
