using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BloodOfEvil.Utilities.UI
{
    /// <summary>
    /// Permet de changer l'image en fonction de l'état de notre case à coché.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ModifyImageToggleButton : AToggleButtonAction
    {
      #region Fields
      [SerializeField, Tooltip("C'est le sprite affiché lorsque l'état de notre case à coché est allumé.")]
      private Sprite onImage;
      [SerializeField, Tooltip("C'est le sprite affiché lorsque l'état de notre case à coché est éteind.")]
      private Sprite offImage;
      #endregion

      #region Override Behaviour
      protected override void ButtonToggleAction(bool isOn)
      {
          GetComponent<Image>().sprite = isOn ? 
              null != this.onImage ? this.onImage : null :
              null != this.offImage ? this.offImage : null;
      }
      #endregion
    }
}