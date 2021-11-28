USE [master]
GO
/****** Object:  Database [Bank]    Script Date: 11/28/2021 1:59:43 PM ******/
CREATE DATABASE [Bank]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Bank', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Bank.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Bank_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Bank_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Bank] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Bank].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Bank] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Bank] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Bank] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Bank] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Bank] SET ARITHABORT OFF 
GO
ALTER DATABASE [Bank] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Bank] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Bank] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Bank] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Bank] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Bank] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Bank] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Bank] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Bank] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Bank] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Bank] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Bank] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Bank] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Bank] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Bank] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Bank] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Bank] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Bank] SET RECOVERY FULL 
GO
ALTER DATABASE [Bank] SET  MULTI_USER 
GO
ALTER DATABASE [Bank] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Bank] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Bank] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Bank] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Bank] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Bank] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Bank', N'ON'
GO
ALTER DATABASE [Bank] SET QUERY_STORE = OFF
GO
USE [Bank]
GO
/****** Object:  Table [dbo].[LoginHistory]    Script Date: 11/28/2021 1:59:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginHistory](
	[OId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Created_at] [datetime] NOT NULL,
	[Created_by] [nvarchar](64) NOT NULL,
	[Updated_at] [datetime] NULL,
	[Updated_by] [nvarchar](64) NULL,
 CONSTRAINT [PK_LoginHistory] PRIMARY KEY CLUSTERED 
(
	[OId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 11/28/2021 1:59:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[OId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [decimal](19, 2) NOT NULL,
	[Remark] [nvarchar](64) NOT NULL,
	[Type] [nvarchar](64) NOT NULL,
	[Created_at] [datetime] NOT NULL,
	[Created_by] [nvarchar](64) NOT NULL,
	[Updated_at] [datetime] NULL,
	[Updated_by] [nvarchar](64) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[OId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/28/2021 1:59:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[OId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Address] [nvarchar](128) NULL,
	[Gender] [nvarchar](64) NULL,
	[Phone] [nvarchar](64) NOT NULL,
	[Email] [nvarchar](64) NOT NULL,
	[Password] [nvarchar](64) NOT NULL,
	[Balance] [decimal](19, 2) NULL,
	[Created_at] [datetime] NOT NULL,
	[Created_by] [nvarchar](64) NOT NULL,
	[Updated_at] [datetime] NULL,
	[Updated_by] [nvarchar](64) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[OId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[LoginHistory] ON 

INSERT [dbo].[LoginHistory] ([OId], [UserId], [DateTime], [Created_at], [Created_by], [Updated_at], [Updated_by]) VALUES (3, 1, CAST(N'2021-11-28T12:57:04.000' AS DateTime), CAST(N'2021-11-28T12:57:04.000' AS DateTime), N'Azad', NULL, NULL)
SET IDENTITY_INSERT [dbo].[LoginHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([OId], [UserId], [Name], [Date], [Amount], [Remark], [Type], [Created_at], [Created_by], [Updated_at], [Updated_by]) VALUES (1, 1, N'Azad', CAST(N'2021-11-28T12:57:21.000' AS DateTime), CAST(100.00 AS Decimal(19, 2)), N'Gift', N'Diposit', CAST(N'2021-11-28T12:57:21.000' AS DateTime), N'Azad', NULL, NULL)
INSERT [dbo].[Transaction] ([OId], [UserId], [Name], [Date], [Amount], [Remark], [Type], [Created_at], [Created_by], [Updated_at], [Updated_by]) VALUES (3, 1, N'Ador', CAST(N'2021-11-28T13:01:37.000' AS DateTime), CAST(50.00 AS Decimal(19, 2)), N'Other', N'Withdraw', CAST(N'2021-11-28T13:01:37.000' AS DateTime), N'Ador', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([OId], [Name], [Address], [Gender], [Phone], [Email], [Password], [Balance], [Created_at], [Created_by], [Updated_at], [Updated_by]) VALUES (1, N'Azad', N'Dhaka -1207', N'Male', N'0123456789', N'azad@bank.com', N'Azad123456', CAST(150.00 AS Decimal(19, 2)), CAST(N'2021-11-28T12:50:11.000' AS DateTime), N'Azad', CAST(N'2021-11-28T13:01:37.000' AS DateTime), N'Ador')
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[LoginHistory]  WITH CHECK ADD  CONSTRAINT [FK_LoginHistory_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([OId])
GO
ALTER TABLE [dbo].[LoginHistory] CHECK CONSTRAINT [FK_LoginHistory_User]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([OId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_User]
GO
USE [master]
GO
ALTER DATABASE [Bank] SET  READ_WRITE 
GO
