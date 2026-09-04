using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadsideSequence : MonoBehaviour
{
    [Header("Sequence")]
    [SerializeField]
    private float delayBetweenWeapons = 0.08f;

    private Coroutine currentSequence;

    public bool IsFiring => currentSequence != null;

    public void FireSequence(
        List<Weapon> weapons,
        Action onSequenceCompleted = null)
    {
        if (IsFiring)
            return;

        if (weapons == null || weapons.Count == 0)
            return;

        currentSequence = StartCoroutine(
            FireWeaponsSequentially(
                weapons,
                onSequenceCompleted
            )
        );
    }

    private IEnumerator FireWeaponsSequentially(
        List<Weapon> weapons,
        Action onSequenceCompleted)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon != null && weapon.CanFire())
            {
                weapon.Fire();
            }

            yield return new WaitForSeconds(
                delayBetweenWeapons
            );
        }

        currentSequence = null;

        onSequenceCompleted?.Invoke();
    }

    public void CancelSequence()
    {
        if (currentSequence == null)
            return;

        StopCoroutine(currentSequence);

        currentSequence = null;
    }
}