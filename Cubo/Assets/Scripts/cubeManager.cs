using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class cubeManager : MonoBehaviour
{
    [SerializeField] private AudioClip MusicClip;

    [SerializeField] private AudioSource MusicSource;
    // Start is called before the first frame update
    [SerializeField] private GameObject CubePiece;
    private Transform CubeTransform;

    private List<GameObject>
        AllPieces =
            new List<GameObject>(); //lista de gameObjects para guardar todos los cubosque conforman n el cubo grande

    private GameObject CubeCenter;
    private bool canRotate = true;
    private bool canShuffle = true;

    [SerializeField] private KeyCode upRotation,
        downRotation,
        leftRotation,
        rightRotation,
        backRotation,
        frontRotation,
        resetCube, shuffleCube;

    [SerializeField] private Camera _camera;

    private Transform cubeTransf;

    Vector3[] RotateVectors = { 
     new Vector3(0, 1, 0), new Vector3(0, -1, 0), 
    new Vector3(0, 0, -1),new Vector3(0, 0, 1), 
    new Vector3(1, 0, 0), new Vector3(-1, 0, 0)
    

};
    //[SerializeField] private AudioClip MusicClip;

   // [SerializeField] private AudioSource MusicSource;
    //para encontrar todas las piezas de arriba
    private List<GameObject> UpPieces
    {
        //asigna las x where Mathf.Round(x.transform.localPosition.y) == 0
        // si entendi bien es como una query
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0); }
    }
    
    // para encontrar todas las piezas de abajo
    private List<GameObject> DownPieces
    {
       
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2); }
    }
    
    // para encontrar todas las piezas de la izqda
    private List<GameObject> LeftPieces
    {
       
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0); }
    }
    
    // para encontrar todas las piezas de la drcha
    private List<GameObject> RightPieces
    {
       
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2); }
    }
    
    // para encontrar todas las piezas del frente
    private List<GameObject> FrontPieces
    {
        
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0); }
    }
    
    // para encontrar todas las piezas de atras
    private List<GameObject> BackPieces
    {
        
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2); }
    }
    
    
    private List<GameObject> UpHorizontalPieces
    {
       
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1); }
    }
    private List<GameObject> UpVerticalPieces
    {
       
        get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1); }
    }
    private List<GameObject> FrontHorizontalPieces
         {
            
             get { return AllPieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1); }
         }
   
    void Start()
    {
        cubeTransf = transform;
        CreateCube();//funcion que genera el cubo 3x3 a partir del CubePiece
        MusicSource.clip = MusicClip;
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
            CheckInput();
    }

   

    private void CreateCube()
    {
        //destroy los GameObjects que estan en AllPieces y limpia la lista AllPieces. Asi no hay problemas
        // cuando se crea un nuevo cubo
        foreach (GameObject cubePieces in AllPieces)
        {
            DestroyImmediate(cubePieces);
        }
        AllPieces.Clear();
        for (int x = 0; x < 3; x++) //para generar los cubos en x
        {
            for (int y = 0; y < 3; y++) //para generar los cubos en y
            {
                for (int z = 0; z < 3; z++) //para generar los cubos en z
                {
                    GameObject cuboPieceInstace = Instantiate(CubePiece, CubeTransform, false);
                    cuboPieceInstace.transform.localPosition = new Vector3(-x, -y, z);
                    cuboPieceInstace.GetComponent<cubePieceScr>().SetColor(-x, -y, z);
                    AllPieces.Add(cuboPieceInstace);//agrego a la lista cada cubo
                }
            }
        }
        //necesito tener registro de donde esta el centro del cubo.
        //El centro del cubo es la piece 13 de todo el cubo. Se puede ver graficamente
        CubeCenter = AllPieces[13];
    }

    private void CheckInput()
    {    //Si la tecla apretada para rotar es la de rotar la tapa de arriba, roto en y
        
        if (Input.GetKeyDown(upRotation))
            StartCoroutine(Rotate(UpPieces, new Vector3(0, 1, 0)));
        ////Si la tecla apretada para rotar es la de rotar la tapa de abajo, roto en y
        else if (Input.GetKeyDown(downRotation))
            StartCoroutine(Rotate(DownPieces, new Vector3(0, 1, 0)));
        //Si la tecla apretada para rotar es la de rotar la tapa de la izqda, roto en z
        else if (Input.GetKeyDown(leftRotation))
            StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, 1)));
        //Si la tecla apretada para rotar es la de rotar la tapa de la drcha, roto en z
        else if (Input.GetKeyDown(rightRotation))
            StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, 1)));
        //Si la tecla apretada para rotar es la de rotar la tapa del frente, roto en x
        else if (Input.GetKeyDown(frontRotation))
            StartCoroutine(Rotate(FrontPieces, new Vector3(1, 0, 0)));
        //Si la tecla apretada para rotar es la de rotar la tapa de atras, roto en x
        else if (Input.GetKeyDown(backRotation))
            StartCoroutine(Rotate(BackPieces, new Vector3(1, 0, 0)));
        else if(Input.GetKeyDown(resetCube) && canShuffle)
               CreateCube();
        else if (Input.GetKeyDown(shuffleCube) && canShuffle)
            StartCoroutine(Shuffle());
    }
//funcion para detectar que se esta rotando con el mouse
    public void DetectRotate(List<GameObject> pieces, List<GameObject> planes)
    {
        if (!canRotate || !canShuffle)
            return;
        // Controla la rotacion del medio
        /*if( UpVerticalPieces.Exists(x => x == pieces[0]) &&
           UpVerticalPieces.Exists(x => x == pieces[1]))
        {
            StartCoroutine(Rotate(UpVerticalPieces, new Vector3(0, 0, -1)));
        }else if( UpHorizontalPieces.Exists(x => x == pieces[0]) &&
                  UpHorizontalPieces.Exists(x => x == pieces[1]))
        {
            StartCoroutine(Rotate(UpHorizontalPieces, new Vector3(-1, 0, 0)));
        }/*
        else if( FrontHorizontalPieces.Exists(x => x == pieces[0]) &&
                 FrontHorizontalPieces.Exists(x => x == pieces[1]))
        {
            StartCoroutine(Rotate(FrontHorizontalPieces, new Vector3(0, 1, 0)));
        }else*/
        //Controla la rotacion de los costados
        //Piezas de la parte de arriba
        if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 0, 1), UpPieces))
            StartCoroutine(Rotate(UpPieces, new Vector3(0, 1 , 0)));
        //piezas de la parte de abajo
        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 0, 1), DownPieces))
            StartCoroutine(Rotate(DownPieces, new Vector3(0, 1 , 0)));
        //piezas del lado frontal
        else if (DetectSide(planes, new Vector3(0, 0, 1), new Vector3(0, 1, 0), FrontPieces))
            StartCoroutine(Rotate(FrontPieces, new Vector3(1 * DetectFrontBackSign(pieces), 0, 0)));
        //piezas del lado trasero
        else if (DetectSide(planes, new Vector3(0, 0, 1), new Vector3(0, 1, 0), BackPieces))
            StartCoroutine(Rotate(BackPieces, new Vector3(1 * DetectFrontBackSign(pieces), 0, 0)));
        //piezas de la izda
        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 1, 0), LeftPieces))
            StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, 1 * DetectLeftRightSign(pieces))));
        //piezas de la drcha
        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 1, 0), RightPieces))
            StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, 1 * DetectLeftRightSign(pieces))));
        
        
    }
//detecta el signo para ver como gira la parte de los costados
    float DetectLeftRightSign(List<GameObject> pieces)
    {
        float sign = 0f;
        if (Mathf.Round(pieces[1].transform.position.y) != Mathf.Round(pieces[0].transform.position.y))
        {
            if (Mathf.Round(pieces[0].transform.position.x) == -2)
                sign = Mathf.Round(pieces[0].transform.position.y) - Mathf.Round(pieces[1].transform.position.y);
            else 
                sign = Mathf.Round(pieces[1].transform.position.y) - Mathf.Round(pieces[0].transform.position.y);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.y) == -2)
                sign = Mathf.Round(pieces[1].transform.position.x) - Mathf.Round(pieces[0].transform.position.x);
            else 
                sign = Mathf.Round(pieces[0].transform.position.x) - Mathf.Round(pieces[1].transform.position.x);
        }

        return sign;
    }
    //detecta el signo para ver como gira la parte del frente y de atras
    float DetectFrontBackSign(List<GameObject> pieces)
    {
        float sign = 0f;
        if (Mathf.Round(pieces[0].transform.position.z) != Mathf.Round(pieces[0].transform.position.z))
        {
            if (Mathf.Round(pieces[0].transform.position.y) == 0)
                sign = Mathf.Round(pieces[1].transform.position.z) - Mathf.Round(pieces[0].transform.position.z);
            else 
                sign = Mathf.Round(pieces[0].transform.position.z) - Mathf.Round(pieces[1].transform.position.z);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.z) == 0)
                sign = Mathf.Round(pieces[1].transform.position.y) - Mathf.Round(pieces[0].transform.position.y);
            else 
                sign = Mathf.Round(pieces[0].transform.position.y) - Mathf.Round(pieces[1].transform.position.y);
        }

        return sign;
    }
    //detecta el signo para ver como gira la parte de arriba y de abajo
    float DetectUpDownSign(List<GameObject> pieces)
    {
        float sign = 0f;
        if (Mathf.Round(pieces[0].transform.position.z) != Mathf.Round(pieces[0].transform.position.z))
        {
            if (Mathf.Round(pieces[0].transform.position.x) == -2)
                sign = Mathf.Round(pieces[1].transform.position.z) - Mathf.Round(pieces[0].transform.position.z);
            else 
                sign = Mathf.Round(pieces[0].transform.position.z) - Mathf.Round(pieces[1].transform.position.z);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.z) == 0)
                sign = Mathf.Round(pieces[0].transform.position.x) - Mathf.Round(pieces[1].transform.position.x);
            else 
                sign = Mathf.Round(pieces[1].transform.position.x) - Mathf.Round(pieces[0].transform.position.x);
        }

        return sign;
    }
//funcion para detectar que lado se esta rotando
    private bool DetectSide(List<GameObject> planes, Vector3 firstDirection, Vector3 secondDirection, List<GameObject> side)
    {
        GameObject centerPiece = side.Find(x =>
            x.GetComponent<cubePieceScr>().Planes.FindAll(y => y.activeInHierarchy).Count == 1);
        List<RaycastHit> hit1 = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, firstDirection)),
            hit2 = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, firstDirection)),
            hit1_m = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, -firstDirection)),
            hit2_m = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position,-firstDirection)),
            
            hit3 = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, secondDirection)),
            hit4 = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, secondDirection)),
            hit3_m = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, -secondDirection)),
            hit4_m = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position,-secondDirection));
        return hit1.Exists(x => x.collider.gameObject == centerPiece) ||
               hit2.Exists(x => x.collider.gameObject == centerPiece) ||
               hit1_m.Exists(x => x.collider.gameObject == centerPiece) ||
               hit2_m.Exists(x => x.collider.gameObject == centerPiece) ||
               
               hit3.Exists(x => x.collider.gameObject == centerPiece) ||
               hit4.Exists(x => x.collider.gameObject == centerPiece) ||
               hit3_m.Exists(x => x.collider.gameObject == centerPiece) ||
               hit4_m.Exists(x => x.collider.gameObject == centerPiece) ;
    }
//Funcion para chequear si el cubo esta resuelto completamente
    void CheckComplete()
    {
        if (SideComplete(UpPieces) && SideComplete(DownPieces)
                                   && SideComplete(LeftPieces) && SideComplete(RightPieces)
                                   && SideComplete(FrontPieces) && SideComplete(BackPieces))
        {
            Debug.Log("Congrats");
            GetComponent<timer>().Finish();
        }
    }
//Funcion para chequear si un lado del cubo esta completo
    bool SideComplete(List<GameObject> pieces)
    {//Encuentro el cubo del centro de esa cara del cubo
        int centerOfFace = pieces[4].GetComponent<cubePieceScr>().Planes.FindIndex(x => x.activeInHierarchy);
        // comparo ese cubo del centro con los demas cubos de la cara, si hay alguno que no es igual devuelvo false
        // ademas comparo el color de ese cubo con los otros colores que hay en esa cara del cubo
        for (int i = 0; i < pieces.Count; i++)
        {
            if(!pieces[i].GetComponent<cubePieceScr>().Planes[centerOfFace].activeInHierarchy ||
               pieces[i].GetComponent<cubePieceScr>().Planes[centerOfFace].GetComponent<Renderer>().material.color !=
               pieces[4].GetComponent<cubePieceScr>().Planes[centerOfFace].GetComponent<Renderer>().material.color)
            {
                return false;
            }
        }
        return true;
    }
    //public void PlaySound()
    //{
     //   MusicSource.Play();
        
    //}
// Corrutina para calcular la rotacion
    IEnumerator Rotate(List<GameObject> pieces, Vector3 rotationVector, int speed = 5)
    {
        MusicSource.Play();
        canRotate = false;
        int angle = 0;
        while (angle < 90)
        {
            foreach (GameObject gameObjectPieces in pieces)
            {
                //roto  alrededor del centro del cubo
                gameObjectPieces.transform.RotateAround(CubeCenter.transform.position, rotationVector, speed);
            }
            angle += speed;
            //PlaySound();
            yield return null;
        }
        CheckComplete();
        canRotate = true;
    }
//corrutina para mesclar el cubo
    IEnumerator Shuffle()
    {
        canShuffle = false;
        for (int movement = Random.Range(45, 60); movement >= 0; movement--)
        {
            int edge = Random.Range(0, 6);
            List<GameObject> edgePiece = new List<GameObject>();
            switch (edge)
            {
                case 0:
                    edgePiece = UpPieces; break;
                case 1:
                    edgePiece = DownPieces; break;
                case 2:
                    edgePiece = LeftPieces; break;
                case 3:
                    edgePiece = RightPieces; break;
                case 4:
                    edgePiece = FrontPieces; break;
                case 5:
                    edgePiece = BackPieces; break;
            }

            StartCoroutine(Rotate(edgePiece, RotateVectors[edge], 15));
            yield return new WaitForSeconds(0.1f);
        }
        canShuffle = true;
    }
}
