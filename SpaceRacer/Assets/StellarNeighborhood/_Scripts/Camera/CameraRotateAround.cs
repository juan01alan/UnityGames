using UnityEngine;

namespace StellarNeighborhood {
public class CameraRotateAround : MonoBehaviour
{
    [SerializeField] private bool startRotating = true;
    [SerializeField] private float autoXRotateSpeed = 2f;
    [SerializeField] private float playerXRotateSpeed = 64f;
    [SerializeField] private float playerYRotateSpeed = 64f;
    [SerializeField] private float zZoomSpeed = 128f;
    [SerializeField] private Vector2 distanceClamp = new Vector2(4f, 64f);

    [SerializeField] private Vector2 yRotateClamp = new Vector2(45f, 135f);

    private Vector3 inputDirection;
    private Transform thisT;
    private Transform target;

    private bool autoRotate;

    private void Start()
    {
        initialize();
    }    

    private void Update()
    {
        GetInput();
    }
    private void LateUpdate()
    {
        if (target == null)
            return;

        HandleMovement();
    }

    private void initialize()
    {
        inputDirection = Vector3.zero;
        thisT = transform;

        if (thisT.parent != null)
        {
            target = thisT.parent;
        }

        autoRotate = startRotating;
    }

    private void GetInput()
    {
        inputDirection = Vector3.zero;

        float x = 0;
        float y = 0;
        float z = 0;

        if(Input.GetMouseButton(0))
        {
            x = Input.GetAxis("Mouse X") * playerXRotateSpeed * Time.deltaTime;
            y = Input.GetAxis("Mouse Y") * playerYRotateSpeed * Time.deltaTime;
        }
        else if(autoRotate)
        {
            x = autoXRotateSpeed * Time.deltaTime;
        }

        z -= Input.GetAxis("Mouse ScrollWheel") * zZoomSpeed * Time.deltaTime;

        inputDirection = new Vector3(x, -y, z);

        if (Input.GetMouseButtonDown(1))
            autoRotate = !autoRotate;
    }
    private void HandleMovement()
    {       
        float distance = Vector3.Distance(thisT.position, target.position);

        if(distance < distanceClamp.x) {
            if(inputDirection.z > 0f)
                thisT.position += -transform.forward * inputDirection.z * zZoomSpeed * (distance) * Time.deltaTime;
        }
        else if(distance > distanceClamp.y) {
            if(inputDirection.z < 0f)
                thisT.position += -transform.forward * inputDirection.z * zZoomSpeed * (distance) * Time.deltaTime;
        }
        else
        {
            thisT.position += -transform.forward * inputDirection.z * zZoomSpeed * (distance) * Time.deltaTime;
        }

        Vector3 localUp = target.up;
        Vector3 localRight = thisT.right;        

        Vector3 localDirection = thisT.position - target.position;
        float angle = Vector3.Angle(localDirection, localUp);

        if (angle < yRotateClamp.x)
        {
            if (inputDirection.y < 0f)
            {
                thisT.RotateAround(target.position, localRight, inputDirection.y);
            }
        }
        else if (angle > yRotateClamp.y)
        {
            if (inputDirection.y > 0f)
            {
                thisT.RotateAround(target.position, localRight, inputDirection.y);
            }
        }
        else
        {
            thisT.RotateAround(target.position, localRight, inputDirection.y);
        }

        thisT.RotateAround(target.position, localUp, inputDirection.x);
    }       
}
}