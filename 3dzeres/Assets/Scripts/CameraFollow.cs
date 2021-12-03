using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow inst;
    public bool puzzleStarted2 = false;
    public Transform Target;
    public Transform camTransform;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
 
    private void Start()
    {
        inst = this;
        Offset = camTransform.position - Target.position;
    }

    private void Update()
    {
        if (Player.player.inBibliot) {
            if (!Player.player.puzzle.activeInHierarchy) {
                transform.localPosition = new Vector3(742.4099f, 16.43828f, 393.0443f);
                transform.localRotation = Quaternion.Euler(19.67f, 180, 0);
                Offset = new Vector3(0, 14.5f, 14);
            }
            else {
                if (!puzzleStarted2) {
                    transform.localRotation = Quaternion.Euler(90, transform.localRotation.y, transform.localRotation.z);
                    Offset = new Vector3(0, 60, Offset.z);
                }
                else {
                    Offset = new Vector3(0, 60, 5);
                }
            }
            SmoothTime = 0;
        }
    }
 
    private void LateUpdate()
    {
        Vector3 targetPosition = Target.position + Offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        if (!Player.player.inBibliot) transform.LookAt(Target);
    }
}
