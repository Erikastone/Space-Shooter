using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;//ндхмнвйю
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
    private void Awake()
    {
        S = this;//сярюмнбхрэ яяшйс мю наэейр ндхмнвйс
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }
    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //еякх онке _онх яндепфхр деиярбхрекэмсч яяшкйс,
                //яапняхрэ бяе нярюкэмше оюпюлерпш б хяундмне янярнъмхе
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        //бшгшбюеряъ дкъ днаюбкемхъ рнвйх б кхмхх
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //еякх рнвйю меднярюрнвмн дюкейн нр опедшдсыеи, опнярн бширх
            return;
        }
        if (points.Count == 0)//еякх щрн рнвйю гюосяйю
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            //дкъ нопедекемхъ днаюбхрэ днонкмхрекэмши тпюцлемр кхмхх,
            //врнаш онлнвэ ксвье опхжекхрэяъ б асдсыел
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //сярюмнбхрэ оепбше дбе рнвйх
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else
        {
            //онякеднбюрекэмнярэ днаюбкемхъ рнвйх
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    //бнгбпюыюел леярнонкнфемхе онякедмеи днаюбкеммни рнвйх
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                //еякх рнвей мер, бепмсрэ бейрнп3.гепн
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    private void FixedUpdate()
    {
        if (poi == null)
        {
            //еякх ябниярбн онх  яндепфхр осярне гмювемхе , мюирх хмрепеясчыхи наэейр
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;//бширх еякх ме мюидем
                }
            }
            else
            {
                return;
            }
        }
        //еякх хмрепеясчыхи наэейр мюидем
        //оношрюрэяъ днаюбхрэ рнвйс я ецн йннпдхмюрюлх б йюфднл fIXEDuPDATE
        AddPoint();
        if (FollowCam.POI == null)
        {
            //еякх fOLLOWcAM.POI яндепфхр NULL, гюохяюрэ NULL б poi
            poi = null;
        }
    }
}
