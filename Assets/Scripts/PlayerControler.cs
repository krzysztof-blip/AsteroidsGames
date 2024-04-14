using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float rotationspeed = 25f;
    public float flySpeed = 5f;
    GameObject levelManagerObject;
    //stan os�on w procentach (1=100%)
    float shieldCapacity = 1;
    // Start is called before the first frame update
    void Start()
    {
        levelManagerObject = GameObject.Find("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        //dodaj do wsp�rz�dnych warto�� x=1, y=0, z=0 pomno�one przez czas mierzony w sekundach od ostatniej klatki 
        //transform.position += new Vector3(1, 0, 0) * Time.deltaTime;

        //prezentacja dzia�ania wyg�adzonego sterowania (emulacja joysticka)
        //Debug.Log(Input.GetAxis("Vertical"));

        //sterowanie pr�dko�ci�
        //stw�rz nowy wektor przesuni�dia o wartosci o 1 do przodu
        Vector3 movement = transform.forward;
        //pomn� go przez czas od ostatniej klatki
        movement *= Time.deltaTime;
        //pomn� go przez "wychylenie" joysticka
        movement *= Input.GetAxis("Vertical");
        movement *= flySpeed;
        //dodaj ruch do obiektu
        transform.position += movement;

        Rigidbody rb = GetComponent<Rigidbody>();
        //dodaj si�e - do przodu statku w trybie zmiany pr�dko�ci
        rb.AddForce(movement, ForceMode.VelocityChange);


        //obr�t
        //modyfikuj o� "Y" obiektu Player
        Vector3 rotation = Vector3.up;
        //pomn� go przez czas od ostatniej klatki
        rotation *= Time.deltaTime;
        //przemn� przez klawiatur�
        rotation *= Input.GetAxis("Horizontal");
        //pomn� przez pr�dko�� obrotu
        rotation *= rotationspeed;
        //dodaj obr�t do obiektu
        //nie mo�emy u�y� += poniewa� unity u�ywa Quaternion�w do zapisu rotacji
        transform.Rotate(rotation);
    }
    private void UpdateUI()
    {
        //metoda wykonuje wszystko zwi�zane z aktualizacj� interfejsu u�ytkownika

        //wyciagnij z menadzera poziomu pozycje wyjscia
        Vector3 target = levelManagerObject.GetComponent<LevelManager>().exitPosition;
        //obroc znacznik w strone wyjscia
        //zmien ilosc procentwo widoczna w interfejsie
        //TODO: poprawi� wy�wietlanie stanu os�on!
        TextMeshProUGUI shieldText =
            GameObject.Find("Canvas").transform.Find("ShieldCapacityText").GetComponent<TextMeshProUGUI>();
        shieldText.text = " Shield: " + (shieldCapacity * 100).ToString() + "%";

        //sprawdzamy czy poziom si� zako�czy� i czy musimy wy�wietli� ekran ko�cowy
        if (levelManagerObject.GetComponent<LevelManager>().levelComplete)
        {
            //znajdz canvas (interfejs), znajdz w nim ekran konca poziomu i go w��cz
            GameObject.Find("Canvas").transform.Find("LevelCompleteScreen").gameObject.SetActive(true);
        }
        //sprawdzamy czy poziom si� zako�czy� i czy musimy wy�wietli� ekran ko�cowy
        if (levelManagerObject.GetComponent<LevelManager>().levelFailed)
        {
            //znajdz canvas (interfejs), znajdz w nim ekran konca poziomu i go w��cz
            GameObject.Find("Canvas").transform.Find("GameOverScreen").gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //uruchamia si� automatycznie je�li zetkniemy sie z innym coliderem

        //sprawdz czy dotkn�li�my asteroidy
        if (collision.collider.transform.CompareTag("Asteroid"))
        {
            //transform asteroidy
            Transform asteroid = collision.collider.transform;
            //policz wektor wed�ug kt�rego odepchniemy asteroide
            Vector3 shieldForce = asteroid.position - transform.position;
            //popchnij asteroide
            asteroid.GetComponent<Rigidbody>().AddForce(shieldForce * 5, ForceMode.Impulse);
            shieldCapacity -= 0.25f;
            if (shieldCapacity <= 0)
            {
                //poinformuj level manager, �e gra si� sko�czy�a bo nie mamy os�on
                levelManagerObject.GetComponent<LevelManager>().OnFailure();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //je�eli dotkniemy znacnzika ko�ca poziomu to ustaw w levelmanager flag�,
        //�e poziom jest uko�czony
        if (other.transform.CompareTag("LevelExit"))
        {
            //wywo�aj dla LevelManager metod� zako�czenia poziomu
            levelManagerObject.GetComponent<LevelManager>().OnSuccess();
        }
    }
}
