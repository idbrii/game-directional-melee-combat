using UnityEngine;
using System.Collections;

public class LofiTransformAnimation : MonoBehaviour
{
    [Tooltip("How much angular movement to use for the attack animation.")]
    [Range(0.01f,360f)]
    public float totalAttackAnimRotation = 90f;

    [Tooltip("How much linear movement to use for the attack animation.")]
    [Range(0.01f,10f)]
    public float totalAttackAnimTranslation = 1f;

    private Vector3 damagerBasePosition;
    private Vector3 damagerTargetPosition;
    private float damagerDistanceToTravel = 0f;

    void Start()
    {
        damagerBasePosition = transform.position;
        damagerTargetPosition = damagerBasePosition + transform.rotation * Vector3.forward * totalAttackAnimTranslation;
        damagerDistanceToTravel = Vector3.Distance(damagerBasePosition, damagerTargetPosition);
    }

    public void UpdateWithProgress(float progress, float direction)
    {
        if (progress > 0.5f)
        {
            progress = 1f - progress;
        }

        UpdateRotationWithProgress(progress, direction);
        UpdateTranslationWithProgress(progress);
    }

    void UpdateRotationWithProgress(float spin_progress, float direction)
    {
        var v = transform.localEulerAngles;
        v.y = direction * totalAttackAnimRotation * spin_progress;
        transform.localEulerAngles = v;
    }

    void UpdateTranslationWithProgress(float move_progress)
    {
        transform.position = Vector3.MoveTowards(damagerBasePosition, damagerTargetPosition, damagerDistanceToTravel * move_progress);
    }
}
