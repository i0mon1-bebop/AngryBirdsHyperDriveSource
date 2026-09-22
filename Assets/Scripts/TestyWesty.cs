using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneFader : MonoBehaviour
{
    public Image image;
    public PlrMovement plr;

    public float timer1;
    public bool movetimer1;
    public float maxtimer2;

    public float timer2;
    public bool movetimer2;
    public float maxtimer3;

    public Animator anim;
    public AudioSource source;
    public AudioClip gameover;


    public SavedData _dataTest;

    private void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlrMovement>();
        StartCoroutine(FadeOut());
        _dataTest = GameObject.FindGameObjectWithTag("Data").GetComponent<SavedData>();
    }


    IEnumerator Fader(float duration, bool RestartStage, string NextStage)
    {
        float t = 0;
        Color c = image.color;
        while (t < duration && movetimer1 == false && movetimer2 == false)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            image.color = c;
            yield return null;
        }
        if (movetimer2 == true)
        {
            Color cA = image.color;
            cA.a = 1;
            image.color = cA;

            if (timer2 < maxtimer3)
            {
                timer2 += 2 * Time.deltaTime;
            }
            else if (timer2 > maxtimer3)
            {
                SceneManager.LoadSceneAsync(NextStage);
            }
        }
       
        if (movetimer1 == true)
        {
            Color cA = image.color;
            cA.a = 1;
            image.color = cA;


            if (timer1 < maxtimer2)
            {
                timer1 += 2 * Time.deltaTime;
            }
            else if (timer1 > maxtimer2)
            {
              

                movetimer1 = false;
                if (RestartStage == true)
                {
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                }
               


                if (RestartStage == false)
                {
                    if (movetimer2 == false)
                    {
                        plr.rb.isKinematic = true;

                        movetimer2 = true;
                        source.PlayOneShot(gameover);

                        anim.SetBool("GameOver", true);

                    }
                
                }
               
            }
        }

        else if (t > duration && movetimer1 == false)
        {
            movetimer1 = true;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        Color c = image.color;
        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / 1f);
            image.color = c;
            yield return null;
        }
    }

    void Update()
    {
        if (plr.Dead == true && plr.GoToTitle == false)
        {
            StartCoroutine(Fader(0.6f,true,"GameOver"));
        }
        if (plr.GoToTitle == true)
        {
            StartCoroutine(Fader(0.6f, false, "Title"));
        }
    
    }
}