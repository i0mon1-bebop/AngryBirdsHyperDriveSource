using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneFaderTitle : MonoBehaviour
{
    public Image image;
    public AudioSource source;
    public AudioSource source2;
    public AudioClip music;

    public AudioClip okay;

    public Animator anim;
    public Animator anim2;

    public float timer1;
    public bool movetimer1;
    public float maxtimer2;
    public SavedData _dataTest;

    private void Start()
    {
        source.clip = music;
        source.Play();
        StartCoroutine(FadeOut());
        _dataTest = GameObject.FindGameObjectWithTag("Data")?.GetComponent<SavedData>();
    }


    IEnumerator Fader(float duration)
    {
        float t = 0;
        Color c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            image.color = c;
            yield return null;
        }
        if (t > duration)
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
        if (movetimer1 == true)
        {
           if (timer1 < maxtimer2)
            {
                timer1 += 2 * Time.deltaTime;
            }
            
        }
        if (timer1 > maxtimer2)
        {
            if (movetimer1 == true)
            {
                movetimer1 = false;
                int count = Random.Range(1, 3);

                if (count == 1)
                {
                    SceneManager.LoadSceneAsync("MainGAME");
                }
                else if (count == 2)
                {
                    SceneManager.LoadSceneAsync("MainGAME 1");
                }


            }

        }

        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("YES");
            if (_dataTest != null)
            {
                _dataTest.currentLives = _dataTest.maxLives;
            }
        
            anim.SetBool("End", true);
            anim2.SetBool("End", true);
           source2.PlayOneShot(okay);
            StartCoroutine(Fader(0.6f));
        }

    }
}