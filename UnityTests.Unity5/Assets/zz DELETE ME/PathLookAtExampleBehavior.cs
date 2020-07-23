using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathLookAtExampleBehavior : MonoBehaviour
{
    public Transform CameraTransform;
    public Transform LookAtTarget;
    public float PathDuration;
    public Transform[] Path;

    private Vector3 _initialCameraPosition;
    private Quaternion _initialCameraRotation;
    private Tweener _tweener;

    public void Start()
    {
        _initialCameraPosition = CameraTransform.position;
        _initialCameraRotation = CameraTransform.rotation;
    }

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }

            CameraTransform.position = _initialCameraPosition;
            CameraTransform.rotation = _initialCameraRotation;

            _tweener = CameraTransform
                .DOPath(
                    path: new Vector3[] { new Vector3(0, 2, 0), new Vector3(0, 2, 0), new Vector3(0, 4, 0) }, 
                    duration: PathDuration,
                    pathType: PathType.CubicBezier)
                .SetLookAt(LookAtTarget.position, true)
                /*.SetOptions(
                    lockPosition: AxisConstraint.None,
                    lockRotation: AxisConstraint.Z | AxisConstraint.W | AxisConstraint.X | AxisConstraint.Y)*/
                .OnKill(() => _tweener = null);
        }
    }
}
