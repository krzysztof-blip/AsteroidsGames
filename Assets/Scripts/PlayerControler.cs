using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float rotationspeed = 25f;
    public float flySpeed = 5f;
    GameObject levelManagerObject;
    //stan os³on w procentach (1=100%)
    float shieldCapacity = 1;
    // Start is called before the first frame update
    void Start()
    {
        levelManagerObject = GameObject.Find("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        //dodaj do wspó³rzêdnych wartoœæ x=1, y=0, z=0 pomno¿one przez czas mierzony w sekundach od ostatniej klatki 
        //transform.position += new Vector3(1, 0, 0) * Time.deltaTime;

        //prezentacja dzia³ania wyg³adzonego sterowania (emulacja joysticka)
        //Debug.Log(Input.GetAxis("Vertical"));

        //sterowanie prêdkoœci¹
        //stwórz nowy wektor przesuniêdia o wartosci o 1 do przodu
        Vector3 movement = transform.forward;
        //pomnó¿ go przez czas od ostatniej klatki
        movement *= Time.deltaTime;
        //pomnó¿ go przez "wychylenie" joysticka
        movement *= Input.GetAxis("Vertical");
        movement *= flySpeed;
        //dodaj ruch do obiektu
        transform.position += movement;

        Rigidbody rb = GetComponent<Rigidbody>();
        //dodaj si³e - do przodu statku w trybie zmiany prêdkoœci
        rb.AddForce(movement, ForceMode.VelocityChange);


        //obrót
        //modyfikuj oœ "Y" obiektu Player
        Vector3 rotation = Vector3.up;
        //pomnó¿ go przez czas od ostatniej klatki
        rotation *= Time.deltaTime;
        //przemnó¿ przez klawiaturê
        rotation *= Input.GetAxis("Horizontal");
        //pomnó¿ przez prêdkoœæ obrotu
        rotation *= rotationspeed;
        //dodaj obrót do obiektu
        //nie mo¿emy u¿yæ += poniewa¿ unity u¿ywa Quaternionów do zapisu rotacji
        transform.Rotate(rotation);
    }
    private void UpdateUI()
    {
        //metoda wykonuje wszystko zwi¹zane z aktualizacj¹ interfejsu u¿ytkownika

        //wyciagnij z menadzera poziomu pozycje wyjscia
        Vector3 target = levelManagerObject.GetComponent<LevelManager>().exitPosition;
        //obroc znacznik w strone wyjscia
        //zmien ilosc procentwo widoczna w interfejsie
        //TODO: poprawiæ wyœwietlanie stanu os³on!
        TextMeshProUGUI shieldText =
            GameObject.Find("Canvas").transform.Find("ShieldCapacityText").GetComponent<TextMeshProUGUI>();
        shieldText.text = " Shield: " + (shieldCapacity * 100).ToString() + "%";

        //sprawdzamy czy poziom siê zakoñczy³ i czy musimy wyœwietliæ ekran koñcowy
        if (levelManagerObject.GetComponent<LevelManager>().levelComplete)
        {
            //znajdz canvas (interfejs), znajdz w nim ekran konca poziomu i go w³¹cz
            GameObject.Find("Canvas").transform.Find("LevelCompleteScreen").gameObject.SetActive(true);
        }
        //sprawdzamy czy poziom siê zakoñczy³ i czy musimy wyœwietliæ ekran koñcowy
        if (levelManagerObject.GetComponent<LevelManager>().levelFailed)
        {
            //znajdz canvas (interfejs), znajdz w nim ekran konca poziomu i go w³¹cz
            GameObject.Find("Canvas").transform.Find("GameOverScreen").gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //uruchamia siê automatycznie jeœli zetkniemy sie z innym coliderem

        //sprawdz czy dotknêliœmy asteroidy
        if (collision.collider.transform.CompareTag("Asteroid"))
        {
            //transform asteroidy
            Transform asteroid = collision.collider.transform;
            //policz wektor wed³ug którego odepchniemy asteroide
            Vector3 shieldForce = asteroid.position - transform.position;
            //popchnij asteroide
            asteroid.GetComponent<Rigidbody>().AddForce(shieldForce * 5, ForceMode.Impulse);
            shieldCapacity -= 0.25f;
            if (shieldCapacity <= 0)
            {
                //poinformuj level manager, ¿e gra siê skoñczy³a bo nie mamy os³on
                levelManagerObject.GetComponent<LevelManager>().OnFailure();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //je¿eli dotkniemy znacnzika koñca poziomu to ustaw w levelmanager flagê,
        //¿e poziom jest ukoñczony
        if (other.transform.CompareTag("LevelExit"))
        {
            //wywo³aj dla LevelManager metodê zakoñczenia poziomu
            levelManagerObject.GetComponent<LevelManager>().OnSuccess();
        }
    }
}
