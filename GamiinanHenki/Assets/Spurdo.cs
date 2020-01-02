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
}
