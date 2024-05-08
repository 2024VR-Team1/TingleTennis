using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class npsController : MonoBehaviour{

    public NavMeshAgent agent;
    public Animator animator;
    

    // TODO: intro path
    public GameObject INTROPATH;
    public Transform[] PathPoints;
    public float minDistance = 0.3f;
    public int roamIndex = 0;

    //TODO: right walk
    public GameObject rightWalkPath;
    public Transform[] RightPathPoints;
    public int walkIndex = 0;
    private float startTime;
    public float rotateAngle = 190f;

    // panel
    public GameObject panelObject;
    public GameObject teachingPanel;
    public Button closeButton;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PathPoints = new Transform[INTROPATH.transform.childCount];
        for(int i = 0; i < PathPoints.Length; i++){
            PathPoints[i] = INTROPATH.transform.GetChild(i);
        }

        RightPathPoints = new Transform[rightWalkPath.transform.childCount];
        for(int i = 0; i < RightPathPoints.Length; i++){
            RightPathPoints[i] = rightWalkPath.transform.GetChild(i);
        }

        panelObject.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    float t = 1f;
    bool hasReachedUser = false;
    bool introEnd = false;
    bool goToRacket = false;
    bool startToRotate = false;

    // talking
    bool startTalking = false;

    void Update(){
        // Debug.Log("introend is : " + introEnd);
        // Coach approaching the user
        if(!hasReachedUser){
            Roam();
        }

        if(hasReachedUser  && !introEnd){
            panelObject.SetActive(true);
            Idle();
        }

        if(startTalking && !goToRacket){
            CoachTalking();
        }
        // Start to walk to the racket
        if(goToRacket){
            // Debug.Log("horizontal value: " + animator.GetFloat("horizontal"));
            // Debug.Log("vertical value: " + animator.GetFloat("vertical"));
            // Debug.Log("introend is : " + introEnd);
            WalkToRacket();
        }

        if(startToRotate){
            // Debug.Log("gogogo");
            float t = (Time.time - startTime) / 2f;
            transform.rotation = Quaternion.Euler(0f, rotateAngle, 0f);
            // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 130f, 0f), t);
            startToRotate = false;
            Invoke("WaveHands", 1f);
        }

        // if(talkingBoard){
        //     CoachTalking();
        // }
    }

    void Roam(){
        if(roamIndex < 3){
            if(Vector3.Distance(transform.position, PathPoints[roamIndex].position) < minDistance){
                // Debug.Log("distance: " + Vector3.Distance(transform.position, PathPoints[roamIndex].position));
                if(roamIndex >= 0 && roamIndex < PathPoints.Length - 1){
                    roamIndex += 1;
                }
            }
            agent.SetDestination(PathPoints[roamIndex].position);
            animator.SetFloat("vertical", 1 ); //  agent.isStopped ? 1 : 0
        }
        else{
            agent.SetDestination(PathPoints[3].position);
            t = Vector3.Distance(transform.position, PathPoints[3].position) < 0.47f ? 0f : 1f;
            animator.SetFloat("vertical", Mathf.Lerp(1f, 0f, 1f - t));
            if (t == 0f) { // reached the user
                hasReachedUser = true;
            }
        }
    }

    void Idle(){
        animator.SetBool("reached", true);
        // Debug.Log("here");

        // if the button is pressed, go to right walk
        // StartCoroutine(DelayedIntroEnd());
        
        //TODO: talking board
        StartCoroutine(DelayedIntroEnd());
    }

    IEnumerator DelayedIntroEnd() {
        yield return new WaitForSeconds(0.5f);
        // animator.SetBool("introEnd", true);
        introEnd = true;
        // goToRacket = true;
        animator.SetBool("talkingBoard", true);
        startTalking = true;
        // panelObject.SetActive(false);
    }

    void WalkToRacket(){
        if(walkIndex < 2){
            if(Vector3.Distance(transform.position, RightPathPoints[walkIndex].position) < minDistance){
                if(walkIndex >= 0 && walkIndex < RightPathPoints.Length - 1){
                    walkIndex += 1;
                }
            }
            agent.SetDestination(RightPathPoints[walkIndex].position);
            animator.SetFloat("horizontal", -1 );
            // Debug.Log("horizontal value: " + animator.GetFloat("horizontal"));
        }
        else{
            agent.SetDestination(RightPathPoints[2].position);
            // Debug.Log("distance: " + Vector3.Distance(transform.position, RightPathPoints[2].position));
            if(Vector3.Distance(transform.position, RightPathPoints[2].position) < 0.47f){
                Debug.Log("here");
                t = 0;
                animator.SetFloat("horizontal", Mathf.Lerp(1f, 0f, 1f - t));
                // transform.rotation = Quaternion.Euler(0f, 130f, 0f);
                startTime = Time.time;
                startToRotate = true;
                goToRacket = false;
            }
        }
    }

    void CoachTalking(){
        animator.SetBool("talkingBoard", true);
    }

    void ClosePanel()
    {
        teachingPanel.SetActive(false);
        animator.SetBool("buttonClicked", true);
        goToRacket = true;
    }

    void WaveHands(){
        animator.SetBool("wavingMode", true);
    }
}
