using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public  class PlayerController : MonoBehaviour
{
    
    public GameObject Player;
    public float[] xPositions = { -1f, 0f, 1f};
    public GameObject character;
    public Animator characterAnimator;


    //public GameObject[] allCharactors;
    public List<GameObject> allCharactors = new List<GameObject>();


    public bool isGameOver = false;
    public bool isFallDown = false;
    


    private float jumpHeight = 1.5f;
    private bool isJumping = false;
    private bool isRolling = false;
    private float movementSpeed = 7f;
    private GameObject playerTrigger;
    private Coroutine moveCoroutine;
    private float targetX;
    private int currentPosition = 1;
    private Vector3 firstTouchPos;
    private Vector3 lastTouchPos;
    private float dragDistance = 0.02f;
   /*private string[] rollStyle = { "Roll", "SpringRoll" };

  
    int[] temp_roll = { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };*/
    //int[] temp_roll = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private void Awake()
    {
       //SelectCharacter(5);

        targetX = xPositions[currentPosition];
        //charAnim = characterAnimator;
        //UpdateMovementSpeed();
        //UpdateJumpHeight();
       // UpdateSwipeDistance(); // Initialize swipe distance
    }
    private void Start()
    {
        //allCharactors[0].SetActive(false);
        //allCharactors[1].SetActive(true);
       // allCharactors.RemoveAt(0);
        
      // Transform Scale=character.transform.GetChild(0);
    }

   /* void SelectCharacter(int index)
    {
        for (int i = 0; i < allCharactors.Length; i++)
        {
            if (i == index)
            {
                allCharactors[i].SetActive(true);
                characterAnimator = allCharactors[i].GetComponent<Animator>();
            }
            else
            {
                allCharactors[i].SetActive(false);
            }
        }
    }*/
    public void Update()
    {
        HandleTouchInput();
        PlayerMoment();
    }
    private void PlayerMoment()
    {
        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,
                                                Player.transform.position.z + GameManager._inst.PlayerSpeed * Time.deltaTime);
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
                //lastTouchPos = touch.position;
                break;

            /*case TouchPhase.Moved:
                lastTouchPos = touch.position;
                //EvaluateSwipe();
                break;*/

            case TouchPhase.Ended:
                lastTouchPos = touch.position;
                EvaluateSwipe();
                break;
        }
    }

    private void EvaluateSwipe()
    {

        if (!GameManager._inst.isGameStart)
            return;
        float horizontalDistance = Mathf.Abs(lastTouchPos.x - firstTouchPos.x);
        float verticalDistance = Mathf.Abs(lastTouchPos.y - firstTouchPos.y);

        if (horizontalDistance > dragDistance || verticalDistance > dragDistance)
        {
            if (horizontalDistance > verticalDistance)
            {
                if (lastTouchPos.x > firstTouchPos.x)
                {
                    // print("Right");
                    
                    MoveRight();
                }
                else
                {
                    //print("Left");
                   
                    MoveLeft();
                }
            }
            else
            {
                if (lastTouchPos.y > firstTouchPos.y)
                {
                    //print("Up");

                   
                    Jump();
                }
                else
                {
                    //print("Down");
                   
                    RollDown();
                }
            }
        }
    }
    private void MoveLeft()
    {
       // Debug.Log("Move right");
        currentPosition--;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];

        StartMoveCoroutine();
    }

    private void MoveRight()
    {
       // Debug.Log("Move right");
        currentPosition++;
        currentPosition = Mathf.Clamp(currentPosition, 0, xPositions.Length - 1);
        targetX = xPositions[currentPosition];

        StartMoveCoroutine();
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
            character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPosition,
                                                                    movementSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
    private void Jump()
    {
        // Prevent jumping while rolling
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
        {
            characterAnimator.SetBool("Jump", false);
        }
    }
    private void RollDown()
    {
        if (!isRolling)
        {
            isRolling = true;
            /*int temp2 = UnityEngine.Random.Range(0, temp_roll.Length);
            string rollType = rollStyle[temp_roll[temp2]];*/
           // characterAnimator.SetBool(rollType, true);
            characterAnimator.SetBool("Roll", true);
            // playerTrigger.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ResetRoll());
            
        }
    }
    private IEnumerator ResetRoll()//string _roll
    {
        BoxCollider Scale = character.transform.GetChild(0).GetComponent<BoxCollider>();
        float y_val = Scale.size.y;
        float Finval = y_val/2;
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
            Scale.size = new Vector3(Scale.size.x, Finval, Scale.size.z);
            yield return null;
        }

        // Ensure it reaches the exact start position
       // playerTrigger.transform.localPosition = startPosition;

        // Wait for 2 seconds before starting to move up
        yield return new WaitForSeconds(1f);


        isRolling = false;
        Scale.size = new Vector3(Scale.size.x, y_val, Scale.size.z);
        // Ensure it reaches the exact end position
        // playerTrigger.transform.localPosition = endPosition;

        // isRolling = false;
        //characterAnimator.SetBool(_roll, false);
        characterAnimator.SetBool("Roll", false);
    }
    public void CharAnim()
    {
        if (GameManager._inst.PlayerSpeed == 0)
        {
            characterAnimator.SetTrigger("Dead");

        }
        else
        {
            characterAnimator.SetTrigger("Dead");

        }
    }

}