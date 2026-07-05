using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    
    public float walkSpeed = 7f;
    public float mouseSensitivity = 2f;
    
    
    public Transform cameraTransform; 

    private CharacterController controller;
    private float verticalLookRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        
        
        HandleSkills(); 
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

       
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
      
        move.y = -9.81f;

        controller.Move(move * walkSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

      
        transform.Rotate(Vector3.up * mouseX);

      
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * verticalLookRotation;
    }

    void HandleSkills()
    {
       
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            
        }

       
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            
        }
    }
}