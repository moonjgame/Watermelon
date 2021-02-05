using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public GameObject[] fruits;
    public Transform canvas;
    public static GameManger instance;
    private GameObject current;
    private float width;
    public Text score;
    private int scoreNumber = 0;
    public AudioClip fallDown;
    public AudioClip merge;
    public AudioClip effectSound;
    private AudioSource audioSource;
    public GameObject effect;
    public GameObject tip;
    public GameObject watermelonEffect;
   public bool isGameOver = false;
    public List<Fruit> fruitList;
    private int id = 0;
    public GameObject gameOverPanel;
    public Text gameoverScore;
    public void PlayFallSound()
    {
        AudioSource.PlayClipAtPoint(fallDown, Vector3.zero);
    }
   

    private void Awake()
    {
        instance = this;
        width = Screen.width;
        audioSource = GetComponent<AudioSource>();
        fruitList = new List<Fruit>();
    }
    void Start()
    {
        current = Instantiate(fruits[Random.Range(0, 1)], canvas);
        current.transform.position = transform.position;
        current.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        fruitList.Add(current.GetComponent<Fruit>());
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(fruitList.Count);
        if (isGameOver)
        {
            
            return;
        }
           
        if (current != null)
        {
            Vector3 mousePos = Input.mousePosition;
            float radius = current.GetComponent<RectTransform>().rect.width / 2.0f;
            if (Input.GetMouseButton(0))
            {

                // Vector3 pos = Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x, mousePos.y, 0));
                if ((Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x - radius, mousePos.y, 0)).x <= 0f))
                {
                    current.transform.position = new Vector3(radius, transform.position.y, transform.position.z);

                }
                else if ((Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x + radius, mousePos.y, 0)).x >= 1f))
                {
                    current.transform.position = new Vector3(Screen.width - radius , transform.position.y, transform.position.z);
                }
                else
                {
                    current.transform.position = new Vector3(mousePos.x, transform.position.y, transform.position.z);
                }



            }
            if (Input.GetMouseButtonUp(0))
            {
               
                if ((Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x - radius, mousePos.y, 0)).x <= 0f))
                {
                    current.transform.position = new Vector3(radius, transform.position.y, transform.position.z);

                }
                else if ((Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x + radius, mousePos.y, 0)).x >= 1f))
                {
                    current.transform.position = new Vector3(Screen.width - radius, transform.position.y, transform.position.z);
                }
                else
                {
                    current.transform.position = new Vector3(mousePos.x, transform.position.y, transform.position.z);
                }
                current.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                //fruitList.Remove(current.GetComponent<Fruit>());
                current = null;
                StartCoroutine("Wait");
            }
        }
        
       
    }

    public void Merge(int id,Transform pos)
    {
        // Debug.Log(id);
        if (id > 9) return;
        if (id <=8)
        {
            GameObject go = Instantiate(GameManger.instance.fruits[id], canvas);
            go.transform.position = pos.position;
            fruitList.Add(go.GetComponent<Fruit>());
            GameObject fruitEffect = Instantiate(effect, canvas);
            fruitEffect.transform.position = pos.position;
            Destroy(fruitEffect, 1f);
        }else if (id == 9)
        {

            AudioSource.PlayClipAtPoint(effectSound, Vector3.zero);
            GameObject go = Instantiate(GameManger.instance.fruits[id], canvas);
            go.transform.position = pos.position;
            fruitList.Add(go.GetComponent<Fruit>());
            GameObject fruitEffect = Instantiate(effect, canvas);
            fruitEffect.transform.position = pos.position;
            Destroy(fruitEffect, 1f);
            GameObject star = Instantiate(watermelonEffect, canvas);
            Destroy(star, 1f);
        }
       

        scoreNumber += id * id;
        score.text = scoreNumber.ToString();
        
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        current = Instantiate(fruits[Random.Range(8,8)], canvas);
        current.transform.position = transform.position;
        current.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        fruitList.Add(current.GetComponent<Fruit>());
    }

     
    public void GameOver()
    {
        StopAllCoroutines();
        for(int i = 0; i < fruitList.Count; i++)
        {
            Invoke("WaitExplosion", i-(i*0.7f));
             
        }
        Invoke("ShowGameOverPanel", 5f);
    }

    public void WaitExplosion()
    {
        if (id > fruitList.Count-1)
            return;
       fruitList[id++].Explosion();
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameoverScore.text = scoreNumber.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
