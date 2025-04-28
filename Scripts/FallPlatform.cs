using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    public float fallDelay;
    public float resetDelay;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody2D rig;
    private Collider2D col;
    private bool isFalling = false;
    private bool playerOnPlatform = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        rig.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = false;
    }

    private void Fall()
    {
        if (playerOnPlatform)
        {
            isFalling = true;
            rig.bodyType = RigidbodyType2D.Dynamic;
            col.isTrigger = true;

            Invoke(nameof(ResetPlatform), resetDelay);
        }
    }

    private void ResetPlatform()
    {
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0f;
        rig.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = false;

        transform.position = startPosition;
        transform.rotation = startRotation;

        isFalling = false;
        playerOnPlatform = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.collider.CompareTag("Player"))
        {
            if (collision.relativeVelocity.y <= 0f)
            {
                playerOnPlatform = true;
                Invoke(nameof(Fall), fallDelay);
            }
        }
    }

}
