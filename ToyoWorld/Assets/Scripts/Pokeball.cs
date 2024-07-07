using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    [SerializeField] GameObject spawnEffect;

    public Toyo ToyoToSpawn { get; set; }
    public ToyoParty ToyoParty { get; set; }
    public int ShakeCount { get; set; }

    // Output
    public bool CatchComplete { get; private set; }

    Transform cam;
    Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    public void LaunchToTarget(Vector3 targetPos)
    {
        CatchComplete = false;

        transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.velocity = CalculateLaunchVelocity(targetPos);
    }

    Vector3 CalculateLaunchVelocity(Vector3 targetPos)
    {
        var startPos = transform.position;

        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        float h = displacementY + 0.1f * displacementXZ.magnitude;
        h = Mathf.Clamp(h, 0f, displacementY + 2f);

        float g = Physics.gravity.y;

        var veclocityY = Vector3.up * Mathf.Sqrt(-2 * g * h);
        var velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / g) + Mathf.Sqrt(2 * (displacementY - h) / g));

        return veclocityY + velocityXZ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.isKinematic) return;

        if (collision.gameObject.tag != "Player")
        {
            rigidbody.isKinematic = true;

            if (ToyoToSpawn != null)
                StartCoroutine(SpawnToyo(collision.collider));
            else
                StartCoroutine(CatchToyo(collision.collider));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rigidbody.isKinematic) return;

        if (other.gameObject.tag != "Player")
        {
            rigidbody.isKinematic = true;

            if (ToyoToSpawn != null)
                StartCoroutine(SpawnToyo(other));
            else
                StartCoroutine(CatchToyo(other));
        }
    }

    IEnumerator SpawnToyo(Collider collider)
    {
        Vector3 spawnPos = transform.position;
        Vector3 dirToPlayerPokemon = Vector3.zero;

        var wildToyo = collider.GetComponent<WildToyo>();
        if (wildToyo != null)
        {
            wildToyo.SetBusyState();

            var dirToCam = (cam.position - wildToyo.transform.position).normalized;
            dirToCam.y = 0;

            dirToPlayerPokemon = Quaternion.Euler(0, 30, 0) * dirToCam;

            spawnPos = wildToyo.transform.position + dirToPlayerPokemon * 6f;
            wildToyo.transform.forward = dirToPlayerPokemon;
        }

        var rayOrigin = spawnPos + Vector3.up;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10f, GameLayers.i.GroundLayer))
        {
            var dirToCam = (cam.position - hit.point).normalized;
            dirToCam.y = 0;

            var effect = Instantiate(spawnEffect, hit.point + Vector3.up * 0.5f, Quaternion.identity);
            effect.transform.forward = dirToCam;

            yield return new WaitForSeconds(0.2f);

            ToyoParty.SpawnModel(ToyoToSpawn, hit.point);

            ToyoToSpawn.Animator.enabled = false;
            yield return null;

            ToyoToSpawn.Model.transform.position = hit.point;
            ToyoToSpawn.Animator.enabled = true;
            ToyoToSpawn.Model.transform.forward = (wildToyo != null)? -dirToPlayerPokemon : dirToCam;

            if (wildToyo != null)
            {
                BattleState.i.StartState(ToyoParty, ToyoToSpawn, wildToyo.Toyo);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator CatchToyo(Collider collider)
    {
        var toyo = collider.GetComponent<WildToyo>();
        if (toyo != null)
        {
            var dirToCam = (cam.position - toyo.transform.position).normalized;
            dirToCam.y = 0;

            var effect = Instantiate(spawnEffect, toyo.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            effect.transform.forward = dirToCam;

            yield return new WaitForSeconds(0.2f);

            toyo.gameObject.SetActive(false);

            //for (int i = 0; i < Mathf.Min(ShakeCount, 3); ++i)
            //{
            //    yield return new WaitForSeconds(0.5f);
            //    yield return transform.DOPunchRotation(new Vector3(0, 0, 10f), 0.8f).WaitForCompletion();
            //}

            if (ShakeCount == 4)
            {
                // Pokemon is caught
                yield return DialogueState.i.ShowDialogue($"{toyo.Toyo.Base.Name} was caught");
                yield return DialogueState.i.ShowDialogue($"{toyo.Toyo.Base.Name} has been added to your party");
            }
            else
            {
                // Pokemon broke out
                yield return new WaitForSeconds(1f);

                if (ShakeCount < 2)
                    yield return DialogueState.i.ShowDialogue($"{toyo.Toyo.Base.Name} broke free");
                else
                    yield return DialogueState.i.ShowDialogue($"Almost caught it");

                toyo.Toyo.Model.gameObject.SetActive(true);
            }

            CatchComplete = true;
        }
    }
}
