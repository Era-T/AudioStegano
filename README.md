
### Universiteti i Prishtines 

### Drejtimi: Inxhinieri Kompjuterike

### Tema: Fshehja e informates ne audio file (.wav)

#### Gjuha programuese: C#

#### Profesor: Blerim Rexha

#### Asistent: Arbnor Halili

#### Studentet: Blerina Haziri, Era Tahiri, Fjoralba Krasniqi


### Abstrakt:
Audio Steganografia është e përqendruar në fshehjen e informacionit sekret në një fajll ose sinjal audio në mënyrë të sigurt.
Siguria dhe qëndrueshmëria e komunikimit janë jetike për transmetimin e informacionit për subjektet e autorizuara, ndërsa u mohohet hyrja atyre që nuk janë të autorizuar.
Duke ngulitur informacione sekrete duke përdorur një sinjal audio si mjet mbulimi, ekzistenca e informacionit sekret fshihet gjatë komunikimit.
Në një sistem steganografie audio të bazuar në kompjuter, mesazhet sekrete janë ngulitur në tingull dixhital.
Mesazhi sekret është ngulitur duke ndryshuar pak sekuencën binare të një skedari zanor.


Implementimi i programit tone eshte bere ne gjuhen programuese C#, duke u bazuar ne wave file layout sic shihet edhe ne figure. <br/>
http://soundfile.sapp.org/doc/WaveFormat/

Wave file format eshte nje nengrup i specifikimeve te RIFF per ruajtjen e skedareve multimedial. Nje RIFF file fillon me nje header dhe ndiqet nga nje sekuence e data chunks.
Pra, nje WAVE file eshte nje RIFF file qe e permban nje WAVE chunk te vetem i cili perbet nga dy subchunk (format sub-chunk dhe data sub-chunk), pershkrimi me i detajuar rreth chunks  gjindet ne linkun me larte.

Meqenese duhet pasur kujdes gjate fshehjes se informates ne nje audio file duhet te kemi parasysh qe te ndajme pjesen e header dhe pjesen e te dhenave (data), sepse pjesa e header perfshin disa specifika te wave formatit qe nuk duhet te ndryshohen ndersa pjesa e data perfshin raw soud data qe nese behet me kujdes nderrimi i bajtave mund te mos verehet qe eshte bere fshehja e informates ne ate wave file. <br/>
Nese deshirojme te bejme fshehjen e fjales "FIEK" atehere ajo duhet te enkodohet dhe qdo karakter e ka nje vlere te caktuar vleren e karaktereve e pjesetojme me 1000 (ne menyre qe ndryshimet te mos verehen) dhe ja shtojme tingullit origjinal, pra: (OriginalAudio + Characters = Audio with encodet data).
<br/>

Nxjerrja e informates: (Audio with encoded data - Original Audio = characters), ne kete rast karakteret i kemi numra me presje prandaj shumezohen me 1000 qe te fitohet vlera e sakte integer e secilit karakter dhe keshtu fitojme informaten qe e kemi fshehur ne audio file.





