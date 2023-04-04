using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    [Header("Set Dynamically")]
    public float camZ;//ЖЕЛАЕМАЯ КООРДИНАТА Z КАМЕРЫ
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    private void FixedUpdate()
    {
        /* //ВЫЙТИ ЕСЛИ НЕТ ИНТЕРЕСУЮЩЕГО ОБЪЕКТА
         if (POI == null) return;
         //ПОЛУЧИТЬ ПОЗИЦИЮ ИНТЕРЕСУЮЩЕГО ОБЪЕКТА
         Vector3 desnation = POI.transform.position;
        */

        Vector3 destination;
        //ЕСЛИ НЕТ ИНТЕРЕСУЮЩЕГО ОБЬЕКТА, ВЕРНУТЬ 0 0 0
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //ПОЛУЧИТЬ ПОЗИЦИЮ ИНТЕРЕСУЮЩЕГО ОБЬЕКТА
            destination = POI.transform.position;
            //ЕСЛИ ИНТЕРЕСУЮЩИЙ ОБЬЕКТ - СНАРЯД, УБЕДИТЬСЯ,  ЧТО ОН ОСТАНОВИЛСЯ
            if (POI.tag == "Projectile")
            {
                //ЕСЛИ ОН СТОИТ НА МЕСТЕ 
                if (POI.GetComponent<Rigidbody>().IsSleeping())/*Теперь, когда снаряд прекратит полет и остановится (то есть когда метод
Rigidbody. IsSleeping() вернет значение правда), сценарий FollowCam присвоит
null полю P0I, и камера вернется в исходную позицию.*/
                {
                    //ВЕРНУТЬ ИСХОДНЫЕ НАСТРОЙКИ ПОЛЯ ЗРЕНИЯ КАМЕРЫ
                    POI = null;
                    //В СЛ КАДРЕ
                    return;
                }
            }
        }
        //ОГРАНИЧИТЬ X И Y МИНИМАЛЬНЫМИ ЗНАЧЕНИЯМИ
        /*
         * Функция Mathf .Мах() выбирает максимальное
           из двух полученных значений. В момент выстрела снаряд имеет отрицательную
           координату X, поэтому Mathf .Мах() гарантирует, что камера никогда не сместится
           левее X = 0 в отрицательную область. Аналогично, второй вызов Mathf .Мах() не позволит
           камере опуститься ниже плоскости Y = 0, когда координата Y снаряда станет
           отрицательной.*/
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //ОПРЕДЕЛИТЬ ТОЧКУ МЕЖДУ ТЕКУЩИМ МЕСТОПОЛОЖЕНИЕМ КАМЕРЫ И DESTINATION
        /*
         * Метод Vector3. Lerp () выполняет интерполяцию между двумя точками, возвращая
        взвешенное среднее. Если полю easing присвоить 0, Lerp() вернет первую
        точку (transform, position); если полю easing присвоить 1, Lerp() вернет вторую
        точку (destination). Если полю easing присвоить любое другое значение
        в диапазоне между 0 и 1, Lerp() вернет точку, находящуюся между заданными
        (при значении 0,5 в поле easing будет возвращена точка, лежащая точно посередине).
        Определение easing = 0.05f означает, что в каждом вызове FixedUpdate
        (то есть при каждом обновлении, выполняемом физическим движком, которое
        происходит с частотой 50 раз в секунду) камера должна перемещаться примерно
        на 5% от расстояния между текущим ее местоположением и местоположением
        POI. Так как P0I постоянно перемещается, камера плавно следует за ним.
        */
        destination = Vector3.Lerp(transform.position, destination, easing);
        //ПРИНУДИТЕЛЬНО УСТАНОВИТЬ ЗНАЧЕНИЕ DESTINATION.Z == CAMZ,
        //ЧТОБЫ ОТОДВИНУТЬ КАМЕРУ ПОДАЛЬШЕ
        destination.z = camZ;
        //ПОМЕСТИТЬ КАМЕРУ В ПОЗИЦИЮ
        transform.position = destination;
        //ИЗМЕНИТЬ РАЗМЕР ORTHOGRAPHICSIZE КАМЕРЫ , ЧТОБЫ ЗЕМЛЯ ОСТАВАЛАСЬ В ПОЛЕ ЗРЕНИЯ
        Camera.main.orthographicSize = destination.y + 10;
    }
}
