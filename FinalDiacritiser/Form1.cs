using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace FinalDiacritiser
{
    public partial class Form1 : Form
    {
        string[] tags = { "P", "T", "NDG", "NDA", "NDN", "NUG", "NUA", "NUN", "P+NDG", "P+NUG", "VIM", "VPR", "VIJ", "VIS", "VID" };
        string[] reducedTags = { "P", "T", "ND", "NU", "P+ND", "P+NU", "VIM", "VPR", "VI" };

        Hashtable transliterate;
        char[] moonLetters = { 'A', '>', '<', '\'', '|', '{', '}', '&', 'b', 'g', 'H', 'j', 'k', 'w', 'x', 'f', 'E', 'q', 'y', 'm', 'h' };
        char[] letters = { '\'', '|', '>', '&', '<', '}', 'A', 'b', 'p', 't', 'v', 'j', 'H', 'x', 'd', '*', 'r', 'z', 's', '$', 'S', 'D', 'T', 'Z', 'E', 'g', 'f', 'q', 'k', 'l', 'm', 'n', 'h', 'w', 'Y', 'y', 'P', 'J', 'V', 'G', '{' };

        public Form1()
        {
            InitializeComponent();
            transliterate = new Hashtable();

            #region Buckwalter's Transliteration Table
            transliterate.Add("'", "ء");
            transliterate.Add("|", "آ");
            transliterate.Add(">", "أ");
            transliterate.Add("&", "ؤ");
            transliterate.Add("<", "إ");
            transliterate.Add("}", "ئ");
            transliterate.Add("A", "ا");
            transliterate.Add("b", "ب");
            transliterate.Add("p", "ة");
            transliterate.Add("t", "ت");
            transliterate.Add("v", "ث");
            transliterate.Add("j", "ج");
            transliterate.Add("H", "ح");
            transliterate.Add("x", "خ");
            transliterate.Add("d", "د");
            transliterate.Add("*", "ذ");
            transliterate.Add("r", "ر");
            transliterate.Add("z", "ز");
            transliterate.Add("s", "س");
            transliterate.Add("$", "ش");
            transliterate.Add("S", "ص");
            transliterate.Add("D", "ض");
            transliterate.Add("T", "ط");
            transliterate.Add("Z", "ظ");
            transliterate.Add("E", "ع");
            transliterate.Add("g", "غ");
            transliterate.Add("f", "ف");
            transliterate.Add("q", "ق");
            transliterate.Add("k", "ك");
            transliterate.Add("l", "ل");
            transliterate.Add("m", "م");
            transliterate.Add("n", "ن");
            transliterate.Add("h", "ه");
            transliterate.Add("w", "و");
            transliterate.Add("Y", "ى");
            transliterate.Add("y", "ي");
            transliterate.Add("P", "ب");
            transliterate.Add("J", "ج");
            transliterate.Add("V", "ف");
            transliterate.Add("G", "ق");

            transliterate.Add("¡", "،");
            transliterate.Add("¿", "؟");
            transliterate.Add("Ü", "ـ");
            transliterate.Add("º", "؛");

            //transliterate.Add("`", "\u0670");//not a letter
            transliterate.Add("`", "\u064E");//not a letter
            //transliterate.Add("{", "\u0671");//letter hamza al wasel
            transliterate.Add("{", "ا");//letter hamza al wasel
            transliterate.Add("_", "\u0640");//not a letter

            transliterate.Add("ال", "d");
            transliterate.Add("\u0650", "i");
            transliterate.Add("\u064E", "a");
            transliterate.Add("\u064F", "u");
            transliterate.Add("ج", "g");
            transliterate.Add("ن", "a");
            transliterate.Add("ر", "n");

            transliterate.Add("F", "\u064B");
            transliterate.Add("N", "\u064C");
            transliterate.Add("K", "\u064D");
            transliterate.Add("a", "\u064E");
            transliterate.Add("u", "\u064F");
            transliterate.Add("i", "\u0650");
            transliterate.Add("~", "\u0651");
            transliterate.Add("o", "\u0652");
            #endregion
        }

        private void updateProgress(string text, int value, Color color)
        {//Update the Progress Bar with the current status.
            progressLabel.Text = text;
            progressBar1.Value = value;
            progressBar1.ForeColor = color;
        }

        #region Event Handlers
        //These handlers are ordered based on the sequence of processes that are performed to diacritise the text.
        //Please read the comments on them in order to understand the process.
        private void intitialPOSBtn_Click(object sender, EventArgs e)
        {
            try
            {
                POSNgramBtn.Enabled = false;            //
                InitialDiacriticsBtn.Enabled = false;   //Disable prohibited buttons
                diacriticsNgramBtn.Enabled = false;     //
                initialPOS();//Run BAMA
                perl_Exited();//Parse BAMA output
                updateProgress("Done", 100, Color.Green);

                outputTextBox.Text = File.ReadAllText("temp.txt");//Show tag probabilities map
                outputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
                POSNgramBtn.Enabled = true;//Enable next stage button
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                updateProgress("Error!!", 100, Color.Red);
            }
        }

        private void POSNgramBtn_Click(object sender, EventArgs e)
        {
            try
            {
                POSNgramBtn.Enabled = false;//Disable prohibited buttons
                updateProgress("Running HMM POS Tagger", 50, Color.Green);
                POSNgram();//Run POS ngram on the probablities map
                updateProgress("Done", 100, Color.Green);
                InitialDiacriticsBtn.Enabled = true;

                outputTextBox.Text = File.ReadAllText("temp.txt");//Show tag sequence
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                updateProgress("Error!!", 100, Color.Red);
            }
        }

        private void InitialDiacriticsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                InitialDiacriticsBtn.Enabled = false;//Disable prohibited buttons
                updateProgress("Running BAMA", 25, Color.Green);
                initialDiacritics();//Run BAMA on input text
                updateProgress("Parsing BAMA Output", 50, Color.Green);
                perl_Exited1();//filter BAMA's results based on the POS tags from previous stage
                updateProgress("Parsing BAMA Output", 75, Color.Green);
                createBAMAMap();//Create probabilities map for the diacritisation N-gram
                updateProgress("Done", 100, Color.Green);
                diacriticsNgramBtn.Enabled = true;

                outputTextBox.Text = File.ReadAllText("temp.txt");//Show probabilities map
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                updateProgress("Error!!", 100, Color.Red);
            }
        }

        private void diacriticsNgramBtn_Click(object sender, EventArgs e)
        {
            try
            {
                updateProgress("Running the N-gram Diacritiser", 50, Color.Green);
                string res = diacriticsNgram((int)DiacNumericUpDown.Value);//Run ngram on probabilities map
                updateProgress("Parsing N-gram Output", 75, Color.Green);
                res = mergeOutput(res);//Parese and merge the output into the diacritised text
                updateProgress("Done", 100, Color.Green);

                res = postProcessResult(res);//Delete <unk> tags
                outputTextBox.Text = res;//Show the diacrtised text

                outputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                updateProgress("Error!!", 100, Color.Red);
            }
        }
        #endregion

        #region Initial POS
        private void initialPOS()
        {//Run BAMA (Buckwalter Morphological Analyser) on the input text.
            string text = inputTextBox.Text;
            text = preProcessBeforeBAMA(text);

            progressLabel.Text = "Checking Text!!";
            progressBar1.Value = 25;

            if (checkTextValidity(text))
            {
                File.WriteAllText("temp.txt", text, Encoding.GetEncoding("Windows-1256"));
                Process perl = new Process();
                perl = new Process();
                ProcessStartInfo perlStartInfo = new ProcessStartInfo("aramorph.bat");

                string d = System.Environment.CurrentDirectory;
                perlStartInfo.Arguments = "/k";
                perlStartInfo.UseShellExecute = false;
                perlStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                perlStartInfo.CreateNoWindow = false;
                perlStartInfo.RedirectStandardOutput = false;

                perl.StartInfo = perlStartInfo;
                perl.EnableRaisingEvents = true;

                try
                {
                    progressLabel.Text = "Running BAMA!!";
                    progressBar1.Value = 50;
                    perl.Start();

                    perl.WaitForExit();
                }
                catch (Exception ex)
                {
                    Exception e = new Exception("Ooops!!! The BAMA analyser did not work properly. Check if the input file is formatted correctly\n" + "Here is the exception details:\n" + ex.Message);
                    throw e;
                }
            }
            else
            {
                Exception e = new Exception("Illegal characters in text or text too long");
                throw e;
            }
        }

        private string preProcessBeforeBAMA(string text)
        {//This is only a simple preprocessing phase. In it we correct commen mistakes in writing on the web
            string res = Regex.Replace(text, "\'\'", "\"");
            res = Regex.Replace(res, "(\\p{P})", " $1 ");
            res = Regex.Replace(res, "  ", " ");
            return res;
        }

        private string postProcessResult(string text)
        {//Removes the <unk> tag from text. This tag resuls from an unrecognized punctuation in the diacritization n-gram
            return Regex.Replace(text, "<unk>", " ");
        }

        private bool checkTextValidity(string text)
        {// This function checks for the validity of input text and that it does not contain invalid characters

            Match m = Regex.Match(text, "[^ء-ي\\p{P}\u064B\u064C\u064D\u064E\u064F\u0650\u0651\u0652\\s]");
            if (m.Success)
            {
                string match = m.Value;
                inputTextBox.Select(m.Index, m.Length);
                return false;
            }

            if (text.Length > 5000)
                return false;

            return true;
        }

        private void perl_Exited()
        {// This function parses the BAMA results to generate a probability map input to the ngram POS tagger.
            string[] inputSentences = Regex.Split(File.ReadAllText("temp1.txt", Encoding.GetEncoding("Windows-1252")).Trim() + "\r\n", "[\\p{P}\\¡]\r\n");//Split the text into sentances buy looking for punctation
            string[] sep = { "\r\n\r\n" };
            StringBuilder output = new StringBuilder();//Result will be stored here

            for (int h = 0; h < inputSentences.Length; h++)//For each sentance
            {
                string[] words = inputSentences[h].Trim().Split(sep, StringSplitOptions.None);//Extract the words

                if (!(words.Length == 0 || (words.Length == 1 && words[0].Trim() == "")))//if this sentence is not empty
                {
                    output.Append("<s> *noevent*\r\n");
                    for (int i = 0; i < words.Length; i++)//For each word
                    {
                        string[] sep1 = { "\r\n" };
                        string[] options = words[i].Trim().Split(sep1, StringSplitOptions.None);//Get possible tags for the word
                        string optionsString = "w ";
                        List<string> optionsList = new List<string>();
                        if (options.Length >= 1 && options[0].Contains('/'))//if there is at least one possible tag for the word
                        {//Analyse the tag an generate a tag based on the above tagset that corresponds to buckwalter's tags
                            for (int j = 0; j < options.Length; j++)
                            {
                                bool P = false;
                                bool N = false;
                                bool D = false;
                                bool V = false;
                                bool I = false;
                                bool T = false;
                                bool A = false;
                                bool C = false;
                                bool DP = false;
                                string[] BAMATags = options[j].Trim().Split('+');
                                for (int k = 0; k < BAMATags.Length; k++)
                                {
                                    string curTag = BAMATags[k].Trim().Split('/')[1];
                                    if (curTag == "P" || curTag == "PREP")
                                        P = true;
                                    else if (curTag == "N" || curTag == "ADJ" || curTag == "NOUN" || curTag == "NOUN_PROP" || curTag == "PROP_NOUN")
                                        N = true;
                                    else if (curTag == "VPR" || curTag == "VERB_PERFECT")
                                        V = true;
                                    else if (curTag == "VI" || curTag == "VERB_IMPERFECT")
                                    {
                                        V = true;
                                        I = true;
                                    }
                                    else if (curTag == "VIM" || curTag == "VERB_IMPERATIVE")
                                        I = true;
                                    else if (curTag == "D" || curTag == "DET")
                                        D = true;
                                    else if (curTag == "ABBREV")
                                        A = true;
                                    else if (curTag == "CONJ")
                                        C = true;
                                    else
                                        T = true;
                                }

                                if (N)
                                {
                                    if (D)
                                        if (P)
                                            optionsList.Add("P+NDG");
                                        else
                                        {
                                            optionsList.Add("NDG");
                                            optionsList.Add("NDN");
                                            optionsList.Add("NDA");
                                        }
                                    else
                                        if (P)
                                            optionsList.Add("P+NUG");
                                        else
                                        {
                                            optionsList.Add("NUG");
                                            optionsList.Add("NUN");
                                            optionsList.Add("NUA");
                                        }
                                }
                                else if (V)
                                {
                                    if (I)
                                    {
                                        optionsList.Add("VIJ");
                                        optionsList.Add("VIS");
                                        optionsList.Add("VID");
                                    }
                                    else
                                        optionsList.Add("VPR");
                                }
                                else if (I)
                                {
                                    optionsList.Add("VIM");
                                }
                                else if (P)
                                {
                                    if (!T)
                                        optionsList.Add("P");
                                    else
                                        optionsList.Add("TT");
                                }
                                else if (A)
                                    optionsList.Add("A");
                                else
                                {
                                    optionsList.Add("T");
                                }
                            }

                            if (optionsList.Contains("A") && !optionsList.Contains("P"))
                                optionsList.Add("T");

                            optionsList = optionsList.Distinct().ToList();

                            optionsList.Remove("A");

                            int numOpt = optionsList.Count;

                            if (optionsList.Contains("TT"))//If the word contains a Perposition give this tag a higher probability
                                numOpt++;
                            if (optionsList.Contains("P"))//If the word contains a Perposition give this tag a higher probability
                                numOpt = numOpt + 2;

                            for (int j = 0; j < optionsList.Count; j++)
                            {
                                if (optionsList[j] == "P")
                                    optionsString += optionsList[j] + " " + (3.0 / numOpt).ToString() + " ";
                                else if (optionsList[j] == "TT")
                                    optionsString += "T " + (2.0 / numOpt).ToString() + " ";
                                else
                                    optionsString += optionsList[j] + " " + (1.0 / numOpt).ToString() + " ";
                            }

                            optionsString.Substring(0, optionsString.Length - 1);
                            optionsString += "\r\n";
                        }
                        else
                        {
                            for (int j = 0; j < tags.Length; j++)
                            {
                                if (tags[j] != "T" && tags[j] != "P")
                                    optionsString += tags[j] + " " + (1.0 / (tags.Length - 2)).ToString() + " ";
                            }
                            optionsString.Substring(0, optionsString.Length - 1);
                            optionsString += "\r\n";
                        }

                        output.Append(optionsString);
                    }
                    output.Append("</s> *noevent*\r\n");
                }
                else
                    output.Append("<s> *noevent*\r\n</s> *noevent*\r\n");
            }
            File.WriteAllText("temp.txt", output.ToString().Trim());
        }
        #endregion

        private delegate void SetTextCallback();

        #region POS Ngrams
        private void POSNgram()
        {//A POS ngram using a language model trained on the Quran.
         //This POS tagger chooses the most probable sequence of POS tags of the input text.
            progressBar1.Value = 50;
            progressLabel.Text = "Tagging Text!!";
            string text = ngram((int)POSNumericUpDown.Value);

            text = removeSentenceBoundries(text);

            File.WriteAllText("temp.txt", text);
        }

        private string ngram(int order)
        {//See last comment.
            Process ngram = new Process();
            ngram = new Process();
            ProcessStartInfo ngramStartInfo = new ProcessStartInfo("hidden-ngram.exe");
            ngramStartInfo.Arguments = "-lm POSNgrams/taggerNgram" + order.ToString() + ".txt -order " + order.ToString() + " -text-map temp.txt -hidden-vocab POSNgrams/vocab.txt";
            ngramStartInfo.UseShellExecute = false;
            ngramStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            ngramStartInfo.CreateNoWindow = false;
            ngramStartInfo.RedirectStandardOutput = true;
            ngram.StartInfo = ngramStartInfo;
            ngram.EnableRaisingEvents = true;

            ngram.Start();

            string res = ngram.StandardOutput.ReadToEnd();
            byte[] b = Encoding.GetEncoding("Windows-1252").GetBytes(res);
            res = Encoding.UTF8.GetString(b);
            ngram.WaitForExit();
            return res;
        }

        private string removeSentenceBoundries(string res)
        {//Trivial processing
            res = Regex.Replace(res, "</s>", "").Trim();
            res = Regex.Replace(res, "<s>", "");
            res = Regex.Replace(res, "\r\n", "*noevent*");
            res = Regex.Replace(res, "[ ]+", " ").Trim();
            res = Regex.Replace(res, "w ", "").Trim();

            return res;
        }
        #endregion

        #region Initial Diacritics
        private void initialDiacritics()
        {//Run BAMA to get the possible diacritcs for each word.
            string text = inputTextBox.Text;
            text = preProcessBeforeBAMA(text);
            File.WriteAllText("temp1.txt", text, Encoding.GetEncoding("Windows-1256"));

            if (checkTextValidity(text))
            {
                Process perl = new Process();
                perl = new Process();
                ProcessStartInfo perlStartInfo = new ProcessStartInfo("aramorphWithTags.bat");
                perlStartInfo.Arguments = "temp1.txt temp2.txt";
                perlStartInfo.UseShellExecute = false;
                perlStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                perlStartInfo.CreateNoWindow = false;

                perl.StartInfo = perlStartInfo;
                perl.EnableRaisingEvents = true;
                try
                {
                    perl.Start();
                    perl.WaitForExit();
                }
                catch (Exception ex)
                {
                    Exception e = new Exception("Ooops!!! The BAMA analyser did not work properly. Check if the input file is formatted correctly\n" + "Here is the exception details:\n" + ex.Message);
                    throw e;
                }
            }
            else
            {
                Exception e = new Exception("Invalid text input!!");
                throw e;
            }
        }

        private void perl_Exited1()
        {//Filter the possible dacritisation based on the POS tags.
            string[] sep = { "\r\n\r\n" };
            string[] taggedBAMA = File.ReadAllText("temp2.txt", Encoding.GetEncoding("Windows-1252")).Trim().Split(sep, StringSplitOptions.None);
            string[] taggedWords = File.ReadAllText("temp.txt").Trim().Split(' ');
            int tagsCount = 0;

            StringBuilder res = new StringBuilder();
            try
            {
                for (int i = 0; i < taggedBAMA.Length; i++)
                {
                    while (!Regex.IsMatch(taggedWords[tagsCount][0].ToString(), "[A-Z]") && tagsCount < taggedWords.Length - 1)
                    {
                        tagsCount++;
                    }

                    File.WriteAllText("tester.txt", i.ToString());

                    string[] sep1 = { "\r\n" };
                    string[] options = taggedBAMA[i].Trim().Split(sep1, StringSplitOptions.None);

                    res.Append(options[0] + "\r\n");

                    if (Regex.IsMatch(options[0][0].ToString(), "\\p{P}") && options[0].Length == 1)
                    {
                        res.Append("\r\n");
                        continue;
                    }

                    for (int j = 1; j < options.Length; j++)
                    {
                        string[] pair = options[j].Trim().Split(' ');
                        string stem;
                        bool li = false;
                        string tag = getTag(pair[1], out stem, out li);
                        if (tag[0] == taggedWords[tagsCount][0])
                        {
                            string temp = "";
                            if ((taggedWords[tagsCount].Length > 2 && taggedWords[tagsCount].Substring(0, 2) == "ND") || (taggedWords[tagsCount].Length > 4 && taggedWords[tagsCount].Substring(0, 4) == "P+ND"))
                            {
                                int lIndex = 0;
                                if (li)
                                    if (stem[0] == 'l')
                                        lIndex = 0;
                                    else
                                        lIndex = 1;
                                else
                                {
                                    lIndex = 0;
                                }
                                if (lIndex == 0)
                                {
                                    lIndex = pair[0].IndexOf('l');
                                    lIndex++;
                                    if (!letters.Contains(pair[0][lIndex]))
                                        lIndex++;
                                }
                                else
                                {
                                    lIndex = pair[0].IndexOf('l');
                                    lIndex = pair[0].IndexOf('l', lIndex + 1);
                                    lIndex++;
                                }
                                if (moonLetters.Contains(pair[0][lIndex]))
                                    temp = pair[0].Substring(0, lIndex) + "o" + pair[0].Substring(lIndex, pair[0].Length - lIndex);
                                else
                                    if (pair[0][lIndex + 1] == 'A')
                                        temp = pair[0].Substring(0, lIndex + 1) + "~a" + pair[0].Substring(lIndex + 1, pair[0].Length - lIndex - 1);
                                    else
                                        temp = pair[0].Substring(0, lIndex + 1) + "~" + pair[0].Substring(lIndex + 1, pair[0].Length - lIndex - 1);
                            }
                            else
                                temp = pair[0];
                            res.Append(temp);
                            if (letters.Contains(pair[0][pair[0].Length - 1]) || pair[0][pair[0].Length - 1] == '~')
                            {
                                if (taggedWords[tagsCount] == "NDG" || taggedWords[tagsCount] == "P+NDG" || taggedWords[tagsCount] == "NUG" || taggedWords[tagsCount] == "P+NUG")
                                    res.Append("i");
                                if (taggedWords[tagsCount] == "NUA" || taggedWords[tagsCount] == "NDA")
                                    res.Append("a");
                                if (taggedWords[tagsCount] == "NUN" || taggedWords[tagsCount] == "NDN")
                                    res.Append("u");
                            }
                            res.Append("\r\n");

                            res.Append(temp);

                            if (Regex.IsMatch(pair[0][pair[0].Length - 1].ToString(), "[ء-ي~]"))
                            {
                                if (taggedWords[tagsCount] == "NDG" || taggedWords[tagsCount] == "P+NDG" || taggedWords[tagsCount] == "NUG" || taggedWords[tagsCount] == "P+NUG")
                                    res.Append("K");
                                if (taggedWords[tagsCount] == "NUA" || taggedWords[tagsCount] == "NDA")
                                    res.Append("F");
                                if (taggedWords[tagsCount] == "NUN" || taggedWords[tagsCount] == "NDN")
                                    res.Append("N");
                            }
                            res.Append("\r\n");
                        }
                    }
                    res.Append("\r\n");
                    tagsCount++;
                }

                File.WriteAllText("temp.txt", res.ToString().Trim(), Encoding.GetEncoding("Windows-1252"));
            }
            catch (Exception ex)
            {
                Exception e = new Exception("Could not filter BAMA options using N-gram. " + ex.Message);
                throw e;
            }
        }

        private string getTag(string input, out string stem, out bool li)
        {//Reduce the BAMA tags to a tag from our tagset.
            //For example: katab/VERB_PERFECT+a/PVSUFF_SUBJ:3MS Becomes VPR
            //And Al/DET+waZiyf/NOUN+ap/NSUFF_FEM_SG            Becomes NDG or NDA or NDN
            bool P = false;
            bool N = false;
            bool D = false;
            bool V = false;
            bool I = false;
            bool T = false;
            bool A = false;
            bool C = false;
            bool DP = false;
            li = false;
            stem = "";
            try
            {
                string[] BAMATags = input.Trim().Split('+');
                for (int k = 0; k < BAMATags.Length; k++)
                {
                    string curTag = BAMATags[k].Trim().Split('/')[1];
                    if (curTag == "P" || curTag == "PREP")
                    {
                        if (BAMATags[k].Trim().Split('/')[0] == "li")
                            li = true;
                        P = true;
                    }
                    else if (curTag == "N" || curTag == "ADJ" || curTag == "NOUN" || curTag == "NOUN_PROP" || curTag == "PROP_NOUN")
                    {
                        stem = BAMATags[k].Trim().Split('/')[0];
                        N = true;
                    }
                    else if (curTag == "VPR" || curTag == "VERB_PERFECT")
                        V = true;
                    else if (curTag == "VI" || curTag == "VERB_IMPERFECT")
                    {
                        V = true;
                        I = true;
                    }
                    else if (curTag == "VIM" || curTag == "VERB_IMPERATIVE")
                        I = true;
                    else if (curTag == "D" || curTag == "DET")
                        D = true;
                    else if (curTag == "ABBREV")
                        A = true;
                    else if (curTag == "CONJ")
                        C = true;
                    else
                        T = true;
                }

                if (N)
                {
                    if (D)
                        if (P)
                            return "P+NDG";
                        else
                        {
                            return "N";
                        }
                    else
                        if (P)
                            return "P+NUG";
                        else
                        {
                            return "N";
                        }
                }
                else if (V)
                {
                    if (I)
                    {
                        return "V";
                    }
                    else
                        return "VPR";
                }
                else if (I)
                {
                    return "VIM";
                }
                else if (P)
                {
                    if (!T)
                        return "P";
                    else
                        return "T";
                }
                else if (A)
                    return "A";
                else
                {
                    return "T";
                }
            }
            catch (Exception ex)
            {
                Exception e = new Exception("Could no get tag from BAMA POS tags. " + ex.Message);
                throw e;
            }
        }

        private void createBAMAMap()
        {// This function parses the BAMA results to get the probabilities map for the diacritics (a probability for each diacritic).
            try
            {
                string text = System.IO.File.ReadAllText("temp.txt", Encoding.GetEncoding("Windows-1252"));
               
                StringBuilder result = new StringBuilder();

                for (int m = 0; m < text.Length; m++)
                {
                    if (text[m] != ' ' && text[m] != '\r' && text[m] != '\n')
                    {
                        if (transliterate[text[m].ToString()] != null && transliterate[text[m].ToString()] != "")
                            result.Append(transliterate[text[m].ToString()]);
                        else
                            result.Append(text[m]);
                    }
                    else
                        result.Append(text[m]);
                }

                text = result.ToString();
                result.Clear();

                string[] delimiters = { "\r\n\r\n" };
                string[] entries = text.Split(delimiters, StringSplitOptions.None);
                if (!(entries.Length == 1))
                {
                    for (int i = 0; i < entries.Length; i++)
                    {
                        string[] tempDelimiters = { "\r\n" };
                        string[] options = entries[i].Trim().Split(tempDelimiters, StringSplitOptions.None);

                        List<List<string>> reses = new List<List<string>>();
                        for (int gg = 0; gg < options[0].Length; gg++)
                            reses.Add(new List<string>());

                        if (options[0].Length == 1 && Char.IsPunctuation(options[0][0]))
                            result.Append(options[0] + " *noevent*\r\n");
                        else
                        {
                            for (int j = 1; j < options.Length; j++)
                            {
                                string[] curD = Regex.Split(options[j], "[ء-ي]");
                                if (curD.Length != options[0].Split(options[0].ToCharArray()).Length)
                                {
                                    continue;
                                }
                                for (int k = 0; k < curD.Length - 1; k++)
                                {
                                    if (curD[k + 1] != "")
                                        reses[k].Add("<" + curD[k + 1] + ">");
                                    else
                                        reses[k].Add("<u>");
                                }
                            }

                            for (int ii = 0; ii < reses.Count; ii++)
                                reses[ii] = reses[ii].Distinct().ToList();
                            for (int l = 0; l < options[0].Length; l++)
                            {
                                result.Append(options[0][l] + " ");
                                if (reses[l].Count != 0 && !(reses[l].Count == 1 && reses[l][0] == "<u>"))
                                    for (int m = 0; m < reses[l].Count; m++)
                                    {
                                        if (m < reses.Count - 1)
                                            result.Append(reses[l][m] + " " + (1.0 / reses[l].Count) + " ");
                                        else
                                            result.Append(reses[l][m] + " " + (1.0 / reses[l].Count));
                                    }
                                else
                                    if (l != options[0].Length - 1)
                                    {
                                        if (options[0][l] == 'ا')
                                            result.Append("<u> 1");
                                        else
                                            result.Append("<u> 0.07692307692 <\u064B> 0.07692307692 <\u064C> 0.07692307692 <\u064D> 0.07692307692 <\u064E> 0.15384615384 <\u064F> 0.07692307692 <\u0650> 0.07692307692 <\u0651> 0.07692307692 <\u0652> 0.07692307692 <\u0651\u0650> 0.07692307692 <\u0651\u064F> 0.07692307692 <\u0651\u064E> 0.07692307692");
                                    }
                                    else
                                    {
                                        if (options[0][l] == 'ا')
                                            result.Append("<u> 1");
                                        else
                                            result.Append("<u> 0.125 <\u064B> 0.125 <\u064C> 0.125 <\u064D> 0.125 <\u064E> 0.125 <\u064F> 0.125 <\u0650> 0.125 <\u0651> 0.125 <\u0652> 0.125");
                                    }

                                result.Append("\r\n");
                            }
                        }
                        result.Append("s *noevent*\r\n");
                    }
                    string temp = Regex.Replace(result.ToString(), "s \\*noevent\\*\r\n(\\p{P}) \\*noevent\\*\r\ns \\*noevent\\*", "$1 *noevent*");
                    temp = Regex.Replace(temp, "(\\p{P}) \\*noevent\\*\r\ns \\*noevent\\*", "$1 *noevent*");
                    result.Clear();
                    result.Append("<s> *noevent*\r\n" + temp.Trim() + "\r\n</s> *noevent*");
                    System.IO.File.WriteAllText("temp.txt", Regex.Replace(result.ToString(), "\u0670", "\u064E"));
                }
            }
            catch (Exception ex)
            {
                Exception e = new Exception("Could not create BAMA map for POS Tags. " + ex.Message);
                throw e;
            }
        }
        #endregion

        #region Diacritics Ngram
        private string diacriticsNgram(int order)
        {//Run an ngram with a language model trained on a fully diacritised text. this function perfoms the final diacrtisation.
            Process ngram = new Process();
            ngram = new Process();
            ProcessStartInfo ngramStartInfo = new ProcessStartInfo("hidden-ngram.exe");
            ngramStartInfo.Arguments = "-lm DiacNgrams/ngram" + order.ToString() + ".txt -order " + order.ToString() + " -text-map temp.txt -hidden-vocab DiacNgrams/vocab.txt";
            ngramStartInfo.UseShellExecute = false;
            ngramStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            ngramStartInfo.CreateNoWindow = false;
            ngramStartInfo.RedirectStandardOutput = true;
            ngram.StartInfo = ngramStartInfo;
            ngram.EnableRaisingEvents = true;

            ngram.Start();

            string res = ngram.StandardOutput.ReadToEnd();
            byte[] b = Encoding.GetEncoding("Windows-1252").GetBytes(res);
            res = Encoding.UTF8.GetString(b);
            ngram.WaitForExit();
            return res;
        }

        private string mergeOutput(string input)
        {//Trivial processing of the SRILM n-gram output.
            string output = "";
            output = Regex.Replace(input, "<s> ", "");
            output = Regex.Replace(output, " </s>", "");
            output = Regex.Replace(output, "</s>", "");
            output = Regex.Replace(output, @"[\s]", "");
            output = Regex.Replace(output, "s", " ");
            output = Regex.Replace(output, "<u>", "");
            output = Regex.Replace(output, "(.noevent.)", "");
            output = Regex.Replace(output, @"([\w])<([\u064B\u064C\u064D\u064E\u064F\u0650\u0651\u0652\u0670]+)>", "$1$2");
            output = Regex.Replace(output, "[\\s]*(\\p{P})[\\s]*", " $1 ");
            output = Regex.Replace(output, "\\s+", " ");
            return output;
        }
        #endregion
    }
}