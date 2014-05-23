USE [HotBot]
GO

/****** Object:  Table [dbo].[Response]    Script Date: 05/23/2014 01:52:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Response](
	[ResponseId] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[Text] [nvarchar](1024) NOT NULL,
	[Weight] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Response] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Response]  WITH CHECK ADD  CONSTRAINT [FK_Response_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Response] CHECK CONSTRAINT [FK_Response_Job]
GO

ALTER TABLE [dbo].[Response] ADD  CONSTRAINT [DF_Response_Weight]  DEFAULT ((50)) FOR [Weight]
GO

ALTER TABLE [dbo].[Response] ADD  CONSTRAINT [DF_Response_Active]  DEFAULT ((1)) FOR [Active]
GO

