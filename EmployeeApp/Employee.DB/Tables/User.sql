CREATE TABLE [dbo].[User]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,  
    [Name] [varchar](50) NOT NULL,
    [Contact] [varchar](50) NOT NULL,  
    [Email] [varchar](50) NOT NULL,  
    [Address] [varchar](250) NULL,  
    [Gender] [varchar](250) NULL,  
PRIMARY KEY CLUSTERED   
(  
    [Id] ASC  
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]  
) ON [PRIMARY]  
