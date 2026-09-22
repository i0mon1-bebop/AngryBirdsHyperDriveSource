using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SucessTest : MonoBehaviour
{

    public Transform player;
    public PlrMovement testy;

    public Image star1;
    public Image star2;
    public Image star3;

    public AudioSource source;
    [SerializeField] private AudioClip boom;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        testy = player.GetComponent<PlrMovement>();
    }

    void BoomSound(float num)
    {
        if (num == 1 && testy.score >= testy.ScoreForOneStar)
        {
            source.PlayOneShot(boom);
        }
        else if (num == 2 && testy.score >= testy.ScoreForTwoStar)
        {
            source.PlayOneShot(boom);
        }
        else if (num == 3 && testy.score >= testy.ScoreForThreeStar)
        {
            source.PlayOneShot(boom);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (testy.score >= testy.ScoreForOneStar) 
        {
            star1.enabled = true;
        
        }
        else if (testy.score < testy.ScoreForOneStar)
        {
            star1.enabled = false;

        }
        if (testy.score >= testy.ScoreForTwoStar)
        {
            star2.enabled = true;

        }
        else if (testy.score < testy.ScoreForTwoStar)
        {
            star2.enabled = false;

        }
        if (testy.score >= testy.ScoreForThreeStar)
        {
            star3.enabled = true;

        }
        else if (testy.score < testy.ScoreForThreeStar)
        {
            star3.enabled = false;

        }


    }
}
