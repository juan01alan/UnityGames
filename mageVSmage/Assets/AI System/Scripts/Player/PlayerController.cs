using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


    public class PlayerController : MonoBehaviour {

        public CharacterController CharacterController;
        public Animator animator;
    public float rollForce = 10f;
    public bool rolling = false;
    public spellcaster spellcaster;
    public bool ShouldCamGet;
    public Vector3 movement;
    public bool Ivulnerable = false;
    public Quaternion targetRotation;
    public Transform tranformToRoll;
    public Transform transformToLook;
    public Camera targetCam;
    [Header("Player Settings")]
        [Range(0.5f, 10f)]
        [SerializeField] float movementSpeed = 2f;
        [Range(2f, 50f)]
        [SerializeField] float runningSpeed = 10f;
        [SerializeField] float rotationSpeed = 240;

        public Camera MainCam;
        private float rotY;
        public Transform rotateOnlyMesh;
        private float currentSpeed;

        private Rigidbody rb;

        private void Start() {
            rb = GetComponent<Rigidbody>();
        if (ShouldCamGet)
        {
            targetCam = Camera.main;
        }
        }
    public void resetRoll()
    {
        rb.linearVelocity = Vector3.zero;
        rolling = false;
    }

    [System.Obsolete]
    void SetRotate(GameObject toRotate, GameObject camera)
    {
        Vector2 lookI = InputSystem.actions.FindAction("look").ReadValue<Vector2>();
        if (lookI.magnitude > 0.3f)
        {
            Vector3 directionToCamera = camera.transform.forward;
            directionToCamera.y = 0; // Keep the direction on the horizontal plane
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
            targetRotation.x = 0; // Prevent tilt
                                  //You can call this function for any game object and any camera, just change the parameters when you call this function

            transform.rotation = Quaternion.Lerp(toRotate.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }
    public void RollC() {

        rolling = true;
        spellcaster.Ivulnerable = true;
        spellcaster.ResetIvul();
        rb.linearVelocity = Vector3.zero;
        //rb.MovePosition(transform.position);
        rb.AddForce(tranformToRoll.forward * rollForce, ForceMode.Impulse);
        StartCoroutine(ResetRoll());
    }
    private IEnumerator ResetRoll()
    {
        yield return new WaitForSeconds(1);
        rolling = false;
    }
    private void Roll()
    {
        if (!rolling)
        {
            if (tranformToRoll != null)
            {
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack3", false);
                animator.SetBool("Walk", false);
                animator.SetTrigger("Roll");
            }
            else
            {
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack3", false);
                animator.SetBool("Walk", false);
                animator.SetTrigger("Roll");


            }
        }
    }

        private void FixedUpdate() {
            Vector2 move = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
            float x = move.x;
            float y = move.y;

        if (transformToLook!= null)
        {
            if (!rolling)
            {

                movement = transformToLook.right * x + transformToLook.forward * y;
            }

        }
        else
        {
            if (!rolling)
            {

                movement = transform.right * x + transform.forward * y;
            }
        }

            movement = movement.normalized;

            if (movement.magnitude > 1f) {
                movement = movement.normalized;
            }
            if (movement.magnitude < 0.1f) {
                movement = Vector3.zero;
            }
            float Shift = InputSystem.actions.FindAction("Sprint").ReadValue<float>();

            currentSpeed = Shift > 0 ? runningSpeed : movementSpeed;

            movement.y = 0; // Keep movement on the horizontal plane
            movement.Normalize(); // Ensure consistent speed in all directions
        if (!rolling)
        {
            //CharacterController.Move(movement * currentSpeed * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
        }
        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
            {
            if (targetCam != null)
            {
                if (rotateOnlyMesh != null)
                {
                   /* Vector3 directionToCamera = targetCam.transform.forward;

                    movement.z = directionToCamera.z; // Ensure movement is only in the XZ plane*/
                    targetRotation = Quaternion.LookRotation(movement);
                    //prevent tilt
                    targetRotation.x = 0;
                    rotateOnlyMesh.rotation = Quaternion.RotateTowards(rotateOnlyMesh.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                }
                /* Vector3 directionToCamera = targetCam.transform.position - transform.position;

                 // Create a rotation that looks along this direction, keeping the object's up aligned with world up
                 targetRotation = Quaternion.LookRotation(targetCam.transform.position, Vector3.up);*/


            }
            else
            {
                // If no camera is set, just look in the direction of movement
                targetRotation = Quaternion.LookRotation(movement);
                //prevent tilt
                targetRotation.x = 0;
                if (rotateOnlyMesh != null)
                {
                    rotateOnlyMesh.rotation = Quaternion.RotateTowards(rotateOnlyMesh.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                }
            }
            }
            if (movement != Vector3.zero && !rolling)
        {
            animator.SetBool("Walk", true);
                Vector3 velocity = new Vector3(movement.x, transform.position.y, movement.z);
            //CharacterController.Move(velocity * currentSpeed * Time.fixedDeltaTime);
        } else
        {
            animator.SetBool("Walk", false);

        }
    }

        private void Update() {
        if (InputSystem.actions.FindAction("Roll").WasPerformedThisFrame() && !rolling)
        {
            Roll();
        }
        if (targetCam !=null)
        {
            SetRotate(gameObject, targetCam.gameObject);
        }
        //rotY += InputSystem.actions.FindAction("Look").ReadValue<Vector2>().x * Time.deltaTime * rotationSpeed;

        /*Vector2 look = InputSystem.actions.FindAction("Look").ReadValue<Vector2>();
        float h = look.x;
        float v = look.y;
        Vector3 moveDirection = MainCam.transform.forward * v + MainCam.transform.right * h;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //prevent tilt
            targetRotation.x = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }*/

        //transform.rotation = Quaternion.Euler(0f, rotY, 0f);
    }
    }