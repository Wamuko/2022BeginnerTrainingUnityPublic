using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    GameObject turret;
    [SerializeField]
    GameObject shotPoint;
    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    LineRenderer lineRenderer;

    float rotationSpeed = 45.0f; // ????
    float verticalRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ????????
        if (Physics.SphereCast(shotPoint.transform.position, Bullet.transform.localScale.x * 0.5f, turret.transform.forward, out RaycastHit hit, 30))
        {
            // LineRenderer??
            lineRenderer.SetPosition(0, shotPoint.transform.position);
            lineRenderer.SetPosition(1, shotPoint.transform.position + (turret.transform.forward * hit.distance));
            lineRenderer.gameObject.SetActive(true);
        } else
        {
            lineRenderer.gameObject.SetActive(false);
        }

        {
            // ??????????
            float horizontalInput = Input.GetAxis("Horizontal");
            turret.transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0, Space.World);

            // ??????????
            float verticalInput = Input.GetAxis("Vertical");
            verticalRotation += verticalInput * rotationSpeed * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation, 0.0f, 90.0f);

            // ??????????
            turret.transform.localEulerAngles = new Vector3(-verticalRotation, turret.transform.localEulerAngles.y, 0);
        }

        // ????
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet;
            bullet = Instantiate(Bullet, shotPoint.transform.position, Quaternion.identity);

            bullet.GetComponent<Bullet>().SetDirection(turret.transform.forward);
            
        }
    }
}
