using System;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {    //Aqui ficarão as configurações globais do jogo, disponíveis em qualquer cena, e que poserão ser salvas se o jogo contar com um sistema de save
    public static int idLanguage = 0;
    public static float volumeOST = 1, volumeSFX = 1;

    //Essas informações não serão salvas e só servirão para definir certas coisas no jogo:
    public static bool firstScene = true, playedFirstCutscene=false;
    public enum languages {
        english,
        portuguese,
    }

    public static Dictionary<string, string[]> dictLanguage = new Dictionary<string, string[]> {
        {"txtStart", new string[] {"Start", "Começar"} },
        {"txtResume", new string[] {"Resume", "Continuar"} },
        {"txtQuit", new string[] {"Quit", "Sair"} },
        {"txtReset", new string[] {"Reset", "Recomeçar"} },
        {"txtOptions", new string[] {"Options", "Opções"} },
        {"txtControls", new string[] {"Controls", "Controles" } },
        {"txtLang", new string[] {"Language", "Idioma" } },
        {"txtOST", new string[] {"Music", "Música" } },
        {"txtSFX", new string[] {"Sound Effects", "Efeitos Sonoros" } },
        {"txtSensitivity", new string[] {"Cam Sensitivity", "Sensibilidade da Câmera" } },
        {"txtResetSave", new string[] {"Reset Data", "Resetar Dados" } },
        {"langEnglish", new string[] {"English", "Inglês" } },
        {"langPortuguese", new string[] {"Portuguese", "Português" } },
        {"txtControlPause", new string[] {"Pause", "Pausar" } },
    };

    //Métodos para salvar/carregar dados
    /*
    public static void SaveData() {
        string path = Application.persistentDataPath + "/globals.bin";
        ConfigsData data = new ConfigsData(idLanguage, volumeOST, volumeSFX);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
        //Debug.Log("Dados salvos com sucesso!");
    }

    public static void LoadData() {
        string path = Application.persistentDataPath + "/globals.bin";
        if (File.Exists(path)) {   //Se o arquivo existir
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            ConfigsData data = formatter.Deserialize(fileStream) as ConfigsData;
            fileStream.Close();

            idLanguage = data.idLanguage;
            volumeOST = data.volumeOST;
            volumeSFX = data.volumeSFX;

            //Debug.Log("Dados carregados com sucesso!");
        }
    }

    public static void ResetData() {
        idLanguage = 0;
        SaveData();
        LoadData();
        //Debug.Log("Dados zerados com sucesso!");
        TransitionController.GetInstance().LoadMenu();
    }
    */
}

[Serializable]
public class ConfigsData {
    public int idLanguage, numCoins, levelPistol, levelSMG, recordEnemiesDefeated, camSensitivity;
    public float volumeOST, volumeSFX, pistolDamageTax, SMGDamageTax;
    public bool hasMisteryGun;

    public ConfigsData(int idLanguage, float volumeOST, float volumeSFX) {
        this.idLanguage = idLanguage;
        this.volumeOST = volumeOST;
        this.volumeSFX = volumeSFX;
    }
}
