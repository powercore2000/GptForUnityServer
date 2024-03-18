﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    #region Enums
    public enum ServerProtocolTypes
    {

        TCP,
        UDP,
        HTTP
    }

    public enum AiChatServiceTypes
    {
        /// <summary>
        /// Debug service type for testing and placeholder purposes only
        /// </summary>
        Mock,
        /// <summary>
        /// Makes calls to a generic Ai's Api directly. Must assign AiApiKey and AiApiUrl varables on start up in app settings. Should NOT be used on distributted software
        /// </summary>
        AiApi,

        /// <summary>
        /// Makes calls to Unity's Clode Code Api. Used as a template for other cloud service support
        /// </summary>
        UnityCloud,

        /// <summary>
        /// Makes calls to a local instance of Oobabooga's text-generation-webui Api. 
        /// </summary>
        OobaUi,

        /// <summary>
        /// Makes calls to a local instance of Kobold Ai's Api
        /// </summary>
        KoboldAi
    }

    public enum ClassificationServiceTypes
    {        
        Mock,
        SillyTavernExtras,
    }

    public enum ModelManagerServiceTypes
    {
        Mock,
        Cloud,
        AiApi,
    }

    public enum KeyValidationServiceTypes
    {
        Offline,
        AiApi,
        Cloud,
    }

    public enum PromptSettingServiceTypes
    {
        Default
    }
    #endregion

}
