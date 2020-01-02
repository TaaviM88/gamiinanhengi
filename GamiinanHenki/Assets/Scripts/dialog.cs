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

    public void GoNextLineHengi()
    {
       
            if(alogasDialog.Length > alogasIndex)
            {
                UIManager.Instance.AlogasSanoo(alogasDialog[alogasIndex]);
                alogasIndex++;
                
                if(alogasIndex ==3)
                {
                    GameManager.Instance.ShowToivomusVesi();
                }
            }

            if(hengiDialog.Length > hengIndex)
            {
                UIManager.Instance.GaminanHenkiSanoo(hengiDialog[hengIndex]);
                hengIndex++;
                
            }
           

    }
}
