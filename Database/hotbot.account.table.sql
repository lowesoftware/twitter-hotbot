USE [HotBot]
GO

/****** Object:  Table [dbo].[Account]    Script Date: 05/23/2014 01:51:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Token] [nvarchar](200) NOT NULL,
	[TokenSecret] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Job]
GO

