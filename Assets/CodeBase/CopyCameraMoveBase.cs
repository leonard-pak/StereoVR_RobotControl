using UnityEngine;

namespace CodeBase
{
    public abstract class CopyCameraMoveBase : MonoBehaviour
    {
        protected Transform TrackedObject;
        protected virtual void Awake()
        {
            if (Camera.main is { })
            {
                TrackedObject = Camera.main.transform;
            }
        }

        protected virtual void Update()
        {
            SpecialMoveOnUpdate();
        }

        protected abstract void SpecialMoveOnUpdate();
    }
}
