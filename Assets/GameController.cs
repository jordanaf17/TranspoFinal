using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cubePrefab;
    Vector3 cubePosition;
    public static GameObject activeCube;
    int startI, startM;
    GameObject[,] grid;
    int gridI, gridM;
    bool airplaneActive;
    int airplaneI, airplaneM;
    bool destination;
    int destinationI, destinationM;
    float turnLength, takeTurn;
    int cargo;
    int cargoMax;
    int cargoGain;
    int depotI, depotM;
    int score;
    int moveM, moveI;
    private int myI;

    // i can't read x & y (they look too similar) so i'm using i & m instead!
    // i = width, m = height

    void Start()
    {
        turnLength = 1.5f;
        takeTurn = turnLength;

        cargo = 0;
        cargoMax = 90;
        cargoGain = 10;

        gridI = 16;
        gridM = 9;
        grid = new GameObject[gridI, gridM];

        for (int m = 0; m < gridM; m++)
        {
            for (int i = 0; i < gridI; i++)
            {
                cubePosition = new Vector3(i * 2, m * 2, 0);
                grid[i, m] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                grid[i, m].GetComponent<CubeController>().myI = i;
                grid[i, m].GetComponent<CubeController>().myM = m;
            }
        }
        // upper left
        startI = 0;
        startM = 8;
        airplaneI = startI;
        airplaneM = startM;
        grid[airplaneI, airplaneM].GetComponent<Renderer>().material.color = Color.blue;

        // lower right
        depotI = 15;
        depotM = 0;
        grid[depotI, depotM].GetComponent<Renderer>().material.color = Color.black;

        moveI = 0;
        moveM = 0;

        airplaneActive = false;
        destination = false;

    }


    // all my methods!

    public void ProcessClick(GameObject cubeClicked, int i, int m)
    {
        if (i == airplaneI && m == airplaneM)
        {
            if (!airplaneActive)
            {
                airplaneActive = true;
                cubeClicked.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (airplaneActive)
            {
                airplaneActive = false;
                cubeClicked.GetComponent<Renderer>().material.color = Color.blue;
                destinationI = airplaneI;
                destinationM = airplaneM;
            }

        }

        if (i != airplaneI || m != airplaneM)
        {
            if (airplaneActive)
            {
                destinationI = i;
                destinationM = m;
                destination = true;
            }
            else if (!airplaneActive)
            {
                destination = false;
            }
        }
    }

    void LoadCargo()
    {
        if (airplaneI == startI && airplaneM == startM && cargo < cargoMax)
        {
            cargo += cargoGain;
        }
        if (cargo > cargoMax)
        {
            cargo = cargoMax;
        }
    }

    void DeliverCargo()
    {
        if (airplaneI == depotI && airplaneM == depotM)
        {
            score += cargo;
            cargo = 0;
        }
    }

    void MoveAirplane()
    {

        if (airplaneActive && destination)
        {
            if (airplaneI == depotI && airplaneM == depotM)
            {
                grid[depotI, depotM].GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                grid[airplaneI, airplaneM].GetComponent<Renderer>().material.color = Color.white;
            }

            moveI = 0;
            moveM = 0;

            if (airplaneI != destinationI)
            {
                if (airplaneI < destinationI)
                {
                    moveI = +1;
                }
                if (airplaneI > destinationI)
                {
                    moveI = -1;
                }
            }
            airplaneI += moveI;

            if (airplaneM != destinationM)
            {
                if (airplaneM < destinationM)
                {
                    moveM = +1;
                }
                if (airplaneM > destinationM)
                {
                    moveM = -1;
                }
            }
            airplaneM += moveM;

        if (airplaneI == destinationI && airplaneM == destinationM)
        {
            moveI = 0;
            moveM = 0;
                destination = false;
        }
        grid[airplaneI, airplaneM].GetComponent<Renderer>().material.color = Color.red;
    }
}

        void Update()
        {
            if (Time.time > takeTurn)
            {
                MoveAirplane();
                LoadCargo();
                DeliverCargo();
                print("Cargo:" + cargo + "Score:" + score);

                takeTurn += turnLength;
            }
        }
    }
