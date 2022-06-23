using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    //Level olusturma
    public GameObject layoutRoom;
    public Color startColor, endColor, shopColor, gunRoomColor; //Baslangic ve bitis odalari rengi farkli olsun


    public int distanceToEnd;
    //Shop
    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;

    //Silah odasi
    public bool includeGunRoom;
    public int minDistanceToGunRoom, maxDistanceToGunRoom;

    //Baslangic odasi icin baslangic noktasi
    public Transform generatorPoint;

    //Move generator
    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f; // 18 birim x, 10 birim y ekseninde hareketler olcak

    //Odalar üst üste binmesin
    public LayerMask whatIsRoom;

    //Tracking generate room
    private GameObject endRoom, shopRoom, gunRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();


    //Serialazing
    public RoomPrefabs rooms;


    //Room outlines
    private List<GameObject> generatedOutlines = new List<GameObject>();

    //Adding center generation
    public RoomCenter centerStart, centerEnd, centerShop, centerGunRoom;
    public RoomCenter[] potentialCenters;

    

    void Start()
    {
        //Start room
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        //Siradaki oda nerede olacak kararýný veren yapi
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint(); //Hangi noktada oda olusacak ona karar verildi


        //Kaç oda olusacak onun dongusu
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            //Listeye tek tek odalari ekliyor
            layoutRoomObjects.Add(newRoom); 


            //tracking generate room
            if(i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;

                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1); //Ýlk obje sýfýr oldugu icin son elemaný siliyoruz

                endRoom = newRoom;
            }


            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();


            //Odalar üst üste binmesin
            while(Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }

        }

        //Shop
        if(includeShop)
        {
            int shopSelecter = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelecter];
            layoutRoomObjects.RemoveAt(shopSelecter);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        //Silah odasi
        if (includeGunRoom)
        {
            int grSelector = Random.Range(minDistanceToGunRoom, maxDistanceToGunRoom + 1);
            gunRoom = layoutRoomObjects[grSelector];
            layoutRoomObjects.RemoveAt(grSelector);
            gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;
        }

        //Create room outlines
        CreateRoomOutline(Vector3.zero); // Start room icin
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position); // End room
        //Shop
        if(includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }
        //Silah odasi
        if (includeGunRoom)
        {
            CreateRoomOutline(gunRoom.transform.position);
        }




        //Adding center generation
        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if(outline.transform.position == Vector3.zero) // baslangic odasi
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            //shop
            if(includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            //Silah odasi
            if(includeGunRoom)
            {
                if (outline.transform.position == gunRoom.transform.position)
                {
                    Instantiate(centerGunRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

            }


        }
    }

    void Update()
    {

#if UNITY_EDITOR // SADECE unity editorunde calisir
        //Reset yapmak için
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }


    //Move generator
    public void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
     
    }

    //Room Outline
    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);

        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);

        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);

        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);



        int directionCount = 0;
        if(roomAbove)
        {
            directionCount++;
        }

        if (roomBelow)
        {
            directionCount++;
        }

        if (roomLeft)
        {
            directionCount++;
        }

        if (roomRight)
        {
            directionCount++;
        }


        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!!");
                    break;

            case 1: //Tek cikisi olanlar icin

                if (roomAbove)
                {
                   generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }

                if(roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }

                if(roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }

                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }

                break;

            case 2: //Ýki cikisi olanlar icin

                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }

                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }

                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }

                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }

                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }

                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }


                break;

            case 3:

                if(roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }

                if (roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }

                if (roomBelow && roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }

                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }


                break;

            case 4:

                if (roomBelow && roomLeft && roomAbove &&roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }

                break;
        }

    }
}

[System.Serializable] //inspector kisminda görebilmek icin
public class RoomPrefabs
{
    //Serialazing
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
