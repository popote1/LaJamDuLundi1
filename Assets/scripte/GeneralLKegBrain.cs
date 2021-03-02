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
        AvantGauche.CanMove = !AvantDroite.IsMoving;
        AvantDroite.CanMove = !AvantGauche.IsMoving;
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
        BackGauche.CanMove = !BackDroite.IsMoving;
        BackDroite.CanMove = !BackGauche.IsMoving;

    }
}
