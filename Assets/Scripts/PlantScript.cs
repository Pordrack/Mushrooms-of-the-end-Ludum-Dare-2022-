using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantScript : MonoBehaviour
{
    private float initialWaterLevel;
    public float WaterLevel = 10;
    public float MaxWaterLevel = 15;

    public float RessourcesPerSeconds = 3;

    public Image WaterBar;
    float WaterBarY = -3;

    public SpriteRenderer plantSpriteRenderer;
    public Sprite[] Images; //Les images representant les differents niveaux de secheresse de la plante.
    public Sprite DeathImage; //L'image de la plante morte

    public bool IsDead = false;

    //Les sons de l'arbre
    public List<AudioSource> DeathSounds;

    // Start is called before the first frame update
    void Start()
    {
        WaterBar.transform.position = new Vector3(transform.position.x, transform.position.y + WaterBarY, transform.position.z);
        initialWaterLevel = WaterLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            return;
        }
        //On pert de l'eau en fonction du climat
        WaterLevel -= Time.deltaTime * ClimateScript.Instance.DrynessRate;
        WaterBarFiller();

        //On change de sprite en fonction du pourcentage d'eau restant
        float percentage = WaterLevel/MaxWaterLevel;
        int currentSprite = (int)Mathf.Floor((1-percentage) * (Images.Length));
        //Debug.Log(percentage);
        if (currentSprite >= Images.Length)
        {
            currentSprite = Images.Length - 1;
        }
        plantSpriteRenderer.sprite = Images[currentSprite];

        WaterBar.transform.position = new Vector3(transform.position.x, transform.position.y + WaterBarY, transform.position.z);

        //On regarde si on doit mourir
        if (WaterLevel <= 0)
        {
            Die();
            return;
        }

        ShopScript.Instance.Money += Time.deltaTime * RessourcesPerSeconds;
    }

    void WaterBarFiller() //remplissage visible de la jauge d'eau
    {
        WaterBar.fillAmount = WaterLevel / MaxWaterLevel;     
    }

    public void Die()
    {
        if (IsDead)
            return;
        IsDead = true;
        plantSpriteRenderer.sprite = DeathImage;
        //On joue aussi un son special
        int randomIndex = Random.Range(0, DeathSounds.Count);
        DeathSounds[randomIndex].Play();
    }

    public void Water(float waterAmount)
    {
        if (IsDead)
        {
            return;
        }
        WaterLevel += waterAmount;

        if (WaterLevel > MaxWaterLevel)
        {
            WaterLevel = MaxWaterLevel;
        }
    }
}
