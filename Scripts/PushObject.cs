using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    private Rigidbody2D rig;

    public float pushDistance;
    public float pushSpeed;
    public LayerMask collisionLayer;

    private bool isMoving = false;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    public void Push(Vector2 direction)
    {
        if (isMoving)
        {
            return;
        }

        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = rig.Cast(direction, new ContactFilter2D
        {
            layerMask = collisionLayer,
            useLayerMask = true,
            useTriggers = false
        }, hits, pushDistance);

        if (hitCount > 0)
        {
            return;
        }

        Vector2 targetPos = rig.position + direction.normalized * pushDistance;
        StartCoroutine(MoveToPosition(targetPos));
    }

    IEnumerator MoveToPosition(Vector2 targetPos)
    {
        isMoving = true;
        float elapsed = 0f;
        Vector2 startPos = rig.position;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            rig.MovePosition(Vector2.Lerp(startPos, targetPos, elapsed));
            yield return new WaitForFixedUpdate();
        }

        rig.MovePosition(targetPos);
        isMoving = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;

        // Direção baseada no último push ou padrão (direita)
        Vector2 direction = Vector2.right;
        if (GameObject.FindWithTag("Player") != null)
        {
            var player = GameObject.FindWithTag("Player").transform;
            direction = player.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        Vector2 origin = rig.position;
        Vector2 end = origin + direction.normalized * pushDistance;


        Gizmos.DrawLine(origin, end);

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Vector2 size = box.size;
            Vector2 offset = box.offset;

            // Posição final do collider após o cast
            Vector2 boxPos = end + offset;

            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawCube(boxPos, size);
        }
    }

}
