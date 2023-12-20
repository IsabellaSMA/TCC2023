using UnityEngine;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;     // The prefab of the bullet that will be fired
    public Transform firePointX;         // The position where the bullet will be fired from
    public Transform firePointYup;
    public Transform firePointYdown;

    public float bulletSpeed = 10f;     // The speed at which the bullet will move
    public float fireRate = 0.5f;       // The delay between shots in seconds
    private float nextFireTime = 0f;    // The time at which the next shot can be fired

    [SerializeField] GameObject[] Ammo;
    public int AmmoCount = 2;
    public bool canShoot = true;

    // Update is called once per frame
    private void FixedUpdate()
    {


        /*        if (Input.GetKeyDown(KeyCode.K) && !Input.GetKey(KeyCode.W) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + fireRate;
                    Fire(bulletSpeed, 0, firePointX);
                }
                if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.W) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + fireRate;
                    //FireYup();
                    Fire(0, bulletSpeed, firePointYup);
                }
                if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.S) && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + fireRate;
                    FireYDown();
                }*/

        if (canShoot == true)
        {
            FireDetection();
        }
        if (AmmoCount == 1)
        {
            Ammo[1].gameObject.SetActive(false);
        }
        else if (AmmoCount == 0 )
        {
            Ammo[0].gameObject.SetActive(false);
            canShoot = false;
        }
    }
    void FireDetection() 
    {
        if(canShoot == true) 
        {
            

            if (Input.GetKeyDown(KeyCode.K) && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;

                if (Input.GetKey(KeyCode.W))
                {
                    Fire(0, bulletSpeed, firePointYup);
                    AmmoCount--;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Fire(0, -bulletSpeed, firePointYdown);
                    AmmoCount--;
                }
                else
                {
                    Fire(bulletSpeed, 0, firePointX);
                    AmmoCount--;
                }
            }
        }
    }

    void Fire(float speedX, float speedY, Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //rb.velocity = firePointX.right * bulletSpeed;

        rb.velocity = new Vector2(speedX, speedY);

    }
    void FireYup()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePointYup.position, firePointYup.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePointYup.right * bulletSpeed;
    }
    void FireYDown()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePointYdown.position, firePointYdown.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePointYdown.right * bulletSpeed;
    }

}