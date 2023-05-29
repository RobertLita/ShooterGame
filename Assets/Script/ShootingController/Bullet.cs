using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public string Tag;
    public GameObject Originiated;
    public int reduction = 10;
    private void OnCollisionEnter(Collision collision)
    {


        if(collision.gameObject.GetComponent<HealthController>()!=null)
                   collision.gameObject.GetComponent<HealthController>().ReduceHealth(reduction, Originiated);
        else {}
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<HealthController>() != null)
            collision.gameObject.GetComponent<HealthController>().ReduceHealth(reduction, Originiated);


        if (collision.gameObject.tag == Tag)
        {
            Debug.Log("COLLIDED WITH:" + collision.gameObject.name + " \\" + Tag);

        }


        if (SceneManager.GetActiveScene().name == "Defense")
        {
            if (collision.gameObject.layer == 7)
            {
                Destroy(this.gameObject);
            }
        }

    }
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        transform.parent = null;
        rb.AddRelativeForce(new Vector3(0.0f, 0.0f, speed), ForceMode.VelocityChange);

        Destroy(this.gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
