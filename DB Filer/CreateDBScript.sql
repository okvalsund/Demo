USE [master]
GO

/****** Object:  Database [Demo]    Script Date: 2017-05-14 22.55.14 ******/
CREATE DATABASE [Demo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DEMO', FILENAME = N'C:\demo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DEMO_log', FILENAME = N'C:\demo_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [Demo] SET COMPATIBILITY_LEVEL = 130
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Demo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Demo] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Demo] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Demo] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Demo] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Demo] SET ARITHABORT OFF 
GO

ALTER DATABASE [Demo] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Demo] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Demo] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Demo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Demo] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Demo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Demo] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Demo] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Demo] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Demo] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Demo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Demo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Demo] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Demo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Demo] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Demo] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Demo] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Demo] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Demo] SET  MULTI_USER 
GO

ALTER DATABASE [Demo] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Demo] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Demo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Demo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Demo] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Demo] SET QUERY_STORE = OFF
GO

USE [Demo]
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

ALTER DATABASE [Demo] SET  READ_WRITE 
GO

