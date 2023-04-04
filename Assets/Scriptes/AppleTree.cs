using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    //ШАБЛОН ДЛЯ СОЗДАНИЯ ЯБОК
    public GameObject applePrefab;

    //СКОРОСТЬ ДВИЖЕНИЯ ЯДЛОНИ
    public float speed = 1f;

    //РАССТОЯНИЕ, НА КОТОРОМ ДОЛЖНО ИЗМЕНЯТЬСЯ НАПРАВЛЕНИЕ ДВИЖЕНИЯ ЯБЛОНИ
    public float leftAndRightEdge = 10f;

    //ВЕРОЯТНОСТЬ СЛУЧАЙНОГО ИЗМЕНЕНИЯ НАПРАВЛЕНИЯ ДВИЖЕНИЯ
    public float chanceToChangeDirections = 0.1f;

    //ЧАСТОТА СОЗДАНИЯ ЭКЗЕМПЛЯРОВ ЯБЛОК
    public float secondBetweenAppleDrops = 1f;
    private void Start()
    {
        /*СБРАСЫВАТЬ ЯБЛОКИ РАЗ В СЕКУНДУ
        Функция Invoke() вызывает функцию, заданную именем, через указанное
        число секунд.
        */
        Invoke("DropApple", 2f);
    }
    private void DropApple()
    {/*DropApple() — это наша функция. Она создает экземпляр Apple в точке, где
        находится AppleTree.
        */
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;//Местоположение этого нового игрового объекта apple устанавливается равным
                                                      //местоположению яблони AppleTree.
        Invoke("DropApple", secondBetweenAppleDrops);
    }
    private void Update()
    {
        /*ПРОСТОЕ ПЕРЕМЕЩЕНИЕ. 
        Эта строка определяет локальную переменную Vector3 pos для хранения
        текущей позиции яблони.
        */
        Vector3 pos = transform.position;
        /*Компонент х переменной pos увеличивается на произведение скорости speed
         и интервала времени Time.deltaTime(количество секунд, прошедших после
        отображения предыдущего кадра).Благодаря этому яблоня будет перемещаться
        c учетом реального времени, что очень важно при программировании
        игр(см.врезку «Привязка игр к реальному времени»).
        */
        pos.x += speed * Time.deltaTime;
        /*Измененное значение pos присваивается обратно свойству transform,
        position(что вызывает перемещение яблони AppleTree в новое местоположение).
        Если не выполнить присваивание pos свойству transform.position,
        яблоня не переместится.
        */
        transform.position = pos;
        //ИЗМЕНЕНИЕ НАПРОВЕНИЯ
        if (pos.x < -leftAndRightEdge)
        {
            /*Если величина pos. х оказалась слишком маленькой, переменной speed присваивается
             результат вызова Mathf .Abs(speed), который возвращает абсолютное
             положительное значение speed и тем самым гарантирует, что в следующем кадре
             начнется перемещение яблони вправо.
            */
            speed = Mathf.Abs(speed);//НАЧАТЬ ДВИЖЕНИЕ В ПРАВО
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);//ВЛЕВО
        }

    }
    private void FixedUpdate()
    {
        /*Random.value возвращает случайное число типа float между 0 и 1 (включая
      0 и 1 как возможные значения). Если случайное значение меньше
       chanceToChangeDirections...
      */
        if (Random.value < chanceToChangeDirections)
        {//изменить направление движения AppleTree можно, поменяв знак переменной
            speed *= -1;
        }
    }
}
