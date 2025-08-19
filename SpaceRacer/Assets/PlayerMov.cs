using System.Collections;
using System.Linq.Expressions;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMov : MonoBehaviour
{
    public float speed = 100f;
    public float curSpeed = 6f;
    public float aceleration = 8f;
    public float XposEndArea;
    public float ZposEndArea;
    public float XposStartArea;
    public float ZposStartArea;
    public float lookSpeed = 12f;
    public float Acelerate;
    public float TurnSmoothTime = 0.2f;
    public float turnSmoothVelocity;
    public float HeightSpeed;
    public float acelerationPerSecond;
    public float DesacelerationPerSecond;
    public float maxSpeed;
    public Vector3 LastVec;
    public float angleZ;
    public float angleSpeed;
    public float angleTime = 0.2f;
    public Transform Nav;


    [Header("Effects")]
    public GameObject QuarentaRangeVel;
    public GameObject SetenaRangeVel;
    public GameObject CemRangeVel;
    public GameObject QuarentaFireVel;
    public GameObject SetenaFireVel;
    public GameObject CemFireVel;

    [Header("Important")]
    public Transform cameraX;
    public CinemachineCamera cinemachineFreeLook;
    public Rigidbody rb;
    public bool canMove;
    public bool newMove;
    public float YposWithoutFall;
    public float DownScaleFall;
    public float rotationSpeed = 720f;
    public float AngleX;
    public float angleY;
    public float SimulJump;
    public float ImpulseForce;
    public float ExplosionRadius;
    private bool isRuningAcelerate;
    public float rotateForward;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachineFreeLook.Target.TrackingTarget = Nav;
        speed = 150f;
     curSpeed = 0f;
        maxSpeed = 110f;
        rotateForward = 2f;
        ImpulseForce = 80f;
        ExplosionRadius = 10f;
        angleTime = 0.1f;
        acelerationPerSecond = 15f;
        DesacelerationPerSecond = 50f;
        aceleration = 8f; 
        lookSpeed = 180f;
        canMove = true;
        HeightSpeed = 8f;
        DownScaleFall = 10f;
        newMove = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector2 look = InputSystem.actions.FindAction("Look").ReadValue<Vector2>();
        Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        float Jump = InputSystem.actions.FindAction("Jump").ReadValue<float>();
        float Fly = InputSystem.actions.FindAction("Fly").ReadValue<float>();
        float horizontal = input.x;
        float vertical = input.y;
        if (InputSystem.actions.FindAction("Sprint").WasPerformedThisFrame())
        {
            Acelerate = 100f;
            transform.position += transform.forward * ImpulseForce;
        }
        if (horizontal != 0f)
        {
            AngleX = Mathf.SmoothDampAngle(Nav.eulerAngles.x, transform.eulerAngles.x, ref turnSmoothVelocity, TurnSmoothTime);
            angleY = Mathf.SmoothDampAngle(Nav.eulerAngles.y, transform.eulerAngles.y, ref turnSmoothVelocity, TurnSmoothTime);
            if (horizontal >0f)
            {
                angleZ = Mathf.SmoothDampAngle(Nav.eulerAngles.z, -90f, ref turnSmoothVelocity, TurnSmoothTime);
                //rb.MovePosition(rb.position + Vector3.right * speed/2 * Time.fixedDeltaTime);
            }
            else
            {
                angleZ = Mathf.SmoothDampAngle(Nav.eulerAngles.z, 90f, ref turnSmoothVelocity, TurnSmoothTime);
                //rb.MovePosition(rb.position + Vector3.left * speed / 2 * Time.fixedDeltaTime);
            }

            transform.Rotate(0, horizontal * lookSpeed * Time.fixedDeltaTime, 0f);
            Nav.rotation = Quaternion.Euler(0f, angleY, angleZ);

        }
        if (vertical > 0.1f)
        {
            if (Acelerate < 70f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 60f;
                QuarentaFireVel.SetActive(true);
            }
            if (Acelerate >= 40f && Acelerate < 70f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 80f;
                QuarentaRangeVel.SetActive(true);
                SetenaRangeVel.SetActive(false);
                CemRangeVel.SetActive(false);
                SetenaFireVel.SetActive(false);
                CemFireVel.SetActive(false);
            }
            if (Acelerate >= 70f && Acelerate < 100f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 90f;
                SetenaRangeVel.SetActive(true);
                SetenaFireVel.SetActive(true);
                QuarentaRangeVel.SetActive(false);
                CemRangeVel.SetActive(false);
                QuarentaFireVel.SetActive(false);
                CemFireVel.SetActive(false);
            }
            if (Acelerate == 100f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 120f;
                CemRangeVel.SetActive(true);
                CemFireVel.SetActive(true);
                QuarentaRangeVel.SetActive(false);
                SetenaRangeVel.SetActive(false);
                QuarentaFireVel.SetActive(false);
                SetenaFireVel.SetActive(false);
            }

        }
        else
        {
            if (Acelerate < 70f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 60f;
                QuarentaFireVel.SetActive(true);
            }
            if (Acelerate >= 40f && Acelerate < 70f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 80f;
                QuarentaFireVel.SetActive(true);

                QuarentaRangeVel.SetActive(false);
                SetenaRangeVel.SetActive(false);
                CemRangeVel.SetActive(false);
                SetenaFireVel.SetActive(false);
                CemFireVel.SetActive(false);
            }
            if (Acelerate >= 70f && Acelerate < 100f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 90f;
                QuarentaFireVel.SetActive(true);
                SetenaRangeVel.SetActive(false);
                SetenaFireVel.SetActive(true);
                QuarentaRangeVel.SetActive(false);
                CemRangeVel.SetActive(false);
                CemFireVel.SetActive(false);
            }
            if (Acelerate == 100f)
            {
                cinemachineFreeLook.Lens.FieldOfView = 120f;
                QuarentaFireVel.SetActive(true);
                CemRangeVel.SetActive(false);
                CemFireVel.SetActive(true);
                QuarentaRangeVel.SetActive(false);
                SetenaRangeVel.SetActive(false);
                SetenaFireVel.SetActive(false);
            }

        }
            //peguei valores do input system novo
            //fiz duas variaveis para horizontal e vertical desse Vector 2 (x,y)
            /*if (transform.position.y >= YposWithoutFall && Fly == 0f && transform.position.y >0f)
            {
                rb.AddForce(Vector3.down* DownScaleFall);
            }*/
            //fiz um vector 3 com esses valores normalizados para ser a direcao do movimento
            Vector3 direction = new Vector3(0f, 0f, vertical).normalized;
        //se a direcao for maior que 0.1, ou seja, se o player estiver se movendo
        
        if (direction.z != 0 || Fly != 0 || horizontal != 0)
        {
            if (newMove)
            {
                curSpeed = speed - (aceleration * 10);
                curSpeed = Mathf.Clamp(curSpeed, 20f, speed);
                newMove = false;
            }
            if (direction.z < -0.1f)
            {
                Vector3 finalMoveS = new Vector3(LastVec.x, Fly, LastVec.z);
                if (curSpeed > 0)
                {
                    float Desacelerate = DesacelerationPerSecond + 20f;
                    curSpeed -= Desacelerate * Time.deltaTime;
                    Acelerate = Mathf.RoundToInt(curSpeed / maxSpeed * 100);
                    rb.MovePosition(rb.position + finalMoveS.normalized * curSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    //rb.linearVelocity -= 0.1f * rb.linearVelocity;
                    //rb.linearVelocity = rb.linearVelocity * 0.95f * Time.deltaTime;      //Edit 0.95 with the value you want as long as it is smaller than 1 and bigger than 0
                    QuarentaRangeVel.SetActive(false);
                    SetenaRangeVel.SetActive(false);
                    CemRangeVel.SetActive(false);
                    CemFireVel.SetActive(false);
                    SetenaFireVel.SetActive(false);
                    newMove = true;
                    curSpeed = 0f;
                    Acelerate = 0f;
                }
            }
            else
            {
                //Vector3 finalMove = transform.forward * vertical + transform.up * (Fly + SimulJump);
                Vector3 finalMove = transform.rotation * new Vector3(0, 0, rotateForward)  + transform.right * horizontal + transform.up * (Fly + SimulJump);
                if (curSpeed < maxSpeed)
                {
                    curSpeed += acelerationPerSecond * Time.deltaTime;
                }
                else
                {
                    curSpeed = maxSpeed;
                }
                Acelerate = Mathf.RoundToInt((curSpeed / maxSpeed) * 100);
                LastVec = finalMove;
                //transform.position +=  * Time.fixedDeltaTime;

                rb.MovePosition(rb.position + finalMove.normalized * curSpeed * Time.fixedDeltaTime);


            }
        }



        
        else
        {
            Vector3 finalMove = new Vector3(LastVec.x, Fly, LastVec.z);
            if (curSpeed > 0)
            {
                curSpeed -= DesacelerationPerSecond * Time.deltaTime;
                Acelerate = Mathf.RoundToInt(curSpeed / maxSpeed * 100);
                rb.MovePosition(rb.position + finalMove.normalized * curSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //rb.linearVelocity -= 0.1f * rb.linearVelocity;
                //rb.linearVelocity = rb.linearVelocity * 0.95f * Time.deltaTime;      //Edit 0.95 with the value you want as long as it is smaller than 1 and bigger than 0
                QuarentaRangeVel.SetActive(false);
                SetenaRangeVel.SetActive(false);
                CemRangeVel.SetActive(false);
                CemFireVel.SetActive(false);
                SetenaFireVel.SetActive(false);
                newMove = true;
                curSpeed = 0f;
                Acelerate = 0f;
            }
        }
        //limit player movement
        /*float myPosX = transform.position.x;
        float myPosZ = transform.position.z;
        Vector3 LimitedMov;
        if (myPosX < 4)
        {
            LimitedMov.x = 4;
            transform.position = new Vector3(myPosX, transform.position.y, myPosZ);
        }
        if (myPosX >490)
        {
            LimitedMov.x = 490f;
            transform.position = new Vector3(myPosX, transform.position.y, myPosZ);
        }
        if (myPosZ < -14)
        {
            LimitedMov.z = -14;
            transform.position = new Vector3(myPosX, transform.position.y, myPosZ);
        }
        if (myPosZ > 520)
        {
            LimitedMov.z = 520;
            transform.position = new Vector3(myPosX, transform.position.y, myPosZ);
        }*/
    }
}
