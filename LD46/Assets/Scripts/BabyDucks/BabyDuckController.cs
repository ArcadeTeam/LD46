using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyDuckController : Duck
{
    public Transform destination;
    NavMeshAgent agent;
    int distractionProbability = 0;
    [SerializeField] Animator animator;

    [Header("Skins")]
    public Material[] skins;
    private SkinnedMeshRenderer skinnedMesh;

    private GameObject honk;
    private AudioSource audio;

    private bool lost = true;
    private bool waitingForPlayLostSound = false;

    void Start() {
        honk = transform.Find("Honk").gameObject;
        audio = GetComponent<AudioSource>();
        honk.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMesh.material = skins[Random.Range(0, skins.Length)];
    }

    void Update() {
        if (dead) return;

        animator.SetBool("Running", agent.velocity.magnitude > 0f);
        if (lost && !audio.isPlaying && !waitingForPlayLostSound) StartCoroutine(lostSound(0.5f+Random.value));

        if (destination != null)
            agent.destination = destination.position;
    }

    public void Distract(Transform newDestination) {
        distractionProbability += 10;
        if(distractionProbability > Random.Range(0, 100)) {
            destination = newDestination;
        }
    }

    private IEnumerator lostSound(float waitTime)
    {
        waitingForPlayLostSound = true;
        yield return new WaitForSeconds(waitTime);
        audio.PlayOneShot((AudioClip)Resources.Load("sounds/honk/BABY_DUCK_LOST_" + (1 + (int)(Random.value * 4))));
        waitingForPlayLostSound = false;
    }

    public void GoWithMom(Transform newDestination)
    {

        lost = false;
        StartCoroutine(startHonk(0.75f));
        distractionProbability = 0;
        destination = newDestination;
    }
    private IEnumerator startHonk(float waitTime)
    {
        yield return new WaitForSeconds(waitTime+Random.value*0.75f);
        honk.SetActive(true);
        audio.loop = true;
        audio.clip = (AudioClip) Resources.Load("sounds/honk/BABY_DUCK_" + (1 + (int) (Random.value * 4)));
        audio.Play();
        StartCoroutine(disableHonk(0.75f + Random.value * 1f));
    }

    private IEnumerator disableHonk(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audio.loop = false;
        honk.SetActive(false);
    }

    public void HumanDetected(Vector3 humanPosition)
    {
        Vector3 dir = transform.position - humanPosition;
        agent.destination = humanPosition + dir * 3;
        destination = null;
    }
}
