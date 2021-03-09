using UnityEngine;

public class BodyBalance : MonoBehaviour
{
    public bool IsUsDebug;
    public bool IsKeyBordControlled;
    public float MoveSpeed = 0.1f;
    public float DistanceToGround = 5;
    
    public Transform RaycastAvantGauche;
    public Transform RaycastAvantDroit;
    public Transform RaycastArriereGauche;
    public Transform RaycastArriereDroit;

    private Vector3 ContactAvantGauche;
    private Vector3 ContactAvantDroit;
    private Vector3 ContactArriereGauche;
    private Vector3 ContactArriereDroit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(RaycastAvantGauche.position, RaycastAvantGauche.forward, out hit))  ContactAvantGauche = hit.point;
        if (Physics.Raycast(RaycastAvantDroit.position, RaycastAvantDroit.forward, out hit)) ContactAvantDroit = hit.point;
        if (Physics.Raycast(RaycastArriereGauche.position, RaycastArriereGauche.forward, out hit)) ContactArriereGauche = hit.point;
        if (Physics.Raycast(RaycastArriereDroit.position, RaycastArriereDroit.forward, out hit)) ContactArriereDroit = hit.point;
        

        Vector3 avant = ContactAvantGauche - ContactAvantDroit;
        Vector3 arriere = ContactArriereGauche - ContactArriereDroit;
        Vector3 gauche = ContactAvantGauche - ContactArriereGauche;
        Vector3 droit = ContactAvantDroit - ContactArriereDroit;

        Vector3 longeur = arriere + avant;
        Vector3 largeur = gauche + droit;
        Vector3 normal = Vector3.Cross(longeur, largeur).normalized;
        Vector3 moyen = (ContactArriereDroit + ContactArriereGauche + ContactAvantDroit + ContactAvantGauche) / 4;

        //transform.position = Vector3.Lerp(transform.position , Vector3.Project(moyen , normal*DistanceToGround), 0.2f);
        var pos = transform.position;
        transform.position = new Vector3(pos.x , Mathf.Lerp(pos.y,moyen.y + DistanceToGround, 0.1f), pos.z);
        
        if (IsUsDebug)
        {
            Debug.DrawLine(RaycastAvantGauche.position, ContactAvantGauche, Color.red);
            Debug.DrawLine(RaycastAvantDroit.position, ContactAvantDroit, Color.red);
            Debug.DrawLine(RaycastArriereGauche.position, ContactArriereGauche, Color.red);
            Debug.DrawLine(RaycastArriereDroit.position, ContactArriereDroit, Color.red);
            Debug.DrawLine(transform.position, moyen, Color.magenta);
        }

        transform.up =Vector3.Lerp(transform.up, normal , 0.01f);

        if (IsKeyBordControlled)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Vertical") * MoveSpeed ,0,- Input.GetAxis("Horizontal") * MoveSpeed);
            transform.position += movement * Time.deltaTime;
            
            Vector3 rotation = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
            {
                rotation += Vector3.up;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                rotation += Vector3.down;
            }

            transform.eulerAngles += rotation;
        }

        
    }
}
