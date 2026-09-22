using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedData : MonoBehaviour
{

    public static SavedData Instance { get; private set; }

    public float currentLives;
    public float maxLives;

    public bool testa;

    public float highScore;

    public PlrMovement plr;

    // Start is called before the first frame update
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
           
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
      

    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null)
        {
            plr = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlrMovement>();
        }
      


        if (plr != null)
        {
          

            if (plr.Dead == false  && testa == true)
            {
                testa = false;
                plr.ChangeText(plr.livestest, currentLives.ToString("000"));

            }
            if (plr.Dead == true && plr.Useless == false && testa == false)
            {
                testa = true;
                if (currentLives >= 0)
                {
                    currentLives -= 1;
                    plr.ChangeText(plr.livestest, currentLives.ToString("000"));
                }
                if (currentLives == 0)
                {
                    currentLives = maxLives;
                    plr.GoToTitle = true;
                    plr.ChangeText(plr.livestest, "000");
                    

                }
              

            }
        }

    }
}
