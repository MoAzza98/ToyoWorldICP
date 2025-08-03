using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    [SerializeField] GameObject spawnEffect;

    public Toyo ToyoToSpawn { get; set; }
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

    Toyo targetToyoToCatch = null;
    ToyoballItem pokeballItem = null;
    bool thrownFromBattle = false;

    public void ThrowPokeballFromBattle(Toyo wildToyo, ToyoballItem pokeballItem)
    {
        CatchComplete = false;
        this.pokeballItem = pokeballItem;
        thrownFromBattle = true;
        targetToyoToCatch = wildToyo;

        ShakeCount = TryToCatchToyo(wildToyo, pokeballItem);
        ThrowPokeballFromFreeRoam(wildToyo.Model.transform.position + Vector3.up, pokeballItem);
    }

    public void ThrowPokeballFromFreeRoam(Vector3 targetPos, ToyoballItem pokeballItem)
    {
        CatchComplete = false;
        this.pokeballItem = pokeballItem;
        thrownFromBattle = false;

        LaunchToTarget(targetPos);
    }

    int TryToCatchToyo(Toyo wildToyo, ToyoballItem pokeballItem)
    {
        float a = (3 * wildToyo.MaxHp - 2 * wildToyo.Hp) * wildToyo.Base.CatchRate * pokeballItem.CatchRateModifier /** ConditionsDB.GetStatusBonus(enemyToyo.Status)*/ / (3 * wildToyo.MaxHp);

        if (a >= 255)
            return 4;

        float b = 1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / a));

        int shakeCount = 0;
        while (shakeCount < 4)
        {
            if (UnityEngine.Random.Range(0, 65535) >= b)
                break;

            ++shakeCount;
        }

        return shakeCount;
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
                BattleState.i.StartWildBattle(PlayerController.i.Party, ToyoToSpawn, wildToyo.Toyo);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator CatchToyo(Collider collider)
    {
        var wildToyo = collider.GetComponent<WildToyo>();
        if (targetToyoToCatch != null && targetToyoToCatch != wildToyo.Toyo)
            yield break;

        if (wildToyo != null)
        {
            if (!thrownFromBattle)
                ShakeCount = TryToCatchToyo(wildToyo.Toyo, pokeballItem);

            var dirToCam = (cam.position - wildToyo.transform.position).normalized;
            dirToCam.y = 0;

            var effect = Instantiate(spawnEffect, wildToyo.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            effect.transform.forward = dirToCam;

            yield return new WaitForSeconds(0.2f);

            wildToyo.gameObject.SetActive(false);

            // Make pokeball fall down
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f, GameLayers.i.GroundLayer))
            {
                yield return transform.DOMoveY((hit.point + Vector3.up * 0.2f).y, 0.5f).SetEase(Ease.OutBounce).WaitForCompletion();
            }

            for (int i = 0; i < Mathf.Min(ShakeCount, 3); ++i)
            {
                yield return new WaitForSeconds(0.5f);
                yield return transform.DOShakeRotation(0.5f, 20, 8, 70, true);
            }

            if (ShakeCount == 4)
            {
                // Pokemon is caught
                yield return DialogueState.i.ShowDialogue($"{wildToyo.Toyo.Base.Name} was caught");
                yield return DialogueState.i.ShowDialogue($"{wildToyo.Toyo.Base.Name} has been added to your party");

                wildToyo.enabled = false;
                PlayerController.i.Party.AddToyo(wildToyo.Toyo);
            }
            else
            {
                // Pokemon broke out
                yield return new WaitForSeconds(1f);

                if (ShakeCount < 2)
                    yield return DialogueState.i.ShowDialogue($"{wildToyo.Toyo.Base.Name} broke free");
                else
                    yield return DialogueState.i.ShowDialogue($"Almost caught it");

                if (!thrownFromBattle)
                    wildToyo.transform.forward = Vector3.Scale(new Vector3(1, 0, 1), (PlayerController.i.transform.position - wildToyo.transform.position));

                wildToyo.Toyo.Model.gameObject.SetActive(true);

                if (thrownFromBattle)
                    wildToyo.Toyo.ShowHUD();
            }

            targetToyoToCatch = null;
            CatchComplete = true;
        }

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
