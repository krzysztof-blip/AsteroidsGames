#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    //gracz (jego pozycja)
    Transform player;

    //prefab statycznej asteroidy
    public GameObject staticAsteroid;

    //czas od ostatio wygenerowanej asteoidy
    float timeSinceSpawn;

    //odleg³oœ w jakiej spawnuj¹ siê asteroidy
    public float spawnDistance = 10;

    //odleg³oœæ pomiêdzy asteroidami
    public float safeDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        //znajdz gracz i przypisz do zmiennej
        player = GameObject.FindWithTag("Player").transform;

        //zeruj czas
        timeSinceSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAsteroid(staticAsteroid);

        AsteroidCountControll();
    }

    void SpawnAsteroid(GameObject prefab)
    {
        //generyczna funkcja sluzaca do wylosowania wspolrzednych i umieszczenia
        //w tym miejscu asteroidy z prefaba

        //stworz losow¹ pozycjê na okrêgu (x,y)
        Vector2 randomCirclePosition = Random.insideUnitCircle.normalized;

        //losowa pozycja w odleg³oœci 10 jednostek od œrodka œwiata
        //mapujemy x->x, y->z, a y ustawiamy 0
        Vector3 randomPosition = new Vector3(randomCirclePosition.x, 0, randomCirclePosition.y) * spawnDistance;

        //na³ó¿ pozycjê gracza - teraz mamy pozycje 10 jednostek od gracza
        randomPosition += player.position;

        //sprawdz czy miejsce jest wolne
        //! oznacza "nie" czyli nie ma nic w promieniu jednostek od miejsca randomPosition
        if (!Physics.CheckSphere(randomPosition, safeDistance))
        {
            //stworz zmienn¹ asteroid, zespawnuj nowy asteroid korzystaj¹c z prefaba
            // w losowym miejscu, z rotacj¹ domyœln¹ (Quaternion.identity)
            GameObject asteroid = Instantiate(staticAsteroid, randomPosition, Quaternion.identity);
        }

    }
    void AsteroidCountControll()
    {
        //przygotuj tablicê wszystkich asteroidów na scenie
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        //przejdŸ pêtl¹ przez wyszystkie
        foreach (GameObject asteroid in asteroids)
        {
            //odleg³oœæ od gracza

            //wektor przesuniêcia miêdzy graczem a asteroid¹
            //(o ile musze przesun¹c gracza, ¿eby znalaz³ siê w miejscu asteroidy
            Vector3 delta = player.position - asteroid.transform.position;

            //magnitude to dugoœæ wektora = odleg³oœæ od gracza
            float distanceToPlayer = delta.magnitude;

            if (distanceToPlayer > 30)
            {
                Destroy(asteroid);
            }
        }
    }
}