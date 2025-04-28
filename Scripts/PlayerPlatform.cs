using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerPlatform : MonoBehaviour
{
    public float speed;
    public float jumpPower;

    public Transform groundVerif;
    public LayerMask layerGround;

    private bool isGround;
    private bool isRight = true;
    private bool canJump = false;
    private bool canFocus = false;
    private bool canVital = false;
    public bool isDialogueActive = true;
    private bool isPerformPowerUpAnim = false;
    private Rigidbody2D rig;
    private Coroutine focusCoroutine;

    private Animator animController;

    [Header("Audios")]
    public AudioSource attackSound;
    public AudioSource stepSound;
    public AudioSource drinkingSound;
    public AudioSource focusSound;

    [Header("Others Animations")]
    public GameObject concentrationAnim;

    [Header("Focus Settings")]
    public LayerMask focusLayer;
    public float focusDectectionRadius;
    public float focusDisableDelay;
    public Transform focusPoint;


    [Header("Push Settings")]
    public float pushRadius;
    public Transform pushCheckpoint;
    private bool isControlLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animController = GetComponent<Animator>();
        concentrationAnim = transform.Find("FocusAnim").gameObject;
        animController.SetBool("iswalk", false);
        animController.SetBool("jump", false);

        stepSound.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Moviment();
        Toggle();
        UpdateGroundStatus();
    }

    #region Moviment

    public void Moviment()
    {
        if (isPerformPowerUpAnim || isDialogueActive || InFocusAnimation() || isControlLocked)
        {
            animController.SetBool("iswalk", false);

            return;
        }

        float inputMove = Input.GetAxisRaw("Horizontal");

        rig.velocity = new Vector2(inputMove * speed, rig.velocity.y);

        bool isWalk = Mathf.Abs(inputMove) > 0.1f;

        animController.SetBool("iswalk", isWalk);

        if (isWalk && isGround)
        {
            if (!stepSound.isPlaying)
            {
                stepSound.Play();
            }
        }
        else
        {
            if (stepSound.isPlaying)
            {
                stepSound.Stop();
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isPerformPowerUpAnim || InFocusAnimation())
        {
            return;
        }

        isGround = Physics2D.OverlapCircle(groundVerif.position, 0.2f, layerGround);

        animController.SetBool("jump", !isGround);

        if (canJump && isGround && context.started)
        {
            rig.velocity = new Vector2(rig.velocity.x, jumpPower);

            if (stepSound.isPlaying)
            {
                stepSound.Stop();
            }
        }
    }

    void Toggle()
    {
        if (isControlLocked)
        {
            return;
        }

        float inputMove = Input.GetAxis("Horizontal");

        if (inputMove > 0 && !isRight)
        {
            Flip();
        }
        else if (inputMove < 0 && isRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isRight = !isRight;

        Vector3 lScale = transform.localScale;
        lScale.x *= -1;
        transform.localScale = lScale;
    }

    #endregion

    public void Focus(InputAction.CallbackContext context)
    {
        if (!canFocus)
        {
            return;
        }

        if (context.started)
        {
            animController.SetBool("focus", true);
            focusSound.Play();

            if (focusCoroutine != null)
            {
                StopCoroutine(focusCoroutine);

                if (concentrationAnim == null)
                {
                    Debug.LogError("concentrationAnim está NULL!");
                }
            }

            focusCoroutine = StartCoroutine(ActiveConcentrationAnim(0.5f));

            Collider2D[] hits = Physics2D.OverlapCircleAll(focusPoint.position, focusDectectionRadius);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("focus"))
                {
                    StartCoroutine(DisabelObjectFocus(hit.gameObject, focusDisableDelay));
                }
            }
        }

        if (context.canceled)
        {
            animController.SetBool("focus", false);
            focusSound.Stop();

            if (focusCoroutine != null)
            {
                StopCoroutine(focusCoroutine);
                focusCoroutine = null;
            }

            concentrationAnim.SetActive(false);
        }
    }

    public void Vital(InputAction.CallbackContext context)
    {
        if (isPerformPowerUpAnim || isDialogueActive)
        {
            return;
        }

        if (Gamepad.all.Count > 0)
        {
           
        }

        if (canVital && context.started)
        {
            isPerformPowerUpAnim = true;
            animController.SetTrigger("attack");
            attackSound.Play();
        }
    }

    public void EnableJump()
    {
        canJump = true;
    }

    public void EnableFocus()
    {
        canFocus = true;
    }

    public void EnableVital()
    {
        canVital = true;
    }

    public void PlayPowerUpAnimation(string parameter)
    {
        isPerformPowerUpAnim = true;

        stepSound.Stop();

        rig.velocity = Vector2.zero;

        animController.SetTrigger(parameter);

        if (drinkingSound != null)
        {
            drinkingSound.Play();
        }
    }

    private void PushObjects()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(pushCheckpoint.position, pushRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Push"))
            {
                PushObject pushObj = hit.GetComponent<PushObject>();

                if (pushObj != null)
                {
                    Vector2 pushDir = isRight ? Vector2.right : Vector2.left;
                    pushObj.Push(pushDir);
                }
            }
        }
    }

    public void OnAttackImpact()
    {
        PushObjects();
    }

    public void PowerUpAnimationEnd()
    {
        isPerformPowerUpAnim = false;
    }

    public void VitalAnimationEnd()
    {
        isPerformPowerUpAnim = false;
    }

    private bool InFocusAnimation()
    {
        return animController.GetCurrentAnimatorStateInfo(0).IsName("Use_Focus_Player1");
    }

    IEnumerator ActiveConcentrationAnim(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        concentrationAnim.SetActive(true);
        focusCoroutine = null;
    }

    public IEnumerator ZoomCameraEffect(Camera cam, PlayerPlatform player, float zoomAmount, float duration, float hold, float moveOffset, float moveDuration)
    {
        player.LockControl(true);

        float original = cam.orthographicSize;
        Vector3 originalPosition = cam.transform.position;

        float t = 0f;

        while (t < duration)
        {
            cam.orthographicSize = Mathf.Lerp(original, zoomAmount, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = zoomAmount;

        Vector3 targetPosition = originalPosition + new Vector3(moveOffset, 0f, 0f);
        t = 0f;
        while (t < moveDuration)
        {
            cam.transform.position = Vector3.Lerp(originalPosition, targetPosition, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cam.transform.position = targetPosition;

        yield return new WaitForSeconds(hold);

        t = 0f;
        while (t < moveDuration)
        {
            cam.transform.position = Vector3.Lerp(targetPosition, originalPosition, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cam.transform.position = originalPosition;

        t = 0f;
        while (t < duration)
        {
            cam.orthographicSize = Mathf.Lerp(zoomAmount, original, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = original;

        player.LockControl(false);
    }

    IEnumerator DisabelObjectFocus(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    public void LockControl(bool value)
    {
        isControlLocked = value;
    }

    void UpdateGroundStatus()
    {
        isGround = Physics2D.OverlapCircle(groundVerif.position, 0.2f, layerGround);
        animController.SetBool("jump", !isGround);
    }

    private void OnDrawGizmosSelected()
    {
        if (pushCheckpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pushCheckpoint.position, pushRadius);
        }

        if (focusPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(focusPoint.position, focusDectectionRadius);
        }
    }
}
