USE [HotBot]
GO

/****** Object:  Table [dbo].[MessageResponse]    Script Date: 05/23/2014 01:52:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MessageResponse](
	[MessageResponseId] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [int] NOT NULL,
	[ResponseId] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_MessageResponse] PRIMARY KEY CLUSTERED 
(
	[MessageResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MessageResponse]  WITH CHECK ADD  CONSTRAINT [FK_MessageResponse_Message] FOREIGN KEY([MessageId])
REFERENCES [dbo].[Message] ([MessageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[MessageResponse] CHECK CONSTRAINT [FK_MessageResponse_Message]
GO

ALTER TABLE [dbo].[MessageResponse]  WITH CHECK ADD  CONSTRAINT [FK_MessageResponse_Response] FOREIGN KEY([ResponseId])
REFERENCES [dbo].[Response] ([ResponseId])
GO

ALTER TABLE [dbo].[MessageResponse] CHECK CONSTRAINT [FK_MessageResponse_Response]
GO

