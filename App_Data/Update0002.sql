begin tran T1

CREATE TABLE News
(
	NewsId int IDENTITY(1, 1),
	CorpId int null,
	AllianceId int null,
	NewsTitle nvarchar(50) not null,
	NewsText nvarchar(Max) not null,
	NewsDate datetime not null default(getdate()),
	VisibleToDate datetime null,
	Active bit not null default('1')
	CONSTRAINT PK_News PRIMARY KEY (NewsId)
)
go

commit tran T1