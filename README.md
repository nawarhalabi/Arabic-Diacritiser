# Arabic-Diacritiser
Convert undiacritised Arabic text to diacritised Arabic text.
The interface is a simple .NET forms application written in C#. The diacritisation is done using simple ngrams built using SRILM toolkit (Please see their license on their website) and Buckwalter's morphological analyser.
SRILM is not included in the license here, the hidden-ngrams.exe file from the SRILM toolkit is committed with the project just to make it easy to deploy.

One can easily re-implement the ngram decoding to avoid depending on SRILM which makes the committed license cover all components.

## License
Please read the license file committed with the project.

## Dependencies
1- Perl: Please make sure that the path to Perl.exe is included in the PATH variable.
2- SRILM: The parts of the toolkit required are included with this commit so no need to worry here. Just note that they are not included in the license.
