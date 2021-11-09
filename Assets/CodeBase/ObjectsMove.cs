using UnityEngine;

namespace CodeBase
{
    public class ObjectsMove : CopyCameraMoveBase
    {
        public bool TrackRotation;
        public bool TrackPosition;
        [SerializeField] private float RotateSpeed;

        protected override void SpecialMoveOnUpdate()
        {
            if (TrackRotation)
            {
                if (RotateSpeed > 0)
                {
                    transform.localRotation = Quaternion.Lerp(transform.rotation, TrackedObject.rotation, Mathf.SmoothStep(0.0f, 1.0f, RotateSpeed));
                }
                else
                {
                    if (transform.parent != TrackedObject)
                    {
                        transform.localRotation = TrackedObject.rotation;
                    }
                }
            }

            if (TrackPosition)
            {
                transform.position = TrackedObject.position;
            }
        }
    }
}
