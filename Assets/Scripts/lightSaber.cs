using UnityEngine;

public class VRLightsaberAnimatorController : MonoBehaviour
{
    [Header("References")]
    public Animator handAnimator;          // Animator with your blend tree
    public GameObject lightsaber;
    public Transform handTransform;
    public AudioSource musicPlayer;
    public AudioClip darkSideMusic;

    [Header("Settings")]
    public float throwDistance = 5f;
    public float throwSpeed = 10f;
    public float returnSpeed = 15f;
    public float spinSpeed = 720f;
    public float stopDistance = 0.3f;

    private enum HandState { Idle, Fist, Point }
    private HandState currentState = HandState.Idle;
    private HandState lastState = HandState.Idle;

    private Vector3 targetPosition;
    private bool isFlying = false;

    void Start()
    {
        if (lightsaber != null)
            lightsaber.SetActive(false);
    }

    void Update()
    {
        UpdateHandState();
        HandleLightsaber();
    }

    void UpdateHandState()
    {
        // Get current animator state
        AnimatorStateInfo stateInfo = handAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("fist"))
            currentState = HandState.Fist;
        else if (stateInfo.IsName("point"))
            currentState = HandState.Point;
        else
            currentState = HandState.Idle;

        if (currentState != lastState)
        {
            OnStateChanged(currentState);
            lastState = currentState;
        }
    }

    void OnStateChanged(HandState newState)
    {
        if (newState == HandState.Fist)
        {
            lightsaber.SetActive(true);

            if (darkSideMusic != null && musicPlayer != null)
            {
                musicPlayer.clip = darkSideMusic;
                if (!musicPlayer.isPlaying)
                    musicPlayer.Play();
            }

            isFlying = false;
            targetPosition = handTransform.position;
        }
        else if (newState == HandState.Point)
        {
            isFlying = true;
            targetPosition = handTransform.position + handTransform.forward * throwDistance;
        }
        else if (newState == HandState.Idle)
        {
            lightsaber.SetActive(false);
            if (musicPlayer != null) musicPlayer.Stop();
            isFlying = false;
        }
    }

    void HandleLightsaber()
    {
        if (!lightsaber.activeSelf) return;

        if (currentState == HandState.Fist && !isFlying)
        {
            // Stick to hand
            lightsaber.transform.position = Vector3.Lerp(
                lightsaber.transform.position,
                handTransform.position,
                Time.deltaTime * returnSpeed
            );
            lightsaber.transform.rotation = handTransform.rotation;
        }
        else if (currentState == HandState.Point && isFlying)
        {
            // Fly forward spinning
            lightsaber.transform.position = Vector3.MoveTowards(
                lightsaber.transform.position,
                targetPosition,
                throwSpeed * Time.deltaTime
            );
            lightsaber.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

            if (Vector3.Distance(lightsaber.transform.position, targetPosition) < stopDistance)
            {
                isFlying = false; // wait until fist closes
            }
        }
        else if (currentState == HandState.Fist && !isFlying)
        {
            // Return to hand
            lightsaber.transform.position = Vector3.MoveTowards(
                lightsaber.transform.position,
                handTransform.position,
                returnSpeed * Time.deltaTime
            );

            if (Vector3.Distance(lightsaber.transform.position, handTransform.position) < stopDistance)
            {
                lightsaber.transform.position = handTransform.position;
                lightsaber.transform.rotation = handTransform.rotation;
            }
        }
    }
}
