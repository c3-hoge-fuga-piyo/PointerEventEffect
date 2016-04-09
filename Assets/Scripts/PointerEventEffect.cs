using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public sealed class PointerEventEffect : BaseRaycaster
{
    #region Unity Inspectors.
    [SerializeField]
    ParticleSystem tap;

    [SerializeField]
    ParticleSystem swipe;
    #endregion

    #region Unity Messages.
    protected override void Awake()
    {
        base.Awake();

        this.cachedCamera = this.GetComponent<Camera>();
    }
    #endregion

    bool lastState = false;

    Camera cachedCamera;

    #region BaseRaycaster
    public override Camera eventCamera { get { return this.cachedCamera; } }

    public override void Raycast(
        PointerEventData eventData,
        List<RaycastResult> resultAppendList)
    {
        var e = eventData.eligibleForClick;
        try
        {
            if (e)
            {
                var ep = eventData.position;
                var p = this.eventCamera.ScreenToWorldPoint(
                        new Vector3(
                            ep.x,
                            ep.y,
                            -this.eventCamera.transform.position.z));

                this.swipe.transform.position = p;
                if (!this.lastState)
                {
                    // Down
                    this.swipe.Play();

                    this.tap.transform.position = p;
                    this.tap.Play();
                }

                return;
            }

            if (this.lastState)
            {
                // Up
                this.swipe.Stop();
            }
        }
        finally
        {
            this.lastState = e;
        }
    }
    #endregion
}
