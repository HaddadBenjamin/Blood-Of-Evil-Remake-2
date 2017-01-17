﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BloodOfEvil.Helpers
{
    using Scene;
    using Player;
    using ObjectInScene;
    using Utilities.Serialization;

    public static class SceneManagerHelper
    {
        /// <summary>
        /// Sauvegarde la scène courante.
        /// </summary>
        public static void SaveCurrentScene()
        {
            FileSystemHelper.SafeDeleteDirectoryRecursively(
                SerializerHelper.GetCompleteSavePath(
                SceneServicesContainer.Instance.FileSystemConfiguration.GetCurrentSceneDirectory(),
                ""));

            ((ISerializable)SceneServicesContainer.Instance.SceneStateModule).Save();

            SerializerHelper.JsonSerializeSaveWithEncryption(
                new SerializableString(GetCurrentSceneName()),
                    SceneServicesContainer.Instance.FileSystemConfiguration.SceneNameFilename);

            ((ISerializable)PlayerServicesAndModulesContainer.Instance.AttributesModule).Save();
        }

        /// <summary>
        /// Ne charge pas de scènes mais seulement les données du scenestate.
        /// </summary>
        private static void LoadCurrentSceneFiles()
        {
            SceneServicesContainer.Instance.SceneStateModule.Reset();
            SceneServicesContainer.Instance.AudioReferencesArraysService.DisalbleAllSoundFromMusicCategory(); // Cette ligne est obligatoire car le reset scène.

            ((ISerializable)SceneServicesContainer.Instance.SceneStateModule).Load();
        }

        public static void LoadSceneWithoutLoadFiles(string sceneName)
        {
            SceneServicesContainer.Instance.SceneStateModule.Reset();

            /// Réinitialise la taille des barres de vie et évite un bug comme quoi les barres de vie réaparaisait plusieurs fois.
            //foreach (var crados in SceneServicesContainer.Instance.ObjectsPoolService.GetPool("HealthBarExample").GetGameobjects())
            //    crados.transform.localScale = Vector3.one;
            SceneServicesContainer.Instance.ObjectsPoolService.RemoveAllObjectInPool("HealthBarExample");
            SceneServicesContainer.Instance.AudioReferencesArraysService.DisalbleAllSoundFromMusicCategory();

            SceneManager.LoadScene(sceneName);

            // Permet de vider les références valant null du tas de façon explicite.
            System.GC.Collect();
        }

        /// <summary>
        /// Charge une scène.
        /// </summary>
        public static void LoadScene(string sceneName, bool firstLoadApplication = false)
        {
            if (!firstLoadApplication)
                SaveCurrentScene();

            LoadSceneWithoutLoadFiles(sceneName);
            SceneServicesContainer.Instance.TooltipsService.DisableAllTooltips();

            LoadCurrentSceneFiles();
        }

        /// <summary>
        /// Renvoie le nom de la scène courante.
        /// </summary>
        public static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// C'est la méthode à appelé la première fois que l'on charge une scène ?
        /// </summary>
        public static void FirstApplicationLoad()
        {
            string playerSceneNameFilename = SceneServicesContainer.Instance.FileSystemConfiguration.SceneNameFilename;

            if (SerializerHelper.DoesCompletSavePathExists(playerSceneNameFilename, ".json"))
            {
                string lastPlayerSceneName = SerializerHelper.JsonDeserializeLoadWithEncryption<SerializableString>(SceneServicesContainer.Instance.FileSystemConfiguration.SceneNameFilename).Data;

                if (lastPlayerSceneName == "Don't Destroy Scene")
                    PlayerServicesAndModulesContainer.Instance.FirstLoadApplication();
                else
                    LoadScene(lastPlayerSceneName);
            }
        }
    }
}