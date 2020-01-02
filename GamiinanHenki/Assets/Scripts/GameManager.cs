using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("LightSources")]
    public lightController lamppu, dissivalo, kaminanValo, henkiValo;
    public GameObject dissit, gaminanHenki, firesound, toivomusvesi, headObject, spurdo;
    public Sprite benisHead;
    Sprite originalheadSprite;
    public ParticleSystem zzParticle, tentOnFire,snowPS, spurdoParticles;
    [SerializeField] float temperature = 20;
    [SerializeField] float timer;
    [SerializeField] int woodInKamina = 10;
    [SerializeField] int maxKaminaCapacity = 10;
    [SerializeField] int currentKaminaCapacity;
    //100%
    [SerializeField] float virkeysasteAlogalla = 0;
    public float maxVirkeysasteAlogalla = 100;
    private bool isBurningWood = true, isLoweringTemperature = false;


    public bool gameIsOn = true;
    bool endTimerOn = false, esCooldownOn = false, canUseToivomusVesi = false, goodEnd = false, toivomusVesiEnd = false, FreezeEnd = false, burnEnd = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        //Laitetaan gamina ja dissit piiloon alkuun
        if (dissit.activeInHierarchy)
        {
            dissit.SetActive(false);
        }

        if (gaminanHenki.activeInHierarchy)
        {
            gaminanHenki.SetActive(false);
        }

        if (zzParticle.gameObject.activeInHierarchy)
        {
            zzParticle.gameObject.SetActive(false);
        }

        if (toivomusvesi.activeInHierarchy)
        {
            toivomusvesi.SetActive(false);
        }

        if(tentOnFire.gameObject.activeInHierarchy)
        {
            tentOnFire.gameObject.SetActive(false);
        }

        if(snowPS.gameObject.activeInHierarchy)
        {
            snowPS.gameObject.SetActive(false);
        }

        if(spurdoParticles.gameObject.activeInHierarchy)
        {
            spurdoParticles.gameObject.SetActive(false);
        }
        currentKaminaCapacity = woodInKamina;
        virkeysasteAlogalla = maxVirkeysasteAlogalla;
        originalheadSprite = headObject.GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(BurnWood());
        StartCoroutine(LoseVirkeys());
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(0);
    }
    // Update is called once per frame
    void Update()
    {
        /*  if(woodInKamina <= 0)
          {
              temperature--;
          }
          */
        if (gameIsOn)
        {
            CheckVirkeys();
            CheckTemperature();
            CheckKaminaCapacity();
        }
        else
        {
            if (!endTimerOn)
            {
                StartCoroutine(GameOver());
            }
            //GoEndScene();
        }
    }

    IEnumerator BurnWood()
    {
        while (woodInKamina > 0 && isBurningWood)
        {
            yield return new WaitForSeconds(2);
            woodInKamina--;
            ChangeTemperature(1);
            isBurningWood = false;
            kaminanValo.IncreaseLightIntensity(0.1f);
        }
    }

    IEnumerator LowTemperature()
    {
        while (temperature > -20 && woodInKamina <= 0 && isLoweringTemperature)
        {
            yield return new WaitForSeconds(1);
            ChangeTemperature(-3);

            isLoweringTemperature = false;
            kaminanValo.DecreaseLigthIntensity(0.1f);
        }
    }

    IEnumerator LoseVirkeys()
    {
        while (virkeysasteAlogalla > 0)
        {
            yield return new WaitForSeconds(2f);
            ChangeVirkeys(-1);
            //virkeysasteAlogalla -= 1;
        }
    }

    IEnumerator ESCoolDowner()
    {
        esCooldownOn = true;
        yield return new WaitForSeconds(5f);
        esCooldownOn = false;
    }

    public void StartESCooldown()
    {
        if (esCooldownOn == false)
        {
            StartCoroutine(ESCoolDowner());
        }
    }
    public bool GetESCooldown()
    {
        return esCooldownOn;
    }
    IEnumerator GameOver()
    {
        endTimerOn = true;
        UIManager.Instance.ToggleButtons(false);
        if (!goodEnd)
        {
            AudioManager.Instance.PlayBGM(1);
        }
        else
        {
            AudioManager.Instance.PlayBGM(3);
        }

        yield return new WaitForSeconds(10);
        GoEndScene();
    }

    public void CheckTemperature()
    {
        if(temperature <= -10)
        {
            DialogUpdated("Baleleeee, äggiä puuda", null);
        }

        if (temperature <= -20)
        {
            // UIManager.Instance.AlogasSanoo("Voi VIDDU KAIKKI PALELTUIVAT :DDDD");
            DialogUpdated("Voi VIDDU KAIKKI PALELTUIVAT: DDDD", null);
            snowPS.gameObject.SetActive(true);
            gameIsOn = false;
            FreezeEnd = true;
            // Debug.Log("GameOver! KAIKKI PALELTUIVAT");
        }

        if(temperature > 70)
        {
            DialogUpdated("Voi viddu mehdän guollaan lämpöön däällä", null);
        }

        if (temperature > 85)
        {
            gameIsOn = false;
            burnEnd = true;
            //UIManager.Instance.AlogasSanoo("Voi viddu! ME kuollaa kuumuudeen :DDDD");
            DialogUpdated("Voi viddu! ME kuollaa kuumuudeen :DDDD", null);
            tentOnFire.gameObject.SetActive(true);
            //Debug.Log("KAIKKI KÄRISTYIVÄT");
        }
    }
    public void PlayDrinkAnimation()
    {
        spurdo.GetComponent<Animator>().SetTrigger("Drink");
    }
    public void PlayWoodInKaminaAnimation()
    {
        spurdo.GetComponent<Animator>().SetTrigger("woodInKamina");
    }

    public void PlayPoisPuuKaminasta()
    {
        spurdo.GetComponent<Animator>().SetTrigger("BackWoodInKamina");
    }

    public float GetTemperature()
    {
        return temperature;
    }

    public void ChangeTemperature(float f)
    {
        temperature += f;
        UIManager.Instance.UpdateThermoMeterBlock((f * 0.02f));

        if (temperature >= 30)
        {
            ChangeVirkeys(-2);
        }

        if(temperature >= 60)
        {
            ChangeVirkeys(-3);
        }
    }

    //if spurdo is tired show tissit else do nothing
    public void CheckVirkeys()
    {
        if (virkeysasteAlogalla > maxVirkeysasteAlogalla / 2)
        {
            ToggleDissit(false);
            //ToggleGaminanHenki(false);
        }

        if (virkeysasteAlogalla < maxVirkeysasteAlogalla / 2)
        {
            //UIManager.Instance.AlogasSanoo("Väsyddää. Olisiba Aligersantti Justiina täällä.");
            DialogUpdated("Väsyddää. Olisiba Aligersantti Justiina täällä.", null);
            zzParticle.gameObject.SetActive(true);
            ToggleDissit(true);
        }

        if (virkeysasteAlogalla < maxVirkeysasteAlogalla/2 && temperature > 70 && virkeysasteAlogalla > 0)
        {
            // UIManager.Instance.GaminanHenkiSanoo("OLEN GAMIINAN HENGI :-D EBIN :-D");
            //UIManager.Instance.AlogasSanoo("VOI HELVETTI!");
            DialogUpdated("BERGL...MITÄ?", "OLEN GAMIINAN HENGI :-D EBIN :-D");
            ToggleGaminanHenki(true);
            AudioManager.Instance.PlayBGM(2);
        }

        if (virkeysasteAlogalla <= 0)
        {
            //Game over
            //gameIsOn = false;
            //UIManager.Instance.AlogasSanoo("Zzzzzzzzz......");
            DialogUpdated("ZzZzzZzZzz......", null);
            //Debug.Log("Nukahdit saatana");
        }
    }

    public float GetVirkeys()
    {
        return virkeysasteAlogalla;
    }

    public void ChangeVirkeys(float f)
    {
        //pitäisi palauttaa virkeyttä just sen verran kuin halutaan eikä pitäisi mennä yli tai ali
        if(virkeysasteAlogalla > 0)
        {
            virkeysasteAlogalla = Mathf.Min(virkeysasteAlogalla + f, maxVirkeysasteAlogalla);
        }
       
    }
    public void AddWoodToKamina(int w)
    {
        if (woodInKamina < maxKaminaCapacity)
        {

            bool cantPutWoodInKamiina = UnityEngine.Random.Range(0f, 1f) > 0.75f;
            if (cantPutWoodInKamiina)
            {
                //UIManager.Instance.AlogasSanoo("hmm dissit... puita... myöhemmin...z Z  Z zzz");
                DialogUpdated("En mä jaksa laittaa puida :D", null);
            }
            else
            {
                woodInKamina += w;
                PlayWoodInKaminaAnimation();
                ChangeVirkeys(-5);
                //Debug.Log("Lisäddiin puida gaminaan" + woodInKamina);
                //UIManager.Instance.AlogasSanoo($"Lisäddiin puida gaminaan. SIellä on nyd {woodInKamina} puuta");
                DialogUpdated($"Lisäddiin puida gaminaan. Siellä on nyd {woodInKamina} puuda", null);
            }

        }
        else
        {
            //UIManager.Instance.AlogasSanoo("VOI VIDDU SIELLÄHÄN ON JO PUITA LIIKAA");
            DialogUpdated("VOI VIDDU :D  SIELLÄHÄN ON JO PUIDA LIIGAA :D", null);
            //Debug.Log("Sano jotain tyhmää spurdoki kun yrittää laittaa liikaa puita kaminaan");
        }
    }

    public void RemoveWoodFromKamina(int w)
    {
        if (woodInKamina > 0)
        {
            bool failToTakeWoodOut = UnityEngine.Random.Range(0f, 1f) > 0.8f;

            woodInKamina -= w;
            PlayPoisPuuKaminasta();
            if (failToTakeWoodOut)
            {
                bool tentIsBurning = UnityEngine.Random.Range(0f, 1f) > 0.8f;

                if (tentIsBurning)
                {
                    //UIManager.Instance.AlogasSanoo"Poldin iddeäni mudda olen vähän virgeämpi :D"
                    DialogUpdated("Deltta sytty palamaan kun otit vahingossa palavan buun pois EBIN :-D GG", null);
                    // Debug.Log("Teltta sytty palamaan kun otit vahingossa palavan puun pois GG");

                    tentOnFire.gameObject.SetActive(true);
                    burnEnd = true;
                    gameIsOn = false;
                }
            }
            else
            {
                //UIManager.Instance.AlogasSanoo($"Poldin iddeäni mudda olen vähän virgeämpi :D");
                DialogUpdated("Poldin iddeäni mudda, olen vähän virgeämpi :D", null);
                // ((Debug.Log("Poltit itseäsi, mutta sait puun turvallisesti pois. Olet vähän virkeämpi");
                ChangeVirkeys(7);
            }

        }
        else
        {
            //UIManager.Instance.AlogasSanoo("VOI MINUA DYHMÄÄ :D EI ole puida gaminassa :DDD EBIN");
            DialogUpdated("VOI MINUA DYHMÄÄ!:D EI ole puida gaminassa :DDD EBIN", null);
            //Debug.Log("Ei ole puita gaminassa");
        }
    }
    public void CheckKaminaCapacity()
    {
        if (woodInKamina <= 0)
        {
            if (!isLoweringTemperature)
            {
                isLoweringTemperature = true;
                StartCoroutine(LowTemperature());
            }
            firesound.SetActive(false);

        }
        else
        {
            if (!isBurningWood)
            {
                isBurningWood = true;
                StartCoroutine(BurnWood());
            }
            firesound.SetActive(true);
        }
    }
    public int GetKaminaCurrentCapacity()
    {
        return woodInKamina;
    }

    public bool getCanUseToivomusVesi()
    {
        return canUseToivomusVesi;
    }

    public void ShowToivomusVesi()
    {
        spurdo.GetComponent<Animator>().SetTrigger("showToivomusvesi");
        canUseToivomusVesi = true;  
    }

    public void CanUseToivomusVesi()
    {
        canUseToivomusVesi = true;
    }

    public void UseToivomusVetta()
    {
        spurdo.GetComponent<Animator>().SetTrigger("kaada");
        toivomusVesiEnd = true;
        gameIsOn = false;

    }

    public void SetTentFire()
    {
        tentOnFire.gameObject.SetActive(true);
        spurdoParticles.gameObject.SetActive(true);
    }

    public void ToggleDissit(bool b)
    {
        dissit.SetActive(b);
    }
    public void ToggleGaminanHenki(bool b)
    {
        gaminanHenki.SetActive(b);
        headObject.GetComponent<SpriteRenderer>().sprite = benisHead;
        if (gaminanHenki.activeInHierarchy)
        {
            UIManager.Instance.SetActiveDialogButton();
        }
    }

    public void TimeIsUp()
    {
        //UIManager.Instance.AlogasSanoo("EBIN :-D! Selvisiddin. Nyt runggaan peiton alle :D. Viljami Herädys! :-D");
        DialogUpdated("EBIN :-D! Selvisiddin. Nyt runggaan peiton alle :D. Viljami Herädys! :-D", null);
        goodEnd = true;
        gameIsOn = false;
        //AudioManager.Instance.PlayBGM(3);
    }


    public void DialogUpdated(string alogas, string hengi)
    {
        if (!gaminanHenki.activeInHierarchy)
        {
            if (alogas != null)
            {
                UIManager.Instance.AlogasSanoo(alogas);
            }

            if (hengi != null)
            {
                UIManager.Instance.GaminanHenkiSanoo(hengi);
            }
        }
    }
    public void GoEndScene()
    { 
        if(toivomusVesiEnd)
        {
            SceneManager.LoadScene("ToivomusEndScreen");
        }

        if(FreezeEnd)
        {
            SceneManager.LoadScene("FreezeEndScreen");
        }

        if(burnEnd)
        {
            SceneManager.LoadScene("BurnEndScreen");
        }

        if(goodEnd)
        {
            SceneManager.LoadScene("GoodEndScreen");
        }
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
