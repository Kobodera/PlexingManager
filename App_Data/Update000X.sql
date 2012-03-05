begin tran T1

ALTER TABLE Plexes
ADD CorpId int null
Go

declare @CorpId as int

SELECT Top(1) @CorpId = CorpId FROM PlexUsers WHERE CharacterId in (SELECT Top(1) CharacterId FROM PlexUserRoles)

UPDATE Plexes SET CorpId = @CorpId;

ALTER TABLE PlexingPeriods
ADD CorpId int NULL
Go

declare @CorpId as int

SELECT Top(1) @CorpId = CorpId FROM PlexUsers WHERE CharacterId in (SELECT Top(1) CharacterId FROM PlexUserRoles)

UPDATE PlexingPeriods SET CorpId = @CorpId;

ALTER TABLE PlexingPeriods
ALTER COLUMN CorpId int NOT NULL
Go

ALTER TABLE Plexes
ALTER COLUMN CorpId int NOT NULL
Go

ALTER Table PlexInfos
ALTER COLUMN Points int not null
Go

ALTER Table PlexInfos
ALTER COLUMN Name nvarchar(200) not null
Go

Alter Table PlexInfos
ADD CorpId int null
Go

ALTER Table PlexUsers
ALTER COLUMN CharacterName nvarchar(200) not null
Go

ALTER Table PlexUsers
ALTER COLUMN [Password] nvarchar(200) not null
Go

ALTER Table PlexUsers
ALTER COLUMN CorpId int not null
Go

ALTER Table PlexUsers
ALTER COLUMN AllianceId int not null
Go

ALTER Table PlexUsers
ALTER COLUMN AllianceName nvarchar(200) not null
Go

UPDATE PlexUsers SET [Enabled] = 1 WHERE [Enabled] IS NULL;

ALTER Table PlexUsers
ALTER COLUMN Enabled bit not null
Go

CREATE TABLE Corps
(
	CorpId int not null,
	CorpName nvarchar(200) not null,
	CorpTag nvarchar(10) not null DEFAULT '',
	AllianceId int not null,
	AllianceName nvarchar(200) not null,
	AllianceTag nvarchar(10) not null DEFAULT '',
	Enabled bit not null DEFAULT '0',
	CONSTRAINT PK_Corps PRIMARY KEY (CorpId)
)
go

INSERT INTO Corps (CorpId, CorpName, AllianceId, AllianceName)
SELECT DISTINCT CorpId, CorpName, AllianceId, AllianceName
FROM PlexUsers
WHERE Enabled = 1

SELECT * FROM Corps

commit tran T1