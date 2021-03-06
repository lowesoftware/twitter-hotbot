USE [master]
GO

/****** Object:  Database [HotBot]    Script Date: 05/23/2014 01:51:17 ******/
CREATE DATABASE [HotBot] ON  PRIMARY 
( NAME = N'HotBot', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\HotBot.mdf' , SIZE = 4352KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HotBot_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\HotBot_log.LDF' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [HotBot] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HotBot].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [HotBot] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [HotBot] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [HotBot] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [HotBot] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [HotBot] SET ARITHABORT OFF 
GO

ALTER DATABASE [HotBot] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [HotBot] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [HotBot] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [HotBot] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [HotBot] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [HotBot] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [HotBot] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [HotBot] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [HotBot] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [HotBot] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [HotBot] SET  ENABLE_BROKER 
GO

ALTER DATABASE [HotBot] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [HotBot] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [HotBot] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [HotBot] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [HotBot] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [HotBot] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [HotBot] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [HotBot] SET  READ_WRITE 
GO

ALTER DATABASE [HotBot] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [HotBot] SET  MULTI_USER 
GO

ALTER DATABASE [HotBot] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [HotBot] SET DB_CHAINING OFF 
GO

