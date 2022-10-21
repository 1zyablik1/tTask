using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform testTarget;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Transform bulletSpawner;

    private float speed = 5;
    private Pool bullets;

    private void Awake()
    {
        bullets = new Pool(bullet, bulletContainer.gameObject);
    }

    void Start()
    {
        LookAtTarget();
        Fire();
    }

    void Update()
    {
        LookAtTarget();
        //Fire();
    }

    public void LookAtPoint(Vector3 point)
    {
        Vector2 direction = point - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = rotation;
    }

    public void LookAtTarget()
    {
        Vector2 direction = testTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = rotation;
    }

    public void Fire()
    {
        //testTarget.GetComponent<Target>().RecalculatePosition();
        var bullet = bullets.GetFreeElement(bulletSpawner.position, this.transform.rotation);
        StartCoroutine(A(bullet));
    }

    IEnumerator A(GameObject a)
    {
        for (; ; )
        {
            a.transform.Translate(Vector3.right * Time.deltaTime * speed);
                //+= Vector3.right * Time.deltaTime * 5;

            yield return new WaitForEndOfFrame();
        }
    }
}
