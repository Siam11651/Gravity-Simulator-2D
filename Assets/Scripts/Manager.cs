using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject panelNewPlanet, referencePlanet, panelSelectedPlanet, panelSettings;
    [SerializeField] private Text textSelectedPlanetName, textSelectedPlanetPositionX, textSelectedPlanetPositionY;
    [SerializeField] private InputField inputFieldNewPlanetPositionX, inputFieldNewPlanetPositionY, inputFieldNewPlanetVelocityX, inputFieldNewPlanetVelocityY, inputFieldNewPlanetMass, inputFieldNewPlanetRadius, inputFieldNewPlanetName, inputFieldG;
    [SerializeField] private Slider sliderNewPlanetR, sliderNewPlanetG, sliderNewPlanetB;
    [SerializeField] private Toggle toggleDynamic;
    [SerializeField] private Button buttonSelectedPlanetPanelClose, buttonSelectedPlanetDelete, buttonSettings, buttonAddPlanet, buttonCloseSettings, buttonPlayPause, buttonReset;
    private GameObject planetToBeAdded, selectedPlanet;
    private List<GameObject> _planets;
    private float _G;
    public float G
    {
        get
        {
            return _G;
        }
    }
    public List<GameObject> planets
    {
        get
        {
            return _planets;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedPlanet = null;
        planetToBeAdded = null;
        _planets = new List<GameObject>();
        _G = float.Parse(inputFieldG.text);
        toggleDynamic.onValueChanged.AddListener((bool newValue) =>
        {
            if(newValue)
            {
                inputFieldNewPlanetVelocityX.interactable = true;
                inputFieldNewPlanetVelocityY.interactable = true;
            }
            else
            {
                inputFieldNewPlanetVelocityX.interactable = false;
                inputFieldNewPlanetVelocityY.interactable = false;
            }
        });
        inputFieldNewPlanetPositionX.onValueChanged.AddListener((string newValue)=>
        {
            float newFloat;

            if(planetToBeAdded != null && float.TryParse(newValue, out newFloat))
            {
                planetToBeAdded.transform.position = new Vector2(newFloat, planetToBeAdded.transform.position.y);
            }
        });
        inputFieldNewPlanetPositionY.onValueChanged.AddListener((string newValue) =>
        {
            float newFloat;

            if(planetToBeAdded != null && float.TryParse(newValue, out newFloat))
            {
                planetToBeAdded.transform.position = new Vector2(planetToBeAdded.transform.position.x, newFloat);
            }
        });
        inputFieldNewPlanetRadius.onValueChanged.AddListener((string newValue) =>
        {
            float newFloat;

            if(planetToBeAdded != null && float.TryParse(newValue, out newFloat))
            {
                planetToBeAdded.transform.localScale = new Vector2(newFloat, newFloat) * 2;
            }
        });
        inputFieldG.onEndEdit.AddListener((string newValue)=>
        {
            float newFloat;

            if(float.TryParse(newValue, out newFloat))
            {
                _G = newFloat;
            }
            else
            {
                inputFieldG.text = G.ToString("e3");
            }
        });
        sliderNewPlanetR.onValueChanged.AddListener((float newValue) =>
        {
            if(planetToBeAdded != null)
            {
                SpriteRenderer spriteRenderer = planetToBeAdded.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(newValue / 255f, spriteRenderer.color.g, spriteRenderer.color.b);
            }
        });
        sliderNewPlanetG.onValueChanged.AddListener((float newValue) =>
        {
            if(planetToBeAdded != null)
            {
                SpriteRenderer spriteRenderer = planetToBeAdded.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(spriteRenderer.color.r, newValue / 255f, spriteRenderer.color.b);
            }
        });
        sliderNewPlanetB.onValueChanged.AddListener((float newValue) =>
        {
            if(planetToBeAdded != null)
            {
                SpriteRenderer spriteRenderer = planetToBeAdded.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, newValue / 255f);
            }
        });
        buttonSelectedPlanetPanelClose.onClick.AddListener(()=>
        {
            selectedPlanet = null;

            panelSelectedPlanet.SetActive(false);
        });
        buttonSelectedPlanetDelete.onClick.AddListener(() =>
        {
            planets.Remove(selectedPlanet);
            Destroy(selectedPlanet);

            selectedPlanet = null;

            panelSelectedPlanet.SetActive(false);
        });
        buttonSettings.onClick.AddListener(()=>
        {
            Time.timeScale = 0;
            buttonAddPlanet.enabled = false;
            buttonSettings.enabled = false;
            buttonPlayPause.enabled = false;

            panelSettings.SetActive(true);
        });
        buttonCloseSettings.onClick.AddListener(()=>
        {
            buttonAddPlanet.enabled = true;
            buttonSettings.enabled = true;
            buttonPlayPause.enabled = true;

            panelSettings.SetActive(false);

            Time.timeScale = 1;
        });
        buttonPlayPause.onClick.AddListener(()=>
        {
            Text textPlayPause = buttonPlayPause.GetComponentInChildren<Text>();

            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
                textPlayPause.text = "l l";
            }
            else
            {
                Time.timeScale = 0;
                textPlayPause.text = "▶";
            }
        });
        buttonReset.onClick.AddListener(()=>
        {
            foreach(GameObject gObject in planets)
            {
                Destroy(gObject);
            }

            planets.Clear();
        });
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - Input.mouseScrollDelta.y, 0.01f);

        if(Input.GetMouseButtonUp(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = new Ray(new Vector3(position.x, position.y, -1), new Vector3(0, 0, 1));
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if(hit.collider != null && hit.collider.tag == "Planet")
            {
                selectedPlanet = hit.collider.gameObject;

                panelSelectedPlanet.SetActive(true);

                textSelectedPlanetName.text = selectedPlanet.name;
            }
            else
            {
                selectedPlanet = null;
            }
        }

        if(selectedPlanet != null)
        {
            textSelectedPlanetPositionX.text = selectedPlanet.transform.position.x.ToString("F2"); ;
            textSelectedPlanetPositionY.text = selectedPlanet.transform.position.y.ToString("F2");
        }
    }

    public void AddNewPlanet()
    {
        Time.timeScale = 0;
        buttonSettings.enabled = false;
        buttonAddPlanet.enabled = false;
        buttonPlayPause.enabled = false;

        panelNewPlanet.SetActive(true);

        inputFieldNewPlanetMass.text = Random.Range(0.01f, 5f).ToString("F2");
        inputFieldNewPlanetRadius.text = Random.Range(0.01f, 1.5f).ToString("F2");
        inputFieldNewPlanetPositionX.text = Random.Range(-2f, 2f).ToString("F2");
        inputFieldNewPlanetPositionY.text = Random.Range(-2f, 2f).ToString("F2");
        inputFieldNewPlanetVelocityX.text = 0.00f.ToString("F2");
        inputFieldNewPlanetVelocityY.text = 0.00f.ToString("F2");
        sliderNewPlanetR.value = Random.Range(0, 256);
        sliderNewPlanetG.value = Random.Range(0, 256);
        sliderNewPlanetB.value = Random.Range(0, 256);
        toggleDynamic.isOn = true;
        planetToBeAdded = Instantiate(referencePlanet);
        planetToBeAdded.name = "Untitled";
        planetToBeAdded.transform.position = new Vector2(float.Parse(inputFieldNewPlanetPositionX.text), float.Parse(inputFieldNewPlanetPositionY.text));
        planetToBeAdded.transform.localScale = new Vector2(float.Parse(inputFieldNewPlanetRadius.text), float.Parse(inputFieldNewPlanetRadius.text)) * 2f;
        planetToBeAdded.GetComponent<SpriteRenderer>().color = new Color(sliderNewPlanetR.value / 255f, sliderNewPlanetG.value / 255f, sliderNewPlanetB.value / 255f);
    }

    public void ConfrimPlanet()
    {
        float newVelocityX, newVelocityY, newMass;

        if(!float.TryParse(inputFieldNewPlanetVelocityX.text, out newVelocityX) ||
            !float.TryParse(inputFieldNewPlanetVelocityY.text, out newVelocityY) ||
            !float.TryParse(inputFieldNewPlanetMass.text, out newMass))
        {
            return;
        }

        panelNewPlanet.SetActive(false);

        planetToBeAdded.name = inputFieldNewPlanetName.text;
        PlanetBehaviour planetBehaviour = planetToBeAdded.GetComponent<PlanetBehaviour>();
        planetBehaviour.gActive = true;
        planetBehaviour.isDynamic = toggleDynamic.isOn;
        planetToBeAdded.GetComponent<Rigidbody>().mass = newMass;
        TrailRenderer trailRenderer = planetToBeAdded.GetComponent<TrailRenderer>();

        if(planetBehaviour.isDynamic)
        {
            planetToBeAdded.GetComponent<Rigidbody>().velocity = new Vector2(newVelocityX, newVelocityY);
            trailRenderer.enabled = true;
            trailRenderer.startWidth = planetToBeAdded.transform.localScale.x * 0.5f;
            trailRenderer.endWidth = planetToBeAdded.transform.localScale.x * 0.5f;
            Color color = planetToBeAdded.GetComponent<SpriteRenderer>().color;
            trailRenderer.startColor = new Color(color.r, color.g, color.b, 0.4f);
            trailRenderer.endColor = new Color(color.r, color.g, color.b, 0);
        }
        else
        {
            planetToBeAdded.GetComponent<Rigidbody>().velocity = Vector2.zero;
        }
        
        planets.Add(planetToBeAdded);

        planetToBeAdded = null;
        Time.timeScale = 1;
        buttonSettings.enabled = true;
        buttonAddPlanet.enabled = true;
        buttonPlayPause.enabled = true;
    }

    public void CancelAddingNewPlanet()
    {
        buttonSettings.enabled = true;
        buttonAddPlanet.enabled = true;
        buttonPlayPause.enabled = true;

        Destroy(planetToBeAdded);
        panelNewPlanet.SetActive(false);

        planetToBeAdded = null;
        Time.timeScale = 1;
    }
}
