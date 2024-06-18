using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    [SerializeField] GameObject spawnEffect;

    public Toyo ToyoToSpawn { get; set; }
    public ToyoParty ToyoParty { get; set; }

    Transform cam;
    Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    public void LaunchToTarget(Vector3 targetPos)
    {
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
            StartCoroutine(SpawnToyo(collision.collider));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rigidbody.isKinematic) return;

        if (other.gameObject.tag != "Player")
        {
            rigidbody.isKinematic = true;
            StartCoroutine(SpawnToyo(other));
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
}
