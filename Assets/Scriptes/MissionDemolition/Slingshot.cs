using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    [Header("Set in Inspector")]
    public GameObject prefabProjecttile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;//launchPos хранит трехмерные мировые координаты launchPoint.
    public GameObject projectile;//Projectile — это ссылка на вновь созданный экземпляр Projectile
    public bool aimingMode;
    /*aimingMode — обычно имеет значение false и получает значение true, когда
      игрок нажимает кнопку мыши с номером 0, когда указатель находится
      над объектом Slingshot. Эта переменная управляет поведением остального
      кода.
    */
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null)
            {
                return Vector3.zero;
            }
            return S.launchPos;
        }
    }
    private void Awake()
    {
        S = this;
        /*transform. Find ("LaunchPoint”) найдет дочерний объект с именем LaunchPoint,
       вложенный в Slingshot, и вернет его компонент Transform. Следующая строка
       получит игровой объект GameObject, владеющий этим компонентом Transform,
       и сохранит ссылку на него в поле launch Point.
        */
        Transform lauchPointTrans = transform.Find("LaunchPoint");
        launchPoint = lauchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = lauchPointTrans.position;

    }
    private void OnMouseEnter()
    {
        print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
    }
    private void OnMouseExit()
    {
        print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }
    private void OnMouseDown()
    {
        //ИГРОК НАЖАЛ КНОПКУ МЫШИ, КОГДА УКАЗАТЕЛБЬ НАХОДИЛСЯ НАД РОГАТКОЙ
        aimingMode = true;
        //СОЗДАТЬ СНАРЯД
        projectile = Instantiate(prefabProjecttile) as GameObject;
        //ПОМЕСТИТЬ В ТОЧКУ
        projectile.transform.position = launchPos;
        //СДЕЛАТЬ ЕГО КИНЕМАТИЧЕСКИМ
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    private void Update()
    {
        //ЕСЛИ ИГРОК НЕ В РЕЖИМЕ ПРИЦЕЛИВАНИЯ, НЕ ВЫПОЛНЯТЬ ЭТОТ КОД
        if (!aimingMode) return;
        //ПОЛУЧИТЬ ТЕКУЩИЕ ЭКРАННЫЕ КООРДИНАТЫ УКАЗАТЕЛЯ МЫШИ
        Vector3 mousePos2D = Input.mousePosition;//Преобразовать координаты указателя мыши из экранных координат в мировые.
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //НАЙТИ РАЗНОСТЬ КООРДИНАТ МЕЖДУ LAUNCHPOS И MOUSEPO2D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //ОГРАНИЧИТЬ MOUSEDELTA РАДИУСОМ КОЛЛАЙДЕРА ОБЬЕКТА SLINGSHOT
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;//Этот код ограничивает перемещение снаряда так, чтобы его центр оставался
        //в пределах коллайдера Sphere Collider игрового объекта Slingshot
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //ПЕРЕДВИНУТЬ СНАРЯД В НОВУЮ ПОЗИЦИЮ
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            //КНОПКА МЫШИ ОТПУЩЕНА
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();/*Так как метод ShotFiredQ в MissionDemolition объявлен как статический,
к нему можно обращаться с использованием имени самого
класса MissionDemolition, а не через конкретный экземпляр. Когда
MissionDemolition. Shot Fired () вызывается сценарием Slingshot, он увеличивает
значение MissionDemolition. S. shotsTaken.*/
            ProjectileLine.S.poi = projectile;/*Эта строка заставляет ProjectileLine следовать за новым снарядом Projectile
после выстрела.*/
        }
    }
}
