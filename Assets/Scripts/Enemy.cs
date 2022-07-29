using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int rutina;
    public float crono;
    public Animator anim;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player");
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        AIEnemy();
    }

    public void AIEnemy()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > 50)
        {
            crono += 1 * Time.deltaTime;
            if(crono >= 4)
            {
                rutina = Random.Range(0, 2);
                crono = 0;

            }

            switch (rutina)
            {
                case 0:
                    anim.SetBool("walk", false);
                    break;

                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 4 * Time.deltaTime);
                    anim.SetBool("walk", true);
                    break;
            }
        }

        else 
        {
            anim.SetBool("walk", true);
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Bullet")
        {
            gameController.score += 10;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            gameController.GameOver();
        }
        
    }

}
