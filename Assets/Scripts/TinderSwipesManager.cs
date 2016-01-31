using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class TinderSwipesManager : MonoBehaviour
{
    public Animator[] TinderSpider1;
    public Animator[] TinderSpider2;

    public Image likeSpider1;
    public Image nopeSpider1;
    public Image likeSpider2;
    public Image nopeSpider2;

    private List<int> m_spider1Likes;
    private List<int> m_spider2Likes;

    // Use this for initialization
    void Start ()
    {
        m_spider1Likes = new List<int>();
        m_spider2Likes = new List<int>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator TimeToPickUpChicks(int likesSpider1, int likesSpider2)
    {
        AudioManager.Self.TinderScreenIn();
        yield return StartCoroutine(GameElements.Self.tinderScoring.InteruptAndFadeIn());

        GenerateRandomIntList(likesSpider1, likesSpider2);

        for (int i = 0; i < 10; i++)
        {
            if (m_spider1Likes[m_spider1Likes.Count - 1] == 1)
            {
                likeSpider1.enabled = true;
                AudioManager.Self.TinderPlayer1(true);
                AudioManager.Self.TinderSwoosh();
                TinderSpider1[i].Play("TinderSwipeRight");
            }
            else
            {
                nopeSpider1.enabled = true;
                AudioManager.Self.TinderPlayer1(false);
                AudioManager.Self.TinderSwoosh();
                TinderSpider1[i].Play("TinderSwipeLeft");
            }
            m_spider1Likes.RemoveAt(m_spider1Likes.Count - 1);

            if (m_spider2Likes[m_spider2Likes.Count - 1] == 1)
            {
                likeSpider2.enabled = true;
                AudioManager.Self.TinderPlayer2(true);
                AudioManager.Self.TinderSwoosh();
                TinderSpider2[i].Play("TinderSwipeRight");
            }
            else
            {
                nopeSpider2.enabled = true;
                AudioManager.Self.TinderPlayer2(true);
                AudioManager.Self.TinderSwoosh();
                TinderSpider2[i].Play("TinderSwipeLeft");
            }
            m_spider2Likes.RemoveAt(m_spider2Likes.Count - 1);

            yield return new WaitForSeconds(0.4f);

            likeSpider1.enabled = false;
            nopeSpider1.enabled = false;
            likeSpider2.enabled = false;
            nopeSpider2.enabled = false;

            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
        yield return StartCoroutine(GameElements.Self.tinderScoring.InteruptAndFadeOut());
        for (int i = 0; i < 10; i++)
        {
            TinderSpider1[i].Play("New State");
            TinderSpider2[i].Play("New State");
        }
    }

    private void GenerateRandomIntList(int likes1, int likes2)
    {
        List<int> likesSpider1 = new List<int>();
        List<int> nopesSpider1 = new List<int>();
        List<int> likesSpider2 = new List<int>();
        List<int> nopesSpider2 = new List<int>();

        // Generate likes
        for (int i = 0; i < likes1; i++)
        {
            likesSpider1.Add(1);       // Adding a new even number to the list
        }

        // Generate likes
        for (int i = 0; i < likes2; i++)
        {
            likesSpider2.Add(1);       // Adding a new even number to the list
        }

        // Generate nopes
        for (int i = 0; i < 10 - likes1; i++)
        {
            nopesSpider1.Add(0);       // Adding a new even number to the list
        }

        // Generate nopes
        for (int i = 0; i < 10 -likes2; i++)
        {
            nopesSpider2.Add(0);       // Adding a new even number to the list
        }

        // Creating the whole randomized sequence of numbers, joining even and odds
        for (int i = 0; i < 10; i++)
        {
            // Checks if there are still both even and odd numbers
            if (likesSpider1.Count > 0 && nopesSpider1.Count > 0)
            {
                // Randomly choose to take a like or a nope
                int choice = UnityEngine.Random.Range(0, 2);
                // Choice: Like
                if (choice == 0)
                {
                    m_spider1Likes.Add(likesSpider1[likesSpider1.Count - 1]);  // Add even number to the list
                    likesSpider1.RemoveAt(likesSpider1.Count - 1);  // Remove the number just used
                }
                else
                {
                    m_spider1Likes.Add(nopesSpider1[nopesSpider1.Count - 1]);  // Add even number to the list
                    nopesSpider1.RemoveAt(nopesSpider1.Count - 1);  // Remove the number just used
                }
            }
            else if (nopesSpider1.Count > 0)  // if only odd numbers remained
            {
                m_spider1Likes.Add(nopesSpider1[nopesSpider1.Count - 1]);  // Add even number to the list
                nopesSpider1.RemoveAt(nopesSpider1.Count - 1);
            }
            else // if only even numbers remained
            {
                m_spider1Likes.Add(likesSpider1[likesSpider1.Count - 1]);  // Add even number to the list
                likesSpider1.RemoveAt(likesSpider1.Count - 1);  // Remove the number just used
            }
        }

        for (int i = 0; i < 10; i++)
        {
            // Checks if there are still both even and odd numbers
            if (likesSpider2.Count > 0 && nopesSpider2.Count > 0)
            {
                // Randomly choose to take a like or a nope
                int choice = UnityEngine.Random.Range(0, 2);
                // Choice: Like
                if (choice == 0)
                {
                    m_spider2Likes.Add(likesSpider2[likesSpider2.Count - 1]);  // Add even number to the list
                    likesSpider2.RemoveAt(likesSpider2.Count - 1);  // Remove the number just used
                }
                else
                {
                    m_spider2Likes.Add(nopesSpider2[nopesSpider2.Count - 1]);  // Add even number to the list
                    nopesSpider2.RemoveAt(nopesSpider2.Count - 1);  // Remove the number just used
                }
            }
            else if (nopesSpider2.Count > 0)  // if only odd numbers remained
            {
                m_spider2Likes.Add(nopesSpider2[nopesSpider2.Count - 1]);  // Add even number to the list
                nopesSpider2.RemoveAt(nopesSpider2.Count - 1);
            }
            else // if only even numbers remained
            {
                m_spider2Likes.Add(likesSpider2[likesSpider2.Count - 1]);  // Add even number to the list
                likesSpider2.RemoveAt(likesSpider2.Count - 1);  // Remove the number just used
            }
        }
    }
}
