using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rigidbody;
    public CapsuleCollider capsuleCollider;

    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    private void Awake() {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        SetCollidersEnabled(false);
        SetRigidbodiesKinematic(true);
    }

    private void SetCollidersEnabled(bool enabled) {
        foreach (Collider col in colliders)
            col.enabled = enabled;
        colliders[0].enabled = true;
    }

    private void SetRigidbodiesKinematic(bool kinematic) {
        foreach (Rigidbody rb in rigidbodies)
            rb.isKinematic = kinematic;
        rigidbodies[0].isKinematic = false;
    }

    public void ActivateRagdoll() {
        capsuleCollider.enabled = false;
        rigidbody.isKinematic = true;
        animator.enabled = false;

        SetCollidersEnabled(true);
        SetRigidbodiesKinematic(false);
    }

}
