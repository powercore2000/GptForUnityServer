using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharedLibrary
{
    [ComVisible(true)]
    public class PromptSettings
    {
        public string Model { get; set; } = "text-davinci-003";

        public PromptType PromptTypeEnum { get; set; }

        public string PromptTypeString => PromptTypeEnum.ToString();

        /// <summary>
        /// Additonal context messages the server prefixes to the prompt
        /// </summary>
        public string[] context_history { get; set; } = new string[] {};

        /// <summary>
        /// The full message sent to the Ai service, context and internal history included
        /// </summary>
        public string prompt { get; set; } = string.Empty;

        /// <summary>
        /// The message the user has sent
        /// </summary>
        public string user_message { get; set; } = string.Empty;

        public float temp { get; set; } = 1f;
        public int max_tokens { get; set; } = 50;
        public float top_p { get; set; } = 1f;
        public int frequency_penalty { get; set; } = 0;
        public int presence_penalty { get; set; } = 0;
        public int best_of { get; set; } = 0;
        public int top_k { get; set; } = 40;
        public int typical_p { get; set; } = 1;
        public int top_a { get; set; } = 1;
        public int tfs { get; set; } = 1;
        public int epsilon_cutoff { get; set; } = 0;
        public int eta_cutoff { get; set; } = 0;
        public double rep_pen { get; set; } = 1.2;
        public int no_repeat_ngram_size { get; set; } = 0;
        public int penalty_alpha { get; set; } = 0;
        public int num_beams { get; set; } = 1;
        public int length_penalty { get; set; } = 1;
        public int min_length { get; set; } = 0;
        public int encoder_rep_pen { get; set; } = 1;
        public bool do_sample { get; set; } = true;
        public bool early_stopping { get; set; } = false;
        public string[] stopping_strings { get; set; } = new string[] { "You:" };

        /*
        public void OverritePromptSettings(PromptSettings newSettings)
        {
            Model = newSettings.Model;
            prompt = newSettings.prompt;
            user_message = newSettings.user_message;
            temp = newSettings.temp;
            max_tokens = newSettings.max_tokens;
            top_p = newSettings.top_p;
            frequency_penalty = newSettings.frequency_penalty;
            presence_penalty = newSettings.presence_penalty;
            best_of = newSettings.best_of;
            top_k = newSettings.top_k;
            typical_p = newSettings.typical_p;
            top_a = newSettings.top_a;
            tfs = newSettings.tfs;
            epsilon_cutoff = newSettings.epsilon_cutoff;
            eta_cutoff = newSettings.eta_cutoff;
            rep_pen = newSettings.rep_pen;
            no_repeat_ngram_size = newSettings.no_repeat_ngram_size;
            penalty_alpha = newSettings.penalty_alpha;
            num_beams = newSettings.num_beams;
            length_penalty = newSettings.length_penalty;
            min_length = newSettings.min_length;
            encoder_rep_pen = newSettings.encoder_rep_pen;
            do_sample = newSettings.do_sample;
            early_stopping = newSettings.early_stopping;
            PromptTypeEnum = newSettings.PromptTypeEnum;

            context_history = newSettings.context_history;

        }
        */

    }


        public enum PromptType
    {

        Instruct,
        Chat,
        Image,
        Voice
    }
}
