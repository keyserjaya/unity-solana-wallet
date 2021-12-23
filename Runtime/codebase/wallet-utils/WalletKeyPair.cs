﻿#define MG_SCRIPTS

using dotnetstandard_bip39;
using Solnet.Wallet;
using System;
using System.Collections.Generic;

namespace AllArt.Solana
{
    public static class WalletKeyPair
    {
        public static string derivePath = "m/44'/501'/0'/0'";

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static string GenerateNewMnemonic()
        {
            dotnetstandard_bip39.BIP39 p = new dotnetstandard_bip39.BIP39();
#if MG_SCRIPTS
            string mnemonic = p.GenerateMnemonic(256, BIP39Wordlist.English, SolanaWalletHelper.GetWordlist(BIP39Wordlist.English));
#else
            string mnemonic = p.GenerateMnemonic(256, BIP39Wordlist.English);
#endif
            return mnemonic;
        }

        public static byte[] GetBIP39SeedBytes(string seed)
        {
            return StringToByteArrayFastest(MnemonicToSeedHex(seed));
        }

        public static string MnemonicToSeedHex(string seed)
        {

            dotnetstandard_bip39.BIP39 p = new dotnetstandard_bip39.BIP39();
            return p.MnemonicToSeedHex(seed, string.Empty);
        }

        public static byte[] GetBIP32SeedByte(byte[] seed)
        {
            Ed25519Bip32 bip = new Ed25519Bip32(seed);

            (byte[] key, byte[] chain) = bip.DerivePath(derivePath);
            return key;
        }

        public static byte[] GenerateSeedFromMnemonic(string mnemonic)
        {
            return GetBIP39SeedBytes(mnemonic);
        }

        public static Keypair GenerateKeyPairFromMnemonic(string mnemonics)
        {
            byte[] bip39seed = GetBIP39SeedBytes(mnemonics);

            byte[] finalSeed = GetBIP32SeedByte(bip39seed);
            (byte[] privateKey, byte[] publicKey) = Ed25519Extensions.EdKeyPairFromSeed(finalSeed);

            return new Keypair(publicKey, privateKey);
        }

        public static bool CheckMnemonicValidity(string mnemonic)
        {
            if (mnemonic.Split(' ').Length < 24)
                return false;
            return true;
        }

        public static void SaveKeyPair(Keypair keypair)
        {
            //save to playerPrefs for testing purposes
            //make sure to change later for production
        }

#if MG_SCRIPTS
        ///https://github.com/Kirbyrawr/dotnet-standard-bip39/issues/1#issuecomment-991901290
        private static string[] GetWordlist(BIP39Wordlist wordlist)
        {
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

            UnityEngine.TextAsset worldListResultsAsset = UnityEngine.Resources.Load<UnityEngine.TextAsset>("Wordlists/" + wordListFile);
            var fileContents = worldListResultsAsset.text;
            

            return fileContents.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            /*Modified the following code to make it work during runtime
            var wordListFile = wordlists[wordlist.ToString()];

            var wordListResults = UnityEngine.Resources.ResourceManager.GetString(wordListFile)
                .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return wordListResults;
            */
        }
#endif
    }
}
