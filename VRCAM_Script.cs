using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VRCAM_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;


    public Transform mycam;

    public GameObject focusobj;
    public GameObject temp;
    public Transform viewpos;

    public float timeelapsed = 0;
    public bool starttimer;
    public Image progressimage;


    void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();
        starttimer = false;
        progressimage.fillAmount = 0;

    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        print(progressimage.fillAmount);

        progressimage.fillAmount = timeelapsed / 1200;

        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }
       


        Ray myray = new Ray(mycam.transform.position, mycam.transform.forward);
        Debug.DrawRay(mycam.transform.position, mycam.transform.forward * 50f, Color.yellow);
        

        if (Physics.Raycast(myray, out RaycastHit myrayhitspot))
        {
            //print(myrayhitspot.collider.name);

            // myrayhitspot.collider.GetComponent<Renderer>().material.color = Color.red;

            if (myrayhitspot.collider.tag == "wp")
            {
                focusobj = myrayhitspot.collider.gameObject;
            }
            else
            {
                focusobj = temp;
            }

            

            if (focusobj.tag == "wp")
            {
                starttimer = true;


            }

            if (focusobj.tag != "wp")

            {

                print("Gaze Lost");
                focusobj = temp;

                progressimage.fillAmount = 0;
                starttimer = false;
                timeelapsed = 0;
                
            }

            //
        }


        if(starttimer == true)
        {
            
                timeelapsed = timeelapsed + 1;

                if (progressimage.fillAmount >=1)
                {
                    viewpos = focusobj.transform;
                    cameraContainer.transform.position = viewpos.transform.position;
                    focusobj.GetComponent<Renderer>().material.color = Color.blue;
                }

        }
        
    }


    public void colors()
    {

             

    }
}
