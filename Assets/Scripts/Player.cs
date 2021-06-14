using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float paddingLeft = 0f;
    [SerializeField] private float paddingRight = 0f;
    [SerializeField] private float paddingTop = 0f;
    [SerializeField] private float paddingBottom = 0f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] private GameObject lazerPrefab;
    

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private SpriteRenderer spriteRenderer;
    private Coroutine firingCoroutine;

    // Start is called before the first frame update
    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetUpMoveBoundaries();
        
    }

    // Update is called once per frame
    private void Update() {
        Move();
        Fire();
    }   

    private void Fire() {
        if (Input.GetButtonDown("Fire1")) {
            firingCoroutine = StartCoroutine(FireContinuously());            
        }
        if (Input.GetButtonUp("Fire1")) {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously() {
        while (true) {
            GameObject lazer = Instantiate(lazerPrefab, transform.position, Quaternion.identity) as GameObject;
            lazer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
       
    }

    private void SetUpMoveBoundaries() {
        Camera gameCamera = Camera.main;
        var cameraBottomLeftPoint = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        var cameraTopRightPoint = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        var shipPaddingX = spriteRenderer.bounds.size.x / 2;
        var shipPaddingY = spriteRenderer.bounds.size.y / 2;
        xMin = cameraBottomLeftPoint.x + shipPaddingX + paddingLeft;
        xMax = cameraTopRightPoint.x - shipPaddingX - paddingRight;
        yMin = cameraBottomLeftPoint.y + shipPaddingY + paddingBottom;
        yMax = cameraTopRightPoint.y - shipPaddingY - paddingTop;

    }    

    private void Move() {
        var delta = Time.deltaTime * moveSpeed;
        var deltaX = Input.GetAxis("Horizontal") * delta;
        var deltaY = Input.GetAxis("Vertical") * delta;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
}
