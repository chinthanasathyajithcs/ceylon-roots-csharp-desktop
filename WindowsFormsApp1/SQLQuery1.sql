CREATE TABLE CustomerDelivery (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100),
    PassportNIC NVARCHAR(50),
    Email NVARCHAR(100),
    Mobile NVARCHAR(20),
    HotelName NVARCHAR(100),
    RoomNumber NVARCHAR(20),
    StreetAddress NVARCHAR(200),
    City NVARCHAR(50),
    NearestLandmark NVARCHAR(200),
    PreferredDeliveryDate DATE,
    LeaveAtReception BIT
);