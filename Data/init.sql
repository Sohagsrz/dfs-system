-- Create database if not exists
CREATE DATABASE IF NOT EXISTS scash_db;
USE scash_db;

-- Create Users table
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    PIN VARCHAR(100) NOT NULL,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber VARCHAR(20) NOT NULL UNIQUE,
    Role INT NOT NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    DailyTransactionLimit DECIMAL(18,2) NOT NULL DEFAULT 1000.00,
    MonthlyTransactionLimit DECIMAL(18,2) NOT NULL DEFAULT 10000.00,
    FailedLoginAttempts INT NOT NULL DEFAULT 0,
    LastFailedLogin DATETIME,
    LastLogin DATETIME,
    IsLocked BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    QRCode VARCHAR(100),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME
);

-- Create Transactions table
CREATE TABLE IF NOT EXISTS Transactions (
    TransactionId INT AUTO_INCREMENT PRIMARY KEY,
    SenderId INT NOT NULL,
    RecipientId INT,
    Amount DECIMAL(18,2) NOT NULL,
    Fee DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Commission DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Type ENUM('SendMoney', 'CashIn', 'CashOut', 'MerchantPayment', 'BankWithdrawal', 
              'MobileRecharge', 'UtilityBillPayment', 'RequestMoney', 'QRPayment', 'Commission') NOT NULL,
    Status ENUM('Pending', 'Completed', 'Failed', 'Cancelled') NOT NULL,
    ReferenceNumber VARCHAR(50) NOT NULL UNIQUE,
    Description TEXT,
    UtilityType ENUM('Electricity', 'Gas', 'Water', 'Internet'),
    CreatedAt DATETIME NOT NULL,
    CompletedAt DATETIME,
    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (RecipientId) REFERENCES Users(Id)
);

-- Create TransactionHistory table
CREATE TABLE IF NOT EXISTS TransactionHistories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    TransactionId INT NOT NULL,
    ReferenceNumber VARCHAR(50) NOT NULL,
    Type ENUM('SendMoney', 'CashIn', 'CashOut', 'MerchantPayment', 'BankWithdrawal', 
              'MobileRecharge', 'UtilityBillPayment', 'RequestMoney', 'QRPayment', 'Commission') NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    Description TEXT,
    CreatedAt DATETIME NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId)
);

-- Create SystemSettings table
CREATE TABLE IF NOT EXISTS SystemSettings (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    `Key` VARCHAR(50) NOT NULL UNIQUE,
    Value TEXT NOT NULL,
    Description TEXT,
    LastUpdated DATETIME NOT NULL
);

-- Insert default system settings
INSERT INTO SystemSettings (`Key`, Value, Description, LastUpdated) VALUES
-- Transaction Fees
('SendMoneyFee', '0.50', 'Fee for sending money', NOW()),
('CashOutFee', '1.00', 'Fee for cash out transactions', NOW()),
('MerchantPaymentFee', '0.75', 'Fee for merchant payments', NOW()),
('MobileRechargeFee', '0.25', 'Fee for mobile recharge', NOW()),
('UtilityBillFee', '0.50', 'Fee for utility bill payments', NOW()),
('TransactionFee', '0.50', 'Default transaction fee', NOW()),
('QRPaymentFee', '0.25', 'Fee for QR code payments', NOW()),

-- Commission Rates
('AgentCashInCommission', '0.50', 'Commission for cash in transactions', NOW()),
('AgentCashOutCommission', '0.75', 'Commission for cash out transactions', NOW()),
('AgentCommission', '0.50', 'Default agent commission rate', NOW()),

-- Transaction Limits
('PersonalDailyLimit', '1000.00', 'Daily transaction limit for personal accounts', NOW()),
('PersonalMonthlyLimit', '10000.00', 'Monthly transaction limit for personal accounts', NOW()),
('MerchantDailyLimit', '5000.00', 'Daily transaction limit for merchant accounts', NOW()),
('MerchantMonthlyLimit', '50000.00', 'Monthly transaction limit for merchant accounts', NOW()),
('AgentDailyLimit', '10000.00', 'Daily transaction limit for agent accounts', NOW()),
('AgentMonthlyLimit', '100000.00', 'Monthly transaction limit for agent accounts', NOW()),

-- Security Settings
('MaxFailedLoginAttempts', '3', 'Maximum number of failed login attempts', NOW()),
('AccountLockoutDuration', '30', 'Account lockout duration in minutes', NOW()),
('PINChangeInterval', '90', 'PIN change interval in days', NOW()),
('SessionTimeout', '30', 'Session timeout in minutes', NOW()),

-- System Settings
('SystemName', 'sCash', 'Name of the system', NOW()),
('SupportPhone', '+1234567890', 'Support phone number', NOW()),
('SupportEmail', 'support@scash.com', 'Support email address', NOW()),
('MaintenanceMode', 'false', 'System maintenance mode status', NOW());

-- Create default admin user (password: admin123)
INSERT INTO Users (Username, Password, PIN, FullName, Email, PhoneNumber, Role, Balance, CreatedAt)
VALUES ('admin', '$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 
        '$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy',
        'System Administrator', 'admin@scash.com', '+1234567890', 1, 0.00, NOW()); 