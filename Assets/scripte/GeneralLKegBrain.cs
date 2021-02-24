using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralLKegBrain : MonoBehaviour
{
    public LegController AvantGauche;
    public LegController AvantDroite;
    public LegController MidGauche;
    public LegController MidDroite;
    public LegController BackGauche;
    public LegController BackDroite;


    private void Update()
    {
        if (AvantDroite.IsMoving) AvantGauche.CanMove = false;else AvantGauche.CanMove =true;
        if (AvantGauche.IsMoving) AvantDroite.CanMove = false;else AvantDroite.CanMove = true;
        if (MidDroite.IsMoving)
        {
            MidGauche.CanMove = false;
            AvantDroite.CanMove = false;
            BackDroite.CanMove = false;
        }
        else
        {
            MidGauche.CanMove = true;
            AvantDroite.CanMove = true;
            BackDroite.CanMove = true;
            
            
        }

        if (MidGauche.IsMoving)
        {
            MidDroite.CanMove = false;
            BackGauche.CanMove = false;
            AvantGauche.CanMove = false;
        }
        else
        {
            MidDroite.CanMove = true;
            BackGauche.CanMove = true;
            AvantGauche.CanMove = true;
            
        }
        if (BackDroite.IsMoving) BackGauche.CanMove = false;else BackGauche.CanMove = true;
        if (BackGauche.IsMoving) BackDroite.CanMove = false;else BackDroite.CanMove = true;

    }
}
