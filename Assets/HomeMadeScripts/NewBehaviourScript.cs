﻿using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBehaviourScript : MonoBehaviour
{

    public List<int> eligibles = new List<int>();
    public List<int> cases = new List<int>();
    public List<int> revealed = new List<int>();

    public InventaireSlot Inventory1;
    public InventaireSlot Inventory2;
    public InventaireSlot Inventory3;
    public InventaireSlot Inventory4;
    public InventaireSlot Inventory5;
    public InventaireSlot Inventory6;
    public InventaireSlot Inventory7;
    public InventaireSlot Inventory8;
    public InventaireSlot Inventory9;

    public InventaireSlot InventoryHelmet;
    public InventaireSlot InventoryLeftHand;
    public InventaireSlot InventoryChest;
    public InventaireSlot InventoryRightHand;
    public InventaireSlot InventoryGreaves;

    public InventaireSlot InventoryTalisman1;
    public InventaireSlot InventoryTalisman2;
    public InventaireSlot InventoryTalisman3;


    public GameObject ShowCard;
    public SpriteRenderer sr;

    public GameObject CardStart;

    public GameObject Lifebar;
    public GameObject Hungerbar;
    public GameObject Moralbar;


    public GameObject CardT;
    public GameObject CardP;
    public GameObject CardR;

    public int nbreT = 0;
    public int nbreR = 0;
    public int nbreP = 0;
    public List<int> possibilities = new List<int>();

    public GameObject TCard1;
    public GameObject TCard2;
    public GameObject TCard3;

    public GameObject RCard1;
    public GameObject RCard2;
    public GameObject RCard3;

    public GameObject PCard1;
    public GameObject PCard2;
    public GameObject PCard3;
    public GameObject PCard4;
    public GameObject PCard5;

    public List<GameObject> TCardlist = new List<GameObject>();
    public List<GameObject> RCardlist = new List<GameObject>();
    public List<GameObject> PCardlist = new List<GameObject>();

    public GameObject floor;

    public GameObject camera;
    public GameObject camrotator;

    public GameObject inventory;

    public GameObject go;
    public GameObject bossCard;
    public GameObject token;

    public bool debut = true;
    private bool moving = false;
    public bool canMove = true;


    public int ax = 0;
    public int az = 0;

    public int hitx = 0;
    public int hitz = 0;

    public int floorlvl = 6;

    private int decalagex = 4;
    private int decalagez = 3;


    public bool holding = false;
    public int holdid = 0;
    public InventaireSlot chosen;

    // Use this for initialization
    void Start()
    {
        chosen = Inventory1;
        SpriteRenderer sr = ShowCard.GetComponent<SpriteRenderer>();
        iniLvl(6);
        

    }

    public void iniLvl(int nbreCases)
    {
        System.Random rnd = new System.Random();

        int nbreCards = nbreCases - 1;

        TCardlist = new List<GameObject> { TCard1, TCard2, TCard3 };
        RCardlist = new List<GameObject> { RCard1, RCard2, RCard3 };
        PCardlist = new List<GameObject> { PCard1, PCard2, PCard3, PCard4, PCard5 };

        // set nbreT et nbre R

        nbreT = rnd.Next(1, nbreCases / 4 + 1);
        nbreR = rnd.Next(1, nbreCases / 4 + 1);
        nbreP = nbreCases - nbreR - nbreT - 1;

        if (nbreT > 0)
            possibilities.Add(1);
        if (nbreR > 0)
            possibilities.Add(2);
        if (nbreP > 0)
            possibilities.Add(3);


        //fin set nbre T et set nbreR




        revealed.Add(1010);
        int bossPos = CreateLvl(nbreCases);
        Instantiate(CardStart, new Vector3(10 * decalagex, 1, 10 * decalagez), Quaternion.identity, floor.transform);


        //reveal initiale

        revealIni(nbreT, nbreR, nbreP);

       

        //pose des cartes face cachée

        foreach (int i in cases)
        {
            Instantiate(go, new Vector3((i / 100) * decalagex, 1, (i % 100) * decalagez), Quaternion.identity, floor.transform);
        }
        Instantiate(bossCard, new Vector3((bossPos / 100) * decalagex, 1, (bossPos % 100) * decalagez), Quaternion.identity, floor.transform);
        revealed.Add(bossPos);


        cameraPos(cases);

        token.transform.position = (new Vector3(10 * decalagex, 1, 10 * decalagez));

        //fin pose des cartes face cachée


    }

    public bool isAdj(GameObject card1, RaycastHit card2)
    {
        bool a = (card1.transform.position.x == card2.transform.position.x + decalagex) || (card1.transform.position.x == card2.transform.position.x - decalagex);
        bool b = (card1.transform.position.z == card2.transform.position.z + decalagez) || (card1.transform.position.z == card2.transform.position.z - decalagez);
        return (a ^ b) && Math.Abs((card1.transform.position.x - card2.transform.position.x)) < decalagex + 1 && Math.Abs((card1.transform.position.z - card2.transform.position.z)) < decalagez + 1;
    }

    public void cameraPos(List<int> cases)
    {
        int sumx = 1;
        int sumz = 1;

        foreach (int i in cases)
        {
            sumx += i / 100;
            sumz += i % 100;


           
        }
        int L = cases.Count;

        camera.transform.position = new Vector3(sumx * decalagex / L + 16, 11, sumz * decalagez / L);




    }


    public static bool isIn(int e, List<int> list)
    {
        bool ret = false;
        foreach (int i in list)
        {
            if (e == i)
            {
                ret = true;
            }
        }

        return ret;
    }


    public int CreateLvl(int nbreCases)
    {
        int pos = 1010;
        int L = 0;
        int tirage = 0;
        System.Random rnd = new System.Random();

        cases.Add(1010);

        while (nbreCases > 0)
        {


            if (!isIn(pos - 1, cases))
            {
                eligibles.Add(pos - 1);
            }
            if (!isIn(pos + 1, cases))
            {
                eligibles.Add(pos + 1);
            }
            if (!isIn(pos + 100, cases))
            {
                eligibles.Add(pos + 100);
            }
            if (!isIn(pos - 100, cases))
            {
                eligibles.Add(pos - 100);
            }

            L = eligibles.Count;


            tirage = rnd.Next(L - 1);

            if (!isIn(eligibles[tirage], cases))
            {

                pos = eligibles[tirage];
                if (nbreCases > 1)
                {
                    cases.Add(eligibles[tirage]);
                }
                nbreCases--;
            }



        }
        cases.Remove(1010);
        return pos;
    }

    public void revealIni(int T, int R, int P)
    {
        int casesCount = cases.Count;
        int nbreCasesAReveal = floorlvl / 4;
        int elu;
        char tirage;

        int tirageCases = 0;

        int PossCount = possibilities.Count;

        System.Random rnd = new System.Random();
        GameObject TRP = CardStart;

        while (nbreCasesAReveal > 0 && PossCount > 0)
        {
            tirage = TRPchoose();
            tirageCases = rnd.Next(casesCount);
            elu = cases[tirageCases];

            revealed.Add(elu);
            cases.Remove(elu);
            casesCount--;
            nbreCasesAReveal--;

            switch (tirage)
            {
                case ('T'):
                    Instantiate(CardT, new Vector3(elu / 100 * decalagex, 1, elu % 100 * decalagez), Quaternion.identity, floor.transform);
                    PossCount--;
                    break;
                case ('R'):
                    Instantiate(CardR, new Vector3(elu / 100 * decalagex, 1, elu % 100 * decalagez), Quaternion.identity, floor.transform);
                    PossCount--;
                    break;
                case ('P'):
                    Instantiate(CardP, new Vector3(elu / 100 * decalagex, 1, elu % 100 * decalagez), Quaternion.identity, floor.transform);
                    PossCount--;
                    break;



            }

            //   Instantiate(TRP, new Vector3(elu / 100 * 4, 1, elu % 100 * 2), Quaternion.identity, floor.transform);


        }

    }


    public char TRPchoose()
    {
        System.Random rnd = new System.Random();


        int tirage = rnd.Next(possibilities.Count);

        switch (possibilities[tirage])
        {
            case (1):
                nbreT--;
                if (nbreT < 1)
                    possibilities.Remove(1);
                return 'T';
            case (2):

                nbreR--;
                if (nbreR < 1)
                    possibilities.Remove(2);
                return 'R';
            case (3):

                nbreP--;
                if (nbreP < 1)
                    possibilities.Remove(3);
                return 'P';


        }
        return ' ';

    }


    public bool reveal(int x, int z)
    {
        int tirage = 0;
        char TRPtirage = ' ';
        GameObject newcard = CardStart;

        if (isIn(x * 100 + z, revealed))
        {
            return false;
        }
        else
        {
            System.Random rnd = new System.Random();


            tirage = rnd.Next(1, 3);
            TRPtirage = TRPchoose();



            switch (TRPtirage)
            {
                case 'T':

                    //reveal T
                    tirage = rnd.Next(TCardlist.Count);

                    newcard = TCardlist[tirage];

                    TCardlist.Remove(newcard);
                    break;

                case 'R':
                    //reveal R
                    tirage = rnd.Next(RCardlist.Count);

                    newcard = RCardlist[tirage];

                    RCardlist.Remove(newcard);


                    break;

                case 'P':
                    //reveal P
                    tirage = rnd.Next(PCardlist.Count);

                    newcard = PCardlist[tirage];

                    PCardlist.Remove(newcard);
                    break;
            }


        }
        Instantiate(newcard, new Vector3(x * decalagex, 1, z * decalagez), Quaternion.identity, floor.transform);

        revealed.Add((int)(x * 100F + z));
        return true;
    }




    // Update is called once per frame
    void Update()
    {

        if (moving)
        {
            token.transform.Translate(new Vector3(ax/decalagex, 0, az/decalagez));
            if (token.transform.position.x == hitx && token.transform.position.z == hitz)
            {
                moving = false;
            }

        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ray CheckBelowHit = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;



        if (Input.GetMouseButtonDown(0))
        {


            if (Physics.Raycast(ray, out hit, 100.0F))
            {
                if (isAdj(token, hit) && canMove)
                {
                    hitx = (int)hit.transform.position.x;
                    hitz = (int)hit.transform.position.z;

                    ax = hitx - (int)token.transform.position.x;
                    az = hitz - (int)token.transform.position.z;

                    moving = true;

                    jauges faim = Hungerbar.GetComponent<jauges>();
                    faim.change(-1);
                    

                //    canMove = false; à désactiver pdt tests.

                //    token.transform.position = hit.transform.position;
                    if (!isIn((int)(hit.transform.position.x) * 100 / decalagex + (int)(hit.transform.position.z) / decalagez, revealed))
                    {

                        float x = hit.transform.position.x / decalagex;
                        float z = hit.transform.position.z / decalagez;
                        bool a = reveal((int)x, (int)z);

                        if (a)
                        {
                            hit.collider.gameObject.SetActive(false);
                        }
                    }


                }

                //INVENTORY INTERACTIONS
                {

                    if (hit.transform.tag.Length > 9 && hit.transform.tag.Substring(0, 9) == "Inventory")
                    {
                        int prefixe = 0;
                        prefixe = int.Parse(hit.transform.tag.Substring(9, hit.transform.tag.Length - 9));



                        switch (prefixe)
                        {
                            case 1:

                                chosen = Inventory1;

                                break;
                            case 2:
                                chosen = Inventory2;

                                break;
                            case 3:
                                chosen = Inventory3;
                                break;

                            case 4:
                                chosen = Inventory4;
                                break;

                            case 5:
                                chosen = Inventory5;
                                break;

                            case 6:
                                chosen = Inventory6;
                                break;
                            case 7:
                                chosen = Inventory7;
                                break;
                            case 8:
                                chosen = Inventory8;
                                break;
                            case 9:
                                chosen = Inventory9;
                                break;
                            case 10:
                                chosen = InventoryHelmet;
                                break;
                            case 11:
                                chosen = InventoryLeftHand;
                                break;
                            case 12:
                                chosen = InventoryChest;
                                break;
                            case 13:
                                chosen = InventoryRightHand;
                                break;
                            case 14:
                                chosen = InventoryGreaves;
                                break;
                            case 15:
                                chosen = InventoryTalisman1;
                                break;
                            case 16:
                                chosen = InventoryTalisman2;
                                break;
                            case 17:
                                chosen = InventoryTalisman3;
                                break;

                        }




                        if (chosen.id == 0)
                        {

                           // if (holdid / 100 < 1 && prefixe < 10)
                           // {
                                chosen.setId(holdid);
                                holdid = 0;
                                canMove = true;
                           // }
                        }
                        else if (holdid == 0)
                        {
                            holdid = chosen.id;
                            chosen.setId(0);
                            canMove = false;
                        }
                    }             
                   else if (holdid != 0)
                {
                    chosen.setId(holdid);
                    holdid = 0;
                    canMove = true;
                }
            }


                }
            }

        

        if (Input.GetKeyDown(KeyCode.A))
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in floor.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));

            eligibles.Clear();
            cases.Clear();
            revealed.Clear();
            possibilities.Clear();

            if (floorlvl < 14)
                floorlvl += 2;



            iniLvl(floorlvl);
        }




    }

}

