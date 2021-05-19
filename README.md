# AudioStegano
# Universiteti i Prishtines 

## Drejtimi: Inxhinieri Kompjuterike

### Tema: Fshehja e informates ne audio file (.wav)

#### Gjuha programuese: C#

#### Profesori: Blerim Rexha

#### Asistenti: Arbnor Halili

#### Studentet: Blerina Haziri, Era Tahiri, Fjoralba Krasniqi


### Abstrakt:
Audio Steganografia është e përqendruar në fshehjen e informacionit sekret në një fajll ose sinjal audio në mënyrë të sigurt.
Siguria dhe qëndrueshmëria e komunikimit janë jetike për transmetimin e informacionit për subjektet e autorizuara, ndërsa u mohohet hyrja atyre që nuk janë të lejuara.
Duke ngulitur informacione sekrete duke përdorur një sinjal audio si mjet mbulimi, ekzistenca e informacionit sekret fshihet gjatë komunikimit.
Në një sistem steganografie audio të bazuar në kompjuter, mesazhet sekrete janë ngulitur në tingull dixhital.
Mesazhi sekret është ngulitur duke ndryshuar pak sekuencën binare të një skedari zanor.
Softueri ekzistues i steganografisë audio mund të vendosë mesazhe në skedarë zanorë WAV, AU, madje edhe MP3 por ne programin tone fshehja e informates eshte bere ne wav file.


### Implementimi:
Implementimi i programit tone eshte bere ne gjuhen programuese C#. Siq shihet me poshte ky eshte layout-i i nje wav file. <br/>
![picture](http://www.av-rd.com/knowhow/data/img/wav-sound-format.png)

RIFF chunk descriptor pershkruan informacione per ID-ne, madhesine dhe formatin prandaj nuk duhet te ndryshohet dhe per kete arsye ndahet nga data chunk, pjese ne te cilen ne po e ndryshojme dhe po fshehim informaten tone. Data chunk eshte pjese ne te cilen ndodhen raw sound information.

### Algoritmi:
* Leximi e wav file 
* Tek pjesa e data ruajme mesazhin tone shkronjat e te cilit nga ASCII i konvertojme ne byte, e pastaj vleren e tyre e pjestojme me numrin 1000 per shkak qe te mos verehet ndryshimi i bere nga mesazhi ne wav file-in tone
* Behet leximi i wav file te ndryshuar



