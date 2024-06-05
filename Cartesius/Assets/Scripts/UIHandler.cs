using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // Lijsten met kaarten voor elke fase
    public List<GameObject> geluidsOverlastKaarten; // Lijst van geluidsoverlastkaarten voor elke fase
    public List<GameObject> stikstofDioxideKaarten; // Lijst van stikstofdioxidekaarten voor elke fase

    // Canvases for LegendaGeluidsOverlast and LegendaStikstofdioxide
    public Canvas legendaGeluidsOverlastCanvas; // Canvas for LegendaGeluidsOverlast
    public Canvas legendaStikstofdioxideCanvas; // Canvas for LegendaStikstofdioxide

    // Lijst van knoppen voor de indicatoren (geluidsoverlast en stikstofdioxide)
    public List<Button> indicatorButtons; // Lijst van indicator knoppen

    // Lijst van knoppen voor de verschillende fases
    public List<Button> faseButtons; // Lijst van fase knoppen

    // Kleuren voor actieve en inactieve knoppen
    private Color defaultColor = new Color32(164, 187, 221, 255); // A4BBDD
    private Color activeColor = new Color32(47, 121, 231, 255); // 2F79E7

    // Huidige actieve fase en indicator status
    private int currentPhase = 0; // Houdt bij welke fase momenteel actief is
    private bool showingNoisePollution = true; // Houdt bij welke kaart momenteel wordt weergegeven (geluidsoverlast of stikstofdioxide)

    // Start is called before the first frame update
    void Start()
    {
        // Initial state: maak fase 1 actief en laat geluidsoverlastkaart van fase 1 zien
        SetActivePhase(0); // Maak fase 1 actief en update knoppen
        ShowgeluidsOverlastKaart(); // Laat de geluidsoverlastkaart zien
        UpdateIndicatorButtonColors(); // Update de kleuren van de indicator knoppen
    }

    // Methode om geluidsoverlastkaart weer te geven
    public void ShowgeluidsOverlastKaart()
    {
        showingNoisePollution = true; // Zet de status naar geluidsoverlast
        UpdateKaarten(); // Update de weergegeven kaarten
        UpdateIndicatorButtonColors(); // Update de kleuren van de indicator knoppen
        SwitchCanvas(); // Switch to the corresponding canvas
    }

    // Methode om stikstofdioxidekaart weer te geven
    public void ShowstikstofDioxideKaart()
    {
        showingNoisePollution = false; // Zet de status naar stikstofdioxide
        UpdateKaarten(); // Update de weergegeven kaarten
        UpdateIndicatorButtonColors(); // Update de kleuren van de indicator knoppen
        SwitchCanvas(); // Switch to the corresponding canvas
    }

    // Methode om de juiste kaarten te activeren op basis van de huidige fase en indicator status
    private void UpdateKaarten()
    {
        for (int i = 0; i < geluidsOverlastKaarten.Count; i++)
        {
            if (i == currentPhase)
            {
                // Activeer de juiste kaart voor de huidige fase en indicator status
                geluidsOverlastKaarten[i].SetActive(showingNoisePollution);
                stikstofDioxideKaarten[i].SetActive(!showingNoisePollution);
            }
            else
            {
                // Deactiveer alle kaarten voor niet-actieve fases
                geluidsOverlastKaarten[i].SetActive(false);
                stikstofDioxideKaarten[i].SetActive(false);
            }
        }
    }

    // Methode om de kleuren van de indicator knoppen bij te werken
    private void UpdateIndicatorButtonColors()
    {
        if (indicatorButtons.Count >= 2)
        {
            // Bepaal welke knoppen actief en inactief zijn
            Button activeButton = showingNoisePollution ? indicatorButtons[0] : indicatorButtons[1];
            Button inactiveButton = showingNoisePollution ? indicatorButtons[1] : indicatorButtons[0];
            SetButtonColors(activeButton, inactiveButton); // Update de knoppenkleuren
        }
        else
        {
            Debug.LogWarning("Indicator buttons list does not contain enough buttons!");
        }
    }

    // Methode om de kleuren van de knoppen in te stellen
    private void SetButtonColors(Button activeButton, Button inactiveButton)
    {
        if (activeButton != null && inactiveButton != null)
        {
            // Haal de Image component op van de knoppen
            Image activeImage = activeButton.GetComponent<Image>();
            Image inactiveImage = inactiveButton.GetComponent<Image>();

            if (activeImage != null && inactiveImage != null)
            {
                // Stel de kleuren in
                activeImage.color = activeColor;
                inactiveImage.color = defaultColor;
            }
            else
            {
                Debug.LogWarning("One of the buttons is missing an Image component!");
            }
        }
        else
        {
            Debug.LogWarning("One of the buttons is null!");
        }
    }

    // Methode om de actieve fase te veranderen
    public void SetActivePhase(int phaseIndex)
    {
        currentPhase = phaseIndex; // Stel de huidige fase in
        UpdatePhaseButtons(); // Update de kleuren van de fase knoppen
        UpdateKaarten(); // Zorg ervoor dat de juiste kaart voor de fase wordt getoond
    }

    // Methode om de kleuren van de fase knoppen bij te werken
    private void UpdatePhaseButtons()
    {
        for (int i = 0; i < faseButtons.Count; i++)
        {
            if (i == currentPhase)
            {
                faseButtons[i].image.color = activeColor; // Stel de kleur in voor de actieve fase knop
            }
            else
            {
                faseButtons[i].image.color = defaultColor; // Stel de kleur in voor de inactieve fase knoppen
            }
        }
    }

    // Methode om canvases te wisselen
    private void SwitchCanvas()
    {
        if (legendaGeluidsOverlastCanvas != null && legendaStikstofdioxideCanvas != null)
        {
            legendaGeluidsOverlastCanvas.gameObject.SetActive(showingNoisePollution);
            legendaStikstofdioxideCanvas.gameObject.SetActive(!showingNoisePollution);
        }
        else
        {
            Debug.LogWarning("One of the canvas references is missing!");
        }
    }
}
