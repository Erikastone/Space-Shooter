using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;//ВХЯКН НАКЮЙНБ
    public GameObject cloudPrefab;//ЬЮАКНМ ДКЪ НАКЮЙНБ
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;//ЛХМ ЛЮЬРЮА ЙЮФДНЦН НАКЮЙЮ
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;//ЙНЩТХЖХЕМР ЯЙНПНЯРХ НАКЮЙНБ

    private GameObject[] cloudInstances;
    private void Awake()
    {
        //янгдюрэ люяяхб дкъ упюмемхъ бяеу щйгелокъпнб накюйнб
        cloudInstances = new GameObject[numClouds];
        //мюирх пндхрекэяйхи хцпонбни наэейр CLOUDANCHOR
        GameObject anchor = GameObject.Find("CloudAnchor");
        //янгдюрэ б жхйке гюдюммне йнкхвеярбн накюйнб
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);
            //бшапюрэ леярноннфемхе дкъ накюйA
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //люьрюахпнбюрэ накюйн
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //лемэьхе накюйю ( я лемэьхл гмювемхел SCALEU) днкфмш ашрэ акхфс йгелке
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //лемэьхе накюйю днкфмш ашрэ дюкэье
            cPos.z = 100 - 90 * scaleU;
            //опхлемхрэ онксвеммше гмювемхъ йннпдхмюр х люьрюаю й накюйс
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //ядекюрэ накюйн днвепмхл он нрмньемхч й ANCHOR
            cloud.transform.SetParent(anchor.transform);
            //днаюбхрэ накюйн б люяяхб CLOUDINSTANCES
            cloudInstances[i] = cloud;

        }
    }
    private void Update()
    {
        //нанирх б жхйке бяе янгдюммше накюйю
        foreach (GameObject cloud in cloudInstances)
        {
            //онксвхрэ люьрюа х йннпдхмюрш накюйю
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //сбекхвхрэ яйнпнярэ дкъ акхфмху накюйнб
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //еякх накюйн ялеярхкняэ якхьйнл дюкейн бкебн
            if (cPos.x <= cloudPosMin.x)
            {
                //оепелеярхрэ ецн дюкейн б опюбн
                cPos.x = cloudPosMax.x;
            }
            //опхлемхрэ мнбше йннпдхмюрш й накюйс
            cloud.transform.position = cPos;
        }
    }
}
