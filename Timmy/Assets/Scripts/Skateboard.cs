using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public float rotationSpeed;
    public Transform rotateTrans;

    public Transform riggedTransform;

    public PlayerController playerController;

    public Vector3 crouchRotationOrienation;
    public Vector3 normalRotationOrientaion;
    
    public Vector3 crouchPosition;
    public Vector3 normalPosition;


    public Vector3 riggedObjectOrientationNormal;
    public Vector3 riggedObjectOrientationCrouch;

    public Vector3 riggedObjectPositionNormal;
    public Vector3 riggedObjectPositionCrouch;
    private void FixedUpdate()
    {
        if (playerController.animator.GetBool("air"))
        {
            rotateTrans.Rotate(rotateTrans.forward, rotationSpeed * Time.fixedDeltaTime);
            rotateTrans.localPosition = normalPosition;
            riggedTransform.rotation = Quaternion.Euler(riggedObjectOrientationNormal);
            riggedTransform.localPosition = riggedObjectPositionNormal;
        }
        else
        {
            if (playerController.animator.GetBool("slide"))
            {
                rotateTrans.rotation = Quaternion.Euler(crouchRotationOrienation);
                riggedTransform.rotation = Quaternion.Euler(riggedObjectOrientationCrouch);
                rotateTrans.localPosition = crouchPosition;

                riggedTransform.localPosition = riggedObjectPositionCrouch;
            }
            else
            {
                rotateTrans.rotation = Quaternion.Euler(normalRotationOrientaion);
                riggedTransform.rotation = Quaternion.Euler(riggedObjectOrientationNormal);
                rotateTrans.localPosition = normalPosition;
                riggedTransform.localPosition = riggedObjectPositionNormal;
            }
        }
    }
}
