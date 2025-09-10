using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AngerTest : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps1; 
    [SerializeField] private ParticleSystem ps2; 
    [SerializeField] private ParticleSystem ps3; 
    [SerializeField] private ParticleSystem ps4; 
    [SerializeField] private ParticleSystem ps5; 
    [SerializeField] private ParticleSystem ps6; 
    [SerializeField] private ParticleSystem ps7; 
    [SerializeField] private ParticleSystem ps8;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Play();
    }

    public void Play()
    {
        Debug.Log("Play");
        ps1.Stop();
        ps2.Stop();
        ps3.Stop();
        ps4.Stop();
        ps5.Stop();
        ps6.Stop();
        ps7.Stop();
        ps8.Stop();

        StopAllCoroutines();
        StartCoroutine(PlayCoroutine());
    }

    IEnumerator PlayCoroutine()
    {
        ps1.Play();
        yield return new WaitForSeconds(0.12f);
        ps2.Play();
        yield return new WaitForSeconds(0.12f);
        ps3.Play();
        yield return new WaitForSeconds(0.12f);
        ps4.Play();
        yield return new WaitForSeconds(0.12f);
        ps5.Play();
        yield return new WaitForSeconds(0.12f);
        ps6.Play();
        yield return new WaitForSeconds(0.12f);
        ps7.Play();
        yield return new WaitForSeconds(0.12f);
        ps8.Play();
    }
}
