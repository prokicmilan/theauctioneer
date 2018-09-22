USE [master]
GO

/****** Object:  Database [TheAuctioneer]    Script Date: 22-Sep-18 19:47:19 ******/
CREATE DATABASE [TheAuctioneer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TheAuctioneer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\TheAuctioneer.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TheAuctioneer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\TheAuctioneer_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [TheAuctioneer] SET COMPATIBILITY_LEVEL = 140
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TheAuctioneer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TheAuctioneer] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TheAuctioneer] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TheAuctioneer] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TheAuctioneer] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TheAuctioneer] SET ARITHABORT OFF 
GO

ALTER DATABASE [TheAuctioneer] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [TheAuctioneer] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TheAuctioneer] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TheAuctioneer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TheAuctioneer] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TheAuctioneer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TheAuctioneer] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TheAuctioneer] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TheAuctioneer] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TheAuctioneer] SET  DISABLE_BROKER 
GO

ALTER DATABASE [TheAuctioneer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TheAuctioneer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TheAuctioneer] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TheAuctioneer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TheAuctioneer] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TheAuctioneer] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TheAuctioneer] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TheAuctioneer] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [TheAuctioneer] SET  MULTI_USER 
GO

ALTER DATABASE [TheAuctioneer] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TheAuctioneer] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TheAuctioneer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TheAuctioneer] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [TheAuctioneer] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [TheAuctioneer] SET QUERY_STORE = OFF
GO

ALTER DATABASE [TheAuctioneer] SET  READ_WRITE 
GO


