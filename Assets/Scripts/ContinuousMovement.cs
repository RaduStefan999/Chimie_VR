using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{

    // Input Sources
    public XRNode horizontalInputSource;
    public XRNode verticalInputSource;

    // Locomotion Constants
    public float horizontalSpeed = 1;
    public float verticalSpeed = 1;
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    private XRRig rig;
    private float fallingSpeed;
    private Vector2 horizontalInputAxis;
    private Vector2 verticalInputAxis;
    private CharacterController character;
    private GameObject cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        cameraOffset = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice horizontalDevice = InputDevices.GetDeviceAtXRNode(horizontalInputSource);
        InputDevice verticalDevice = InputDevices.GetDeviceAtXRNode(verticalInputSource);

        horizontalDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out horizontalInputAxis);
        verticalDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out verticalInputAxis);
    }

    private void FixedUpdate()
    {
        bool isGrounded = CheckIfGrounded();

        //horizontal movement
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(horizontalInputAxis.x, 0, horizontalInputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * horizontalSpeed);

        //vertical movement
        float verticalChange = verticalInputAxis.y * Time.fixedDeltaTime * verticalSpeed;
        Vector3 verticalScale = cameraOffset.transform.localScale + new Vector3(verticalChange, verticalChange, verticalChange);

        verticalScale.x = Mathf.Clamp(verticalScale.x, 1f, 3f);
        verticalScale.y = Mathf.Clamp(verticalScale.y, 1f, 3f);
        verticalScale.z = Mathf.Clamp(verticalScale.z, 1f, 3f);
        
        cameraOffset.transform.localScale = verticalScale;

        //gravity
        if (isGrounded) {
            fallingSpeed = 0;
        }
        else {
            fallingSpeed = fallingSpeed + gravity * Time.fixedDeltaTime;
        }
        
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset() 
    {
        character.height = rig.cameraInRigSpaceHeight + 0.2f;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    bool CheckIfGrounded() 
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
