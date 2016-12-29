USE [RiskCaptureDemo.Documents]
GO

CREATE table SagaState
	(SagaId varchar(100) NOT NULL,
	 SagaState varchar(max) not null,
	 SagaStateType varchar(100) not null,
	 PRIMARY KEY (SagaId, SagaStateType))
	 
go

CREATE PROCEDURE 
		[dbo].[sproc_insert_saga_state] 

		@SagaId 		as varchar(100),
		@SagaState 		as Varchar(max),
		@SagaStateType  as Varchar(100)
AS

INSERT INTO SagaState
	(SagaId,SagaState,SagaStateType)
values	
	(@SagaId,@SagaState,@SagaStateType)
	
go

CREATE PROCEDURE 
		[dbo].[sproc_update_saga_state] 

		@SagaId 		as varchar(100),
		@SagaState 		    as  Varchar(max),
		@SagaStateType  as Varchar(100)
AS

UPDATE SagaState
Set
	SagaState = @SagaState
where
	SagaId=@SagaId
	and SagaStateType=@SagaStateType
	
go

CREATE PROCEDURE 
		[dbo].[sproc_delete_saga_state] 

		@SagaId 		as varchar(100),
		@SagaStateType  as Varchar(100)
AS

delete from SagaState
where sagaid=@SagaId
and SagaStateType=@SagaStateType
	
go

CREATE PROCEDURE 
		[dbo].[sproc_get_saga_state] 

		@SagaId 		as Varchar(100),
		@SagaStateType  as Varchar(100)
				
AS

select SagaState from SagaState
where sagaid=@SagaId
and SagaStateType=@SagaStateType
go

CREATE TABLE [dbo].[Document](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataCaptureId] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Template](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductLine] [varchar](50) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [RiskCaptureDemo.Documents].[dbo].[Template]([ProductLine],[Text])
VALUES
('PM','TestDocument: Address 1:{{ProposerPolicyholder_AddressLine1}}, Address2: {{ProposerPolicyholder_AddressLine2}}'),
('PM','TestDocument2: Address 1:{{ProposerPolicyholder_AddressLine1}}, Address2: {{ProposerPolicyholder_AddressLine2}}, Address3: {{ProposerPolicyholder_AddressLine3}}')

GO