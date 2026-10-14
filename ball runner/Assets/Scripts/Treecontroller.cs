
using UnityEngine;
using System;

public class TreeController : MonoBehaviour
{
    private float moveSpeed;
    private float scoreX;
    private Action onScore;
    private bool scored;

    public void Initialize(
        float speed,
        float targetX,
        Action scoreCallback)
    {
        moveSpeed = speed;
        scoreX = targetX;
        onScore = scoreCallback;
    }

    void Update()
    {
        transform.position +=
            Vector3.left * moveSpeed * Time.deltaTime;

        if (!scored && transform.position.x <= scoreX)
        {
            scored = true;
            onScore?.Invoke();
        }

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
