using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CubeRunnerController : MonoBehaviour
{
    [Header("Tree")]
    public GameObject treePrefab;
    public float spawnInterval = 2f;
    public float spawnX = 11f;
    public float minY = -0.6f;
    public float maxY = 0.3f;
    public float moveSpeed = 5f;
    public float scoreX = -6.78f;

    [Header("Player")]
    public float jumpForce = 7f;

    [Header("UI")]
    public TMP_Text scoreText;
    public GameObject gameOverTitle;
    public Button retryButton;

    private Rigidbody2D rb;
    private float spawnTimer;
    private int score;
    private bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Time.timeScale = 3f;

        score = 0;
        isDead = false;
        spawnTimer = 0f;

        gameOverTitle.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateScoreUI();
        retryButton.onClick.AddListener(() =>
        {
            Retry();
        });

        Debug.Log("GAME START");
    }

    void Update()
    {
        if (isDead) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnTree();
            spawnTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (rb == null) return;

        rb.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );

        Debug.Log("PLAYER JUMP");
    }

    void SpawnTree()
    {
        float y = Random.Range(minY, maxY);

        GameObject tree = Instantiate(
            treePrefab,
            new Vector3(spawnX, y, 0f),
            Quaternion.identity
        );

        TreeController controller =
            tree.GetComponent<TreeController>();

        if (controller != null)
        {
            controller.Initialize(
                moveSpeed,
                scoreX,
                AddScore
            );
        }

        Debug.Log($"SPAWN TREE | Y: {y:F2}");
    }

    void AddScore()
    {
        if (isDead) return;

        score++;
        UpdateScoreUI();

        Debug.Log($"SCORE: {score}");
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("tree"))
            Die();
    }

    void Die()
    {
        isDead = true;
        Time.timeScale = 0f;

        gameOverTitle.SetActive(true);
        retryButton.gameObject.SetActive(true);

        Debug.Log($"GAME OVER | FINAL SCORE: {score}");
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
