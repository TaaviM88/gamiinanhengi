using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spurdo : MonoBehaviour
{
    public void PlayDrinkSFX()
    {
        AudioManager.Instance.PlaySFX(0);
    }

    public void PlayWoodInKaminaOpenSFX()
    {
        AudioManager.Instance.PlaySFX(1);
    }

    public void MakeKaadaButtonVisible()
    {
        UIManager.Instance.KaadaActive();
    }

    public void PlayPouringSFX()
    {
        AudioManager.Instance.PlaySFX(2);
        
    }

    public void SetTentOnFire()
    {
        GameManager.Instance.SetTentFire();
    }
}
