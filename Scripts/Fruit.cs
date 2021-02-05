using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    
    public int id;
    private bool isSend = true;
    private bool isPlayGround = true;
    public bool isTigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GameOver" && isTigger&&GameManger.instance.isGameOver==false)
        {
            GameManger.instance.isGameOver = true;
            GameManger.instance.GameOver();

        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tip" && isTigger)
        {
            GameManger.instance.tip.SetActive(true);
        }
        if (collision.tag == "GameOver" && isTigger && GameManger.instance.isGameOver == false)
        {
            GameManger.instance.isGameOver = true;
            GameManger.instance.GameOver();
        }
           
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tip" && isTigger)
        {
            GameManger.instance.tip.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag=="Fruit")
        {
            if (collision.collider.gameObject.GetComponent<Fruit>().id == id&&id<9)
            {
                GameManger.instance.fruitList.Remove(this);
                //StartCoroutine("Wait");

                collision.collider.gameObject.GetComponent<Fruit>().isSend=false;
                int newID = id + 1;
                if(isSend)
                GameManger.instance.Merge(newID,transform);
                AudioSource.PlayClipAtPoint(GameManger.instance.merge, Vector3.zero);
                Destroy(gameObject);
            }
             
           

        }else if(collision.collider.tag == "Ground")
        {
            if(isPlayGround)
            GameManger.instance.PlayFallSound();
            isPlayGround = false;
        }
        if(collision.collider.tag!="Border")
        isTigger = true;
        
    }
    
    
    IEnumerable Wait()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.1f));
    }
    public void DestoryFruit()
    {
        Destroy(gameObject);
    }
    public void Explosion()
    {
        AudioSource.PlayClipAtPoint(GameManger.instance.merge, Vector3.zero);
        GameObject fruitEffect = Instantiate(GameManger.instance.effect, GameManger.instance.canvas);
        fruitEffect.transform.position =transform.position;
        Destroy(fruitEffect, 1f);
        Destroy(gameObject, 1f);
    }
}
