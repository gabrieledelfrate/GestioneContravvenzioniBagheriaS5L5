# GestioneContravvenzioniBagheriaS5L5

QUERIES PER CREAZIONE DATABASE:

CREATE DATABASE GestioneContravvenzioniBagheria2;

CREATE TABLE ANAGRAFICA (
    IDAnagrafica  INT IDENTITY(1,1) PRIMARY KEY,
    Cognome VARCHAR(50),
    Nome VARCHAR(50),
    Indirizzo VARCHAR(100),
    Città VARCHAR(50),
    CAP VARCHAR(10),
    CodFisc VARCHAR(16)
);

CREATE TABLE TIPO_VIOLAZIONE (
    IDViolazione  INT IDENTITY(1,1) PRIMARY KEY,
    Descrizione VARCHAR(100)
);

CREATE TABLE AGENTI (
    IDAgente  INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(50),
    Cognome VARCHAR(50),
    Matricola VARCHAR(20),
    PasswordAgente VARCHAR(16)
);

CREATE TABLE VERBALE (
    IDVerbale  INT IDENTITY(1,1) PRIMARY KEY,
    DataViolazione DATE,
    IndirizzoViolazione VARCHAR(100),
    IDAgente INT,
    DataTrascrizioneVerbale DATE,
    Importo DECIMAL(10, 2),
    DecurtamentoPunti INT,
    IDAnagrafica INT,
    IDViolazione INT,
    FOREIGN KEY (IDAnagrafica) REFERENCES dbo.ANAGRAFICA(IDAnagrafica),
    FOREIGN KEY (IDViolazione) REFERENCES dbo.TIPO_VIOLAZIONE(IDViolazione),
    FOREIGN KEY (IDAgente) REFERENCES dbo.AGENTI(IDAgente)
);
