# ESAME LABORATORIO.

*Argomento: XML, 1 hour*

## PREMESSA

Apri e analizza i dati contenuti nei **sei file XML** forniti con questo documento. Si tratta di informazioni estratte dal sistema informativo di una banca e relative a filiali, clienti, conti correnti e movimenti effettuati da gennaio a ottobre 2026. 
Gli IBAN sono fittizi.

| **FILE** | **CONTENUTI** |
| --- | --- |
| **REGIONI.xml** | Le **20** Regioni italiane: ID (da 1 a 20), NOME REGIONE |
| **FILIALI.xml** | Le filiali della banca: ID, DENOMINAZIONE, CITTA, SIGLA_PROVINCIA, **CODICE** REGIONE |
| **CLIENTI.xml** | **700** anagrafiche: ID, COGNOME, NOME, SESSO, **DATA_NASCITA** (formato `aaaa-mm-gg`) |
| **CONTI.xml** | **600** conti: IBAN, ID_FILIALE, TIPO_CONTO (CORRENTE, RISPARMIO, DEPOSITO), DATA, **FIDO** concesso (in euro) e **SALDO_INIZILE** (saldo al 01/01/2026) |
| **CONTI\_INTESTATARI.xml** | Le “associazioni” tra conti e clienti: IBAN, ID_CLIENTE, RUOLO (TITOLARE oppure COINTESTATARIO). **Un conto ha un titolare ed eventualmente un cointestatario; un cliente può essere intestatario di più conti (oppure di nessuno).** |
| **MOVIMENTI.xml** | **7093** movimenti: ID, DATA (formato `gg/mm/aaaa`), ORARIO, IBAN, IMPORTO, CAUSALE, CANALE. Gli **importi positivi sono accrediti, quelli negativi sono addebiti**; il separatore decimale è il punto. I movimenti **non sono ordinati** per data. |

## VINCOLI

- Costruisci **UN SOLO** applicativo che soddisfi tutte le richieste (non un programma per ogni richiesta).
- Per gli importi usa il tipo `decimal` (non `double`) e fai attenzione alla cultura per la lettura dei numeri decimali.
- Il **SALDO_ATTUALEe** di un conto è il saldo iniziale più la somma di tutti i suoi movimenti.
- (\*) L’**ETA** va calcolata in modo **esatto** (tenendo conto se il compleanno è già avvenuto) rispetto alla **data in cui si utilizza l’applicativo**: non vanno usati valori costanti.
- Attenzione: i dati provengono da un sistema reale e potrebbero contenere incongruenze. Un buon applicativo non deve andare in errore.

## RICHIESTE

### RICHIESTA N. 1 – PUNTI 2,5

Determinare e comunicare a video le seguenti informazioni relative al movimento di **addebito di importo più elevato** (cioè con l’importo negativo più grande in valore assoluto):
DATA (**da esprimere nel formato gg-mm-aaaa**), ORARIO, IBAN, IMPORTO, CAUSALE, CANALE.
*Tutte queste informazioni si ricavano dal solo file MOVIMENTI.xml.*

### RICHIESTA N. 2 – PUNTI 2,0

Aggiungere a quanto già svolto (**RICHIESTA N. 1**) il **TIPO DI CONTO** e la **FILIALE** (denominazione, città e sigla della provincia) presso cui il conto è stato aperto.
*Sarà necessario utilizzare anche i file CONTI.xml e FILIALI.xml.*

### RICHIESTA N. 3 – PUNTI 1,5

Aggiungere a quanto già svolto (**RICHIESTE N. 1, 2**) il **NOME** della Regione in cui si trova la filiale.
*Sarà necessario utilizzare anche il file REGIONI.xml.*

### RICHIESTA N. 4 – PUNTI 3,0

Aggiungere a quanto già svolto (**RICHIESTE N. 1, 2, 3**) **COGNOME**, **NOME** ed **ETA** (\*) di **TUTTI gli INTESTATARI** del conto (TITOLARE ed eventuale COINTESTATARIO), indicando per ciascuno il RUOLO.
*Sarà necessario utilizzare anche i file CONTI\_INTESTATARI.xml e CLIENTI.xml.*

---

*PUNTEGGIO: la somma dei punti delle richieste (9,0) più 1 punto di base per la consegna di un elaborato funzionante.*
