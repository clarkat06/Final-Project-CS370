using UnityEngine;

public class CounterCube : MonoBehaviour
{
    [SerializeField] Sprite Zero;
    [SerializeField] Sprite One;
    [SerializeField] Sprite Two;
    [SerializeField] Sprite Three;
    [SerializeField] Sprite Four;
    [SerializeField] Sprite Five;
    [SerializeField] Sprite Six;
    [SerializeField] Sprite Seven;
    [SerializeField] Sprite Eight;
    [SerializeField] Sprite Nine;

    [SerializeField] public SpriteRenderer Face000;
    [SerializeField] public SpriteRenderer Face090;
    [SerializeField] public SpriteRenderer Face180;
    [SerializeField] public SpriteRenderer Face270;

    // manually track what each face is set to.
    int f000;
    int f090;
    int f180;
    int f270;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Face000.sprite = Zero;
        f000 = 0;
        Face090.sprite = One;
        f090 = 1;
        Face180.sprite = Two;
        f180 = 2;
        Face270.sprite = Nine;
        f270 = 9;
    }

    // Update is called once per frame
    void Update()
    {
        float xRot = 0;
        if(transform.up.y > 0 && transform.up.z > 0) {
            xRot = 45;
        } else if (transform.up.y < 0 && transform.up.z > 0) {
            xRot = 135;
        } else if (transform.up.y < 0 && transform.up.z < 0) {
            xRot = 225;
        } else if (transform.up.y > 0 && transform.up.z < 0) {
            xRot = 315;
        }

        

        f000 = sprit2num(Face000.sprite);
        f090 = sprit2num(Face090.sprite);
        f180 = sprit2num(Face180.sprite);
        f270 = sprit2num(Face270.sprite);


        if (xRot > 0 && xRot < 90) {
            if (f180 != (f090 + 1) % 10) {
                f180 = (f090 + 1) % 10;
                Face180.sprite = num2sprit(f180);
            }
            if (f270 != (f000 + 9) % 10) {
                f270 = (f000 + 9) % 10;
                Face270.sprite = num2sprit(f270);
            }
        } else if (xRot > 90 && xRot < 180) {
            if (f270 != (f180 + 1) % 10) {
                f270 = (f180 + 1) % 10;
                Face270.sprite = num2sprit(f270);
            }
            if (f000 != (f090 + 9) % 10) {
                f000 = (f090 + 9) % 10;
                Face000.sprite = num2sprit(f000);
            }
        } else if (xRot > 180 && xRot < 270) {
            if (f000 != (f270 + 1) % 10) {
                f000 = (f270 + 1) % 10;
                Face000.sprite = num2sprit(f000);
            }
            if (f090 != (f180 + 9) % 10) {
                f090 = (f180 + 9) % 10;
                Face090.sprite = num2sprit(f090);
            }
        } else if (xRot > 270 && xRot < 360) {
            if (f090 != (f000 + 1) % 10) {
                f090 = (f000 + 1) % 10;
                Face090.sprite = num2sprit(f090);
            }
            if (f180 != (f270 + 9) % 10) {
                f180 = (f270 + 9) % 10;
                Face180.sprite = num2sprit(f180);
            }
        }
    }

    public int sprit2num(Sprite a) {
        if(a == Zero) {
            return 0;
        } else if (a == One) {
            return 1;
        } else if (a == Two) {
            return 2;
        } else if (a == Three) {
            return 3;
        } else if (a == Four) {
            return 4;
        } else if (a == Five) {
            return 5;
        } else if (a == Six) {
            return 6;
        } else if (a == Seven) {
            return 7;
        } else if (a == Eight) {
            return 8;
        } else {
            return 9;
        }
    }

    public Sprite num2sprit(int x) {
        switch(x) {
            case 0:
            return Zero;
            case 1:
            return One;
            case 2:
            return Two;
            case 3:
            return Three;
            case 4:
            return Four;
            case 5:
            return Five;
            case 6:
            return Six;
            case 7:
            return Seven;
            case 8:
            return Eight;
            default:
            return Nine;
        }
    }
    
    public void disappear(){
        transform.Rotate(new Vector3(0, 90, 0));
    }
    public void appear(){
        transform.Rotate(new Vector3(0, 0, 0));
    }

    public Transform transformPweaseOwO(){
        return transform;
    }

    public int displayedNumInt() {
        return (int) (displayedNum() + 0.5f);
    }

    public float displayedNum() {
        float uppy = Mathf.Abs(transform.up.y);
        float uppz = Mathf.Abs(transform.up.z);
        float helper = uppy + uppz;
        uppy = uppy / helper;
        uppz = uppz / helper;

        int f0 = f000;
        int f9 = f090;
        int f1 = f180;
        int f2 = f270;
        if (f0 == 0) {
            f0 = 10;
        } else if (f9 == 0) {
            f9 = 10;
        } else if (f1 == 0) {
            f1 = 10;
        } else if (f2 == 0) {
            f2 = 10;
        }


        if(transform.up.y >= 0 && transform.up.z >= 0) { // 45 (f0 & f90)
            if(f9 != 9 && f0 != 9){
                f9 = f9 % 10;
                f0 = f0 % 10;
            }
            return uppy * f0 + uppz * f9;
        } else if (transform.up.y < 0 && transform.up.z >= 0) { // 135 (f90 & f180)
            if(f9 != 9 && f1 != 9){
                f9 = f9 % 10;
                f1 = f1 % 10;
            }
            return uppy * f1 + uppz * f9;
        } else if (transform.up.y < 0 && transform.up.z < 0) { // 225 (f180 & f270)
            if(f1 != 9 && f2 != 9){
                f1 = f1 % 10;
                f2 = f2 % 10;
            }
            return uppy * f1 + uppz * f2;
        } else { // 315 (f270 & f0)
            if(f2 != 9 && f0 != 9){
                f2 = f2 % 10;
                f0 = f0 % 10;
            }
            return uppy * f0 + uppz * f2;
        }
    }
}
