using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _MGR_Night_Day : MonoBehaviour
{
    private static _MGR_Night_Day p_instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public static _MGR_Night_Day Instance { get { return p_instance; } }     // READ ONLY

    private float chrono;       // chrono de la partie
    public CYCLE cycleEnum; // variable de l'énumération qui sait à tous moments s'il fait nuit ou jour
    [SerializeField] private bool activateDayWipeEnemiesAtDay;
    [SerializeField] private bool regenerateDepositesAtDay;
    public enum CYCLE { DAY, NIGHT };

    public GameObject playerPosition;
    public GameObject spawnersContainer;
    //public List<Spawner> spawnersList = new List<Spawner>();

    [System.Serializable]
    public class Night
    {
        public float nightDuration;
        [Tooltip("Ajouter un prefab qui est dans l'objet inacif : SpawnerPrefabs")]
        public List<GameObject> spawnersListPrefabs;
        //public Dictionary<GameObject, Transform> spawnersPrefabPosition;
    }
    [System.Serializable]
    public class Day
    {
        public float dayDuration;
    }
    public List<Night> nightsList;
    public List<Day> dayList;
    [HideInInspector] public int nightCount;
    [HideInInspector] public int dayCount;
    public float volumeDay;
    public float volumeNight;

    #region UI
    [SerializeField] private Image dayImage;
    [SerializeField] private Image nightImage;
    [SerializeField] private Text dayTimer;
    #endregion

    void Awake()
    {        // ===>> Singleton Manager

        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject);   par nécessaire ici car déja fait par script __DDOL sur l'objet _EGO_app qui recueille tous les mgr

        // on met le jour dès le début et le chrono = au délai pour que la nuit arrive au bon moment
        //cycleChrono = cycleDelay;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        _MGR_SoundDesign.Instance.PlayMusic(_MGR_SoundDesign.Instance.day, playerPosition, volumeDay);

        cycleEnum = CYCLE.DAY;
        nightImage.gameObject.SetActive(false);
        nightCount = 0;
        dayCount = 0;
    }

    void Update()
    {
        //initialisation du chrono et détermination du jour ou de la nuit
        //Si on est le jour et qu'on a terminé la durée du jour, on passe à la nuit

        chrono += Time.deltaTime;

        if (cycleEnum == CYCLE.DAY)
        {
            if (chrono >= Instance.dayList[dayCount].dayDuration)
            {

                DayEnds();
                if (nightsList.Count > nightCount)
                    NightBegins(nightCount);

                chrono = 0f;
            }
            dayTimer.text = Mathf.RoundToInt(Instance.dayList[dayCount].dayDuration - chrono).ToString();
        }
        //Sinon on est la nuit, et quand on a terminé la durée de la nuit, on passe au jour
        else if (cycleEnum == CYCLE.NIGHT)
        {

            if (chrono >= nightsList[nightCount].nightDuration)
            {

                NightEnds();
                if (dayList.Count > dayCount)
                    DayBegins(dayCount);
                chrono = 0f;
            }

        }

        //Debug.Log(nightCount+" "+nightsList[nightCount].spawnersListPrefabs[1].GetComponent<Spawner>().targets[0]);

    }
    //Quand la nuit commence
    void NightBegins(int nightNumber)
    {
        dayTimer.gameObject.SetActive(false);
        _MGR_SoundDesign.Instance.PlayMusic(_MGR_SoundDesign.Instance.night, playerPosition, volumeNight);
        cycleEnum = CYCLE.NIGHT;
        dayImage.gameObject.SetActive(false);
        nightImage.gameObject.SetActive(true);
        //On instancie les spawners

        for (int i = 0; i < nightsList[nightNumber].spawnersListPrefabs.Count; i++)
        {
            Instantiate(nightsList[nightCount].spawnersListPrefabs[i]).transform.parent = spawnersContainer.transform;
        }


    }

    void NightEnds()
    {
        Debug.Log("A la fin de la nuit, il restait : " + _MGR_Enemies.Instance.list_enemies.Count);
        if (_MGR_Enemies.Instance.list_enemies.Count >= 0 && activateDayWipeEnemiesAtDay)
        {
            foreach (Enemy enemy in _MGR_Enemies.Instance.list_enemies)
            {
                if (enemy.gameObject != null)
                    Destroy(enemy.gameObject);
            }
            _MGR_Enemies.Instance.list_enemies.Clear();
        }
        dayCount++;
        Debug.Log(dayCount + " - " + nightsList.Count);
        if (dayCount >= nightsList.Count)
        {
            _MGR_SceneManager.Instance.victory = true;
            _MGR_SceneManager.Instance.LoadScene("End");
        }
    }

    //Quand le jour commence, régénère les gisements
    void DayBegins(int dayNumber)
    {
        dayTimer.gameObject.SetActive(true);
        _MGR_SoundDesign.Instance.PlayMusic(_MGR_SoundDesign.Instance.day, playerPosition, volumeDay);
        dayImage.gameObject.SetActive(true);
        nightImage.gameObject.SetActive(false);
        cycleEnum = CYCLE.DAY;
        if (regenerateDepositesAtDay)
        {
            for (int i = 0; i < _MGR_Ressource.Instance.list_deposits.Count; i++)
            {
                _MGR_Ressource.Instance.list_deposits[i].RegenerateDeposit();
            }
        }
        nightCount++;
    }

    void DayEnds()
    {

    }
}
