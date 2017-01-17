﻿using UnityEngine;
using System.Collections;

namespace BloodOfEvil.Utilities
{
    /// <summary>
    /// Gère les singletons de type MonoBehaviour et leurs différentes érreurs potentielles.
    /// </summary>
    public class ASingletonMonoBehaviour<TSingletonType> : MonoBehaviour
                 where TSingletonType : ASingletonMonoBehaviour<TSingletonType>
    {
        #region Fields
        private static TSingletonType instance;
        private static bool haveBeenInitialized;
        #endregion

        #region Properties
        public static TSingletonType Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = (TSingletonType)FindObjectOfType(typeof(TSingletonType));


                    if (FindObjectsOfType(typeof(TSingletonType)).Length > 1)
                    {
                        Debug.LogErrorFormat("[ASingletonMonoBehaviour] : le singleton de type {0} éxiste en plusieurs éxemplaires ce qui n'est pas logique.",
                                             typeof(TSingletonType).Name);

                        return instance;
                    }

                    else if (instance == null)
                        Debug.LogErrorFormat("[ASingletonMonoBehaviour] : le singleton de type {0} a une instance de valeur null.",
                                             typeof(TSingletonType).Name);

                    else
                        instance.Initialize();
                }

                return instance;
            }
        }
        #endregion

        #region Virtual Behaviour
        /// <summary>
        /// La méthode d'initialisation de notre singleton.
        /// </summary>
        public virtual void Initialize()
        {
            if (haveBeenInitialized)
                Debug.LogErrorFormat("[ASingletonMonoBehaviour] : le singleton de type {0} a une instance déjà initialisée.",
                                     typeof(TSingletonType).Name);

            haveBeenInitialized = true;
        }
        #endregion
    }
}