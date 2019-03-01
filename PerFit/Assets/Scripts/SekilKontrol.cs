﻿using System.Collections;
using UnityEngine;

[System.Serializable]
public class SekilKontrol : MonoBehaviour {

    [Header("Speed")]
    [Range(0f, 1f)]
    public float donmeHizi;
    [Header("Panel Scale")]
    [Range(0f, 1f)]
    public float minPanelScale;
    [Range(0f, 1f)]
    public float maxPanelScale;
    [Header("Destroying Time")]
    public float destroyingTimeForObject;

    float donmeYonu;
    bool sekliPaneleSabitleKontrol = false;
    float rightOrLeft;

    GameObject Panel;
    GameObject cutPoint;
    GameObject fitPoint;
    OyunKontrol oyunKontrol;

    public void Start()
    {
        objeBul();
        sekilDonmeYonu();
        sekilBoyutAyarla();
    }

    public void Update()
    {
        sekilDondur();
        paneleSabitle();
    }

    public virtual void objeBul() //specialSkontrol'da override yapabilmemiz icin "virtual" 
    {
        Panel       = GameObject.FindGameObjectWithTag("panelTag");
        cutPoint    = GameObject.FindGameObjectWithTag("cutPointTag");
        fitPoint    = GameObject.FindGameObjectWithTag("fitPointTag");
        oyunKontrol = GameObject.FindGameObjectWithTag("oyunKontrolTag").GetComponent<OyunKontrol>();
    }

    public void sekilDonmeYonu()
    {
        rightOrLeft = Random.value; //random.value 0 ile 1 arasında random deger secer
        donmeYonu = rightOrLeft <= 0.5f ? 1 : -1;

        //if (rightOrLeft <= 0.5f) 
        //    donmeYonu = 1; //sola donmesi icin
        //else
        //    donmeYonu = -1; // saga donmesi icin
    }

    public void sekilBoyutAyarla()
    {
        float sekilBoyut = Random.Range(minPanelScale, maxPanelScale);
        Debug.Log("<color=black>sekil boyutu:</color> " + sekilBoyut);
        gameObject.transform.localScale = new Vector3(sekilBoyut, gameObject.transform.localScale.y, sekilBoyut); 
    }

    public void sekilDondur()
    {
        if (!sekliPaneleSabitleKontrol)
        {
            transform.Rotate(new Vector3(0, Random.Range(0, 360f), 0) * ((Time.deltaTime * donmeYonu) * donmeHizi));
        }
    }

    public void paneleSabitle() //objeleri panele sabitleyerek onunla birlikte hereket etmesini sağlayan fonksiyon
    {
        if (sekliPaneleSabitleKontrol)
        {
            transform.localRotation = Panel.transform.localRotation; //şeklin panele sabitlenmesi
        }

        cutPoint.transform.localRotation = Panel.transform.localRotation; //cutpointin panele sabitlenmesi
        fitPoint.transform.localRotation = Panel.transform.localRotation; //fitpointin panele sabitlenmesi
    }

    protected virtual void OnTriggerEnter(Collider other) //gecirgen yuzeye temas ettiginde
    {
        if (other.name == "FitPoint")
        {
            Panel.GetComponent<PanelKontrol>().panelParticleOynat(true);
            FindObjectOfType<SesKontrol>().sesOynat("FitSound");
            donmeHizi = 0f;
            Panel.transform.localScale += new Vector3(0f, 0.2f, 0f);
            oyunKontrol.score += 1;
            sekliPaneleSabitleKontrol = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "cutPointTag")
        {
            other.transform.localScale = new Vector3(gameObject.transform.localScale.x/5,
                                                     other.transform.localScale.y,
                                                     gameObject.transform.localScale.z/5);
            StartCoroutine(destroySquare());
        }
    }

    IEnumerator destroySquare()
    {
        //Debug.Log(destroyingTimeForObject);
        yield return new WaitForSeconds(destroyingTimeForObject);
        Destroy(gameObject);
    }

}
