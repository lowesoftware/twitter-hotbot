USE [HotBot]
GO

/****** Object:  Table [dbo].[Message]    Script Date: 05/23/2014 01:52:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Message](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[FriendId] [int] NOT NULL,
	[Hash] [nvarchar](64) NOT NULL,
	[Published] [datetime] NOT NULL,
	[Body] [nvarchar](1024) NOT NULL,
	[IsMatch] [bit] NULL,
	[MatchScore] [float] NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Friend] FOREIGN KEY([FriendId])
REFERENCES [dbo].[Friend] ([FriendId])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Friend]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Job]
GO

