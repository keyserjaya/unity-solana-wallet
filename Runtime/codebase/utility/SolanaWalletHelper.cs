using dotnetstandard_bip39;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SolanaWalletHelper {

    public static string[] GetWordlist(BIP39Wordlist wordlist) {
        var wordlists = new Dictionary<string, string>
            {
                {BIP39Wordlist.ChineseSimplified.ToString(), "chinese_simplified"},
                {BIP39Wordlist.ChineseTraditional.ToString(), "chinese_traditional"},
                {BIP39Wordlist.English.ToString(), "english"},
                {BIP39Wordlist.French.ToString(), "french"},
                {BIP39Wordlist.Italian.ToString(), "italian"},
                {BIP39Wordlist.Japanese.ToString(), "japanese"},
                {BIP39Wordlist.Korean.ToString(), "korean"},
                {BIP39Wordlist.Spanish.ToString(), "spanish"}
            };

        var wordListFile = wordlists[wordlist.ToString()];

        TextAsset worldListResultsAsset = UnityEngine.Resources.Load<TextAsset>("Wordlists/" + wordListFile);
        var fileContents = worldListResultsAsset.text;


        return fileContents.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        /*Modified the following code to make it work during runtime
        var wordListFile = wordlists[wordlist.ToString()];

        var wordListResults = UnityEngine.Resources.ResourceManager.GetString(wordListFile)
            .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        return wordListResults;
        */
    }
}
