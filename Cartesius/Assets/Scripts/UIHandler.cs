using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public List<GameObject> noisePollutionMaps; 
    public List<GameObject> nitrogenDioxideMaps; 
    public Canvas noisePollutionLegendCanvas; 
    public Canvas nitrogenDioxideLegendCanvas; 
    public List<Button> indicatorButtons; 
    public TMP_Text ggoScoresText; 
    public Button ggoScoreButton; 
    public List<Button> phaseButtons;  
    public TMP_Text phaseDescriptionText; 
    public Button phaseDescriptionButton; 
    private int currentPhase = 0;
    private bool showingNoisePollution = true;
    private Color defaultColor = new Color32(199, 212, 231, 255); // C7D4E7
    private Color activeColor = new Color32(47, 121, 231, 255); // 2F79E7

    // Scores voor elke fase
    private Dictionary<int, List<float>> phaseScores = new Dictionary<int, List<float>>()
    {
        { 0, new List<float> { 7.5f } },
        { 1, new List<float> { 8.3f } },
        { 2, new List<float> { 7.5f } },
        { 3, new List<float> { 7.6f } },
        { 4, new List<float> { 7.6f } },
        { 5, new List<float> { 8.6f } }
    };

    // Descriptions for each phase
    private Dictionary<int, string> phaseDescriptions = new Dictionary<int, string>()
    {
        { 0, "De indeling is van fase 1 t/m 6 overgenomen uit het omgevingsplan van het koersdocument 2017. Hierbij zijn de woningen ontworpen in Tygron zelf. Dit is een schets van de werkelijkheid. Hierdoor zijn het aantal woningen van de Cartesius driehoek in Tygron ook verschillend van de werkelijk aantal woningen. Het aantal auto’s, vrachtwagens en busjes is ook in elke fase gelijk. Namelijk 40 auto’s, 20 busjes en 15 vrachtwagens." },
        { 1, "Verschil fase 1 en 2: Toename van het aantal woningen van 650 naar 1890. Verwijdering van de weg rondom het CAB-gebouw, wat de verkeersroutes veranderde en minder geluidsoverlast creëerde." },
        { 2, "Verschil fase 2 en 3: Verandering van de verkeersroutes waardoor het verkeer via de Locomotiefstraat werd geleid en omgeving CAB-gebouw autoluw werd. Door de verkeersverandering nu meer woningen met geluidoverlast." },
        { 3, "Verschil fase 3 en 4: Verdere toevoeging van woningen en herstructurering van de verkeersroutes. Blijven gebruik van de Locomotiefstraat voor verkeersaanvoer, wat enige extra belasting kan hebben veroorzaakt, maar door de extra woningen in gebied met relatief weinig geluidsoverlast score omhoog." },
        { 4, "Verschil fase 4 en 5: Geen significante verandering in de verkeersroutes of verkeersintensiteit ten opzichte van fase 4. Verkeersaanvoer blijft via de Locomotiefstraat gaan, hierdoor geen verandering in scores." },
        { 5, "Verschil fase 5 en 6: Voltooing van alle bouwfases, wat zorgde voor verwijdering van alle wegen binnen het Cartesiusgebied en een autoluwe zone realiseerde. Hierdoor minder verkeersgeluidbelasting in het gebied en verbeterde leefbaarheid." }
    };

    void Start()
    {
        // Initial state: maak fase 1 actief en laat geluidsoverlastkaart van fase 1 zien
        SetActivePhase(0); 
        ShowNoisePollutionMap(); 
        UpdateIndicatorButtonColors(); 
    }

    public void ShowNoisePollutionMap()
    {
        showingNoisePollution = true; 
        UpdateMaps(); 
        UpdateIndicatorButtonColors(); 
        SwitchCanvas(); 
        UpdateScores(); 
        ShowNoisePollutionScoresTextAndButton();
    }

    public void ShowNitrogenDioxideMap()
    {
        showingNoisePollution = false; 
        UpdateMaps(); 
        UpdateIndicatorButtonColors(); 
        SwitchCanvas(); 
        HideNoisePollutionScoresTextAndButton(); 
    }

    // Methode om de juiste kaarten te activeren op basis van de huidige fase en indicator status
    private void UpdateMaps()
    {
        for (int phase = 0; phase < noisePollutionMaps.Count; phase++)
        {
            if (phase == currentPhase)
            {
                // Activeer de juiste kaart voor de huidige fase en indicator status
                noisePollutionMaps[phase].SetActive(showingNoisePollution);
                nitrogenDioxideMaps[phase].SetActive(!showingNoisePollution);
            }
            else
            {
                // Deactiveer alle kaarten voor niet-actieve fases
                noisePollutionMaps[phase].SetActive(false);
                nitrogenDioxideMaps[phase].SetActive(false);
            }
        }
    }

    private void UpdateIndicatorButtonColors()
    {
        if (indicatorButtons.Count < 2)
        {
            Debug.LogWarning("Indicator buttons list does not contain enough buttons!");
            return;
        }
        
        // Bepaal welke knoppen actief en inactief zijn en zet de juiste kleuren
        Button activeButton = showingNoisePollution ? indicatorButtons[0] : indicatorButtons[1];
        Button inactiveButton = showingNoisePollution ? indicatorButtons[1] : indicatorButtons[0];
        SetButtonColors(activeButton, inactiveButton);
    }

    private void SetButtonColors(Button activeButton, Button inactiveButton)
    {
        if (activeButton == null || inactiveButton == null)
        {
            Debug.LogWarning("One of the buttons is null!");
            return;
        }

        Image activeImage = activeButton.GetComponent<Image>();
        Image inactiveImage = inactiveButton.GetComponent<Image>();

        if (activeImage == null || inactiveImage == null)
        {
            Debug.LogWarning("One of the buttons is missing an Image component!");
            return;
        }

        activeImage.color = activeColor;
        inactiveImage.color = defaultColor;
    }

    public void SetActivePhase(int phaseIndex)
    {
        currentPhase = phaseIndex;
        UpdatePhaseButtons(); 
        UpdateMaps(); 
        if (showingNoisePollution)
        {
            UpdateScores();
        }
        UpdatePhaseDescription();
    }

    private void UpdatePhaseButtons()
    {
        for (int i = 0; i < phaseButtons.Count; i++)
        {
            if (i == currentPhase)
            {
                phaseButtons[i].image.color = activeColor; 
            }
            else
            {
                phaseButtons[i].image.color = defaultColor; 
            }
        }
    }

    private void SwitchCanvas()
    {
        if (noisePollutionLegendCanvas != null && nitrogenDioxideLegendCanvas != null)
        {
            noisePollutionLegendCanvas.gameObject.SetActive(showingNoisePollution);
            nitrogenDioxideLegendCanvas.gameObject.SetActive(!showingNoisePollution);
        }
        else
        {
            Debug.LogWarning("One of the canvas references is missing!");
        }
    }

    private void UpdateScores()
    {
        if (ggoScoresText != null)
        {
            List<float> scores = phaseScores[currentPhase];
            ggoScoresText.text = "GGO Score\nGeluidsoverlast verkeer\n";
            ggoScoresText.text += $"\nCartesius driehoek: {scores[0]}\n";
            ggoScoresText.gameObject.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("Scores text component is missing!");
        }
    }

    private void HideNoisePollutionScoresTextAndButton()
    {
        if (ggoScoresText != null)
        {
            ggoScoresText.gameObject.SetActive(false); 
            ggoScoreButton.gameObject.SetActive(false); 
            phaseDescriptionText.gameObject.SetActive(false);
            phaseDescriptionButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Scores text component is missing!");
        }
    }

    private void ShowNoisePollutionScoresTextAndButton()
    {
        if (ggoScoresText != null)
        {
            ggoScoresText.gameObject.SetActive(true); 
            ggoScoreButton.gameObject.SetActive(true); 
            phaseDescriptionText.gameObject.SetActive(true);
            phaseDescriptionButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Scores text component is missing!");
        }
    }

    private void UpdatePhaseDescription()
    {
        if (phaseDescriptionText != null)
        {
            phaseDescriptionText.text = phaseDescriptions[currentPhase];
            phaseDescriptionText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Phase description text component is missing!");
        }
    }
}
