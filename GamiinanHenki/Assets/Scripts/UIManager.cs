using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text alogasText, gaminanHenkiText, woodInCamina, temperature, time, virkeys;
    public Image thermometerBlock;
    public Button ESButton, dialogButton;
    bool initialDialog = false;
    Color orginalColor;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        orginalColor = thermometerBlock.color;
        dialogButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameIsOn)
        {
            woodInCamina.text = $"{GameManager.Instance.GetKaminaCurrentCapacity()}";
            temperature.text = $"Temperature: {Mathf.RoundToInt(GameManager.Instance.GetTemperature())}°C";
            virkeys.text = $"virkeys: {Mathf.RoundToInt(GameManager.Instance.GetVirkeys())}";

            if (GameManager.Instance.GetESCooldown() == true)
            {
                ESButton.gameObject.SetActive(false);
            }
            else
            {
                ESButton.gameObject.SetActive(true);
            }

        }

    }

    public void UpdateThermoMeterBlock(float f)
    {
        if (GameManager.Instance.gameIsOn)
        {
            //thermometerBlock.rectTransform.sizeDelta += new Vector2(0,i);
            if (thermometerBlock.rectTransform.localScale.y + new Vector3(0, f, 0).y < 2.01f)
            {
                thermometerBlock.rectTransform.localScale += new Vector3(0, f, 0);
            }


            if (GameManager.Instance.GetTemperature() >= 0 && thermometerBlock.color != orginalColor)
            {
                thermometerBlock.color = Color.Lerp(thermometerBlock.color, orginalColor, 1);
            }

            if (GameManager.Instance.GetTemperature() <= 0)
            {
                thermometerBlock.color = Color.Lerp(thermometerBlock.color, Color.blue, 1);
            }
        }
            
    }

    public void  AlogasSanoo(string t)
    {
        if (GameManager.Instance.gameIsOn)
            alogasText.text = t;
    }

    public void GaminanHenkiSanoo(string t)
    {
        if (GameManager.Instance.gameIsOn)
            gaminanHenkiText.text = t;
    }

    #region Button Methods
    public void AddWoodToKamina()
    {
        if (GameManager.Instance.gameIsOn)
        {
            if (GameManager.Instance.GetVirkeys() > 0)
            {
                GameManager.Instance.AddWoodToKamina(1);
            }
        }
           
       
    }

    public void RemoveWoodOnKamina()
    {
        if (GameManager.Instance.gameIsOn)
        {
            if (GameManager.Instance.GetVirkeys() > 0)
            {
                GameManager.Instance.RemoveWoodFromKamina(1);
            }
        }
            
           
    }

    public void DrinkES()
    {
        if (GameManager.Instance.GetVirkeys() > 0 && GameManager.Instance.gameIsOn)
        {
            GameManager.Instance.PlayDrinkAnimation();
            GameManager.Instance.StartESCooldown();
            GameManager.Instance.ChangeVirkeys(10);
            //AudioManager.Instance.PlaySFX(0);
        }
            
    }

    public void ToivomusWaterToKamina()
    {
      if(GameManager.Instance.GetVirkeys() < 30 && GameManager.Instance.GetTemperature() > 60 && GameManager.Instance.gameIsOn)
        {

            //Debug.Log("LISÄDÄÄN DOIVOMUSVEDDÄ");
            if(GameManager.Instance.getCanUseToivomusVesi())
            {
                GameManager.Instance.UseToivomusVetta();
            }
        }
        else
        {
            AlogasSanoo("En ole vielä niin väsynyd, eddä dädä lisäidin :D! Koko deldda räjähdää! EBIN :D");
        }
    }

    public void SetActiveDialogButton()
    {
        dialogButton.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        //Application.Quit();
        GameManager.Instance.GoMainMenu();
    }
    #endregion
}