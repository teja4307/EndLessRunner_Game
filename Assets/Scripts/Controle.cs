using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Controle : MonoBehaviour
{
    public GameObject Player;
    public float[] xPositions = { -1f, 0f, 1f };
    public GameObject character;
    public Animator characterAnimator;
    public bool isGameOver = false;
    public bool isFallDown = false;

    private float jumpHeight = 1.5f;
    private bool isJumping = false;
    private bool isRolling = false;
    private Coroutine moveCoroutine;
    private float movementSpeed = 5f;
    private float targetX;
    private int currentPosition = 1;
    private Vector3 firstTouchPos;
    private Vector3 lastTouchPos;
    private float dragDistance = 0.05f;
    private string[] rollStyle = { "Roll", "SpringRoll" };
    //int[] temp_roll = { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
    int[] temp_roll = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    // Start is called before the first frame update
    void Awake()
    {
        targetX = xPositions[currentPosition];
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouchInput();
    }
    private void HandleTouchInput()
    {
        if (isGameOver)
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            ProcessTouch(touch);
        }
    }
    private void ProcessTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                firstTouchPos = touch.position;
                lastTouchPos = touch.position;
                break;
            case TouchPhase.Moved:
                lastTouchPos = touch.position;
                break;
            case TouchPhase.Ended:
                lastTouchPos = touch.position;
                EvaluateSwipe();
                break;

        }

    }
    private void EvaluateSwipe()
    {
        float horizontalDistance = Mathf.Abs(lastTouchPos.x - firstTouchPos.x);
        float verticalDistance = Mathf.Abs(lastTouchPos.y - firstTouchPos.y);
        
        if(horizontalDistance> dragDistance || verticalDistance > dragDistance)
        {
            if (horizontalDistance > verticalDistance)
            {
                if (lastTouchPos.x > firstTouchPos.x)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            }
            else
            {
                if (lastTouchPos.y > firstTouchPos.y)
                {
                    Jump();
                }
                else
                {
                    RollDown();
                }
            }
        }

    }
    private void MoveLeft()
    {
        currentPosition--;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];
        StartMoveCoroutine();
    }
    private void MoveRight()
    {
        currentPosition++;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];
        StartMoveCoroutine();
    }
    private void Jump()
    {
        if (isJumping || isRolling)
        {
            return;
        }

        isJumping = true;
        characterAnimator.SetBool("Jump", true);
        StartCoroutine(JumpRoutine());
    }
    private IEnumerator JumpRoutine()
    {
        Transform child = character.transform.GetChild(0);
        float jumpDuration = 0.5f;
        float initialY = child.localPosition.y;
        float targetY = initialY + jumpHeight;

        // Jump up
        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(initialY, targetY, elapsed / jumpDuration);
            child.localPosition = new Vector3(child.localPosition.x, newY, child.localPosition.z);
            yield return null;
        }

        // Fall down
        elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(targetY, initialY, elapsed / jumpDuration);
            child.localPosition = new Vector3(child.localPosition.x, newY, child.localPosition.z);
            yield return null;
        }

        isJumping = false;
        if (!isGameOver)
            characterAnimator.SetBool("Jump", false);
        yield return null;
    }
    private void RollDown()
    {
        if (!isRolling)
        {
            isRolling = true;
            int temp2 = UnityEngine.Random.Range(0, temp_roll.Length);
            string rollType = rollStyle[temp_roll[temp2]];
            characterAnimator.SetBool(rollType, true);
            //playerTrigger.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ResetRoll(rollType));
        }
    }
    private IEnumerator ResetRoll(string _roll)
    {
        Vector3 startPosition = new Vector3(0, 0, 0);
        Vector3 endPosition = new Vector3(0, 0.65f, 0);
        float downDuration = 0.35f; // Duration for going down
                                    //  float upDuration = 0.2f; // Duration for going up
        float elapsed = 0f;

        // Move playerTrigger down to end position
        while (elapsed < downDuration)
        {
            elapsed += Time.deltaTime;
            // playerTrigger.transform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsed / downDuration);
            //   Debug.Log("Going down");
            yield return null;
        }

        // Ensure it reaches the exact start position
        //playerTrigger.transform.localPosition = startPosition;

        // Wait for 2 seconds before starting to move up
        yield return new WaitForSeconds(1f);


        isRolling = false;
        // Ensure it reaches the exact end position
        // playerTrigger.transform.localPosition = endPosition;

        // isRolling = false;
        characterAnimator.SetBool(_roll, false);
        
    }
    private void StartMoveCoroutine()
    {
        Vector3 targetPosition = new Vector3(targetX, character.transform.localPosition.y, character.transform.localPosition.z);

        // If there is already a running coroutine, stop it before starting a new one
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Start the new coroutine and store its reference
        moveCoroutine = StartCoroutine(MoveCharacterRoutine(targetPosition));
    }
    private IEnumerator MoveCharacterRoutine(Vector3 targetPosition)
    {
        // Keep moving the character until it reaches the target position
        while (Vector3.Distance(character.transform.localPosition, targetPosition) > 0.01f)
        {
            character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPosition, movementSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}
