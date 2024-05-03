using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
   public static soundManager Instance;

    public AudioSource mucevherSesi, patlamaSesi, oyunBittiSesi;

    private void Awake()
    {
        Instance = this;
    }

    public void MucevherSesiCikar()
    {
        mucevherSesi.Stop();
        mucevherSesi.pitch = Random.Range(0.8f, 1.2f);

        mucevherSesi.Play();
    }

    public void PatlamaSesiCikar()
    {
        patlamaSesi.Stop();
        patlamaSesi.pitch = Random.Range(0.8f, 1.2f);

        patlamaSesi.Play();
    }

    public void OyunBittiSesiCikar()
    {
        

        oyunBittiSesi.Play();
    }
}
