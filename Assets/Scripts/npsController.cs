using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npsController : MonoBehaviour{

    public NavMeshAgent agent;
    public Animator animator;

    public GameObject PATH;
    public Transform[] PathPoints;

    public float minDistance = 0.3f;

    public int index = 0;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PathPoints = new Transform[PATH.transform.childCount];
        for(int i = 0; i < PathPoints.Length; i++){
            PathPoints[i] = PATH.transform.GetChild(i);
        }
    }

    float t = 1f;
    bool hasReachedUser = false;
    bool introEnd = false;

    void Update(){
        // Coach approaching the user
        if(!hasReachedUser){
            Roam();
        }

        if(hasReachedUser  && !introEnd){
            Idle();
        }
        // Start to walk to the racket
        if(introEnd){
            WalkToRacket();
        }
    }

    void Roam(){
        if(index < 3){
            if(Vector3.Distance(transform.position, PathPoints[index].position) < minDistance){
                // Debug.Log("distance: " + Vector3.Distance(transform.position, PathPoints[index].position));
                if(index >= 0 && index < PathPoints.Length - 1){
                    index += 1;
                }
            }
            agent.SetDestination(PathPoints[index].position);
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
        StartCoroutine(DelayedIntroEnd());
    }

    IEnumerator DelayedIntroEnd() {
        yield return new WaitForSeconds(3f);
        // animator.SetBool("introEnd", true);
        introEnd = true;
    }

    void WalkToRacket(){
        animator.SetBool("introEnd", true);
    }
}
