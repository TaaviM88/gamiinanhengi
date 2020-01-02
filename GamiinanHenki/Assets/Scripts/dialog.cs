using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialog : MonoBehaviour
{
    public string[] hengiDialog;
    public string[] alogasDialog;
    bool hengiIsOn = false;
    int hengIndex = 0;
    int alogasIndex = 0;
    bool initialStart = true;

    private void Start()
    {

    }

    public void GoNextLineHengi()
    {
        if (initialStart)
        {
            UIManager.Instance.GaminanHenkiSanoo(hengiDialog[hengIndex]);
            UIManager.Instance.AlogasSanoo(alogasDialog[alogasIndex]);
            initialStart = false;

        }
        else
        {
            if(alogasDialog.Length > alogasIndex)
            {
                UIManager.Instance.AlogasSanoo(alogasDialog[alogasIndex]);
                alogasIndex++;
                
                if(alogasIndex ==4)
                {
                    GameManager.Instance.CanUseToivomusVesi();
                }
            }

            if(hengiDialog.Length > hengIndex)
            {
                UIManager.Instance.GaminanHenkiSanoo(hengiDialog[hengIndex]);
                hengIndex++;
                
            }
            else
            {
                
            }
            
        }

    }
}
