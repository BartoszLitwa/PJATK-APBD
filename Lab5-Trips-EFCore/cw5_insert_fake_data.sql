USE trip;

-- Insert data into the Client table
INSERT INTO trip.Client (IdClient, FirstName, LastName, Email, Telephone, Pesel) VALUES
(1, 'John', 'Doe', 'john.doe@example.com', '123456789', '12345678901'),
(2, 'Jane', 'Smith', 'jane.smith@example.com', '987654321', '98765432101'),
(3, 'Michael', 'Johnson', 'michael.johnson@example.com', '123123123', '12312312301'),
(4, 'Emily', 'Davis', 'emily.davis@example.com', '321321321', '32132132101');

-- Insert data into the Trip table
INSERT INTO trip.Trip (IdTrip, Name, Description, DateFrom, DateTo, MaxPeople) VALUES
(1, 'Trip to Paris', 'A wonderful trip to Paris', '2023-06-01', '2023-06-10', 20),
(2, 'Trip to London', 'A lovely trip to London', '2023-07-01', '2023-07-10', 15),
(3, 'Trip to New York', 'An exciting trip to New York', '2023-08-01', '2023-08-10', 25);

-- Insert data into the Country table
INSERT INTO trip.Country (IdCountry, Name) VALUES
(1, 'France'),
(2, 'United Kingdom'),
(3, 'United States');

-- Insert data into the Country_Trip table
INSERT INTO trip.Country_Trip (IdCountry, IdTrip) VALUES
(1, 1), -- France -> Trip to Paris
(2, 2), -- United Kingdom -> Trip to London
(3, 3); -- United States -> Trip to New York

-- Insert data into the Client_Trip table
INSERT INTO trip.Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate) VALUES
(1, 1, '2023-05-01', '2023-05-15'), -- John Doe -> Trip to Paris
(2, 2, '2023-06-01', '2023-06-15'), -- Jane Smith -> Trip to London
(3, 3, '2023-07-01', NULL), -- Michael Johnson -> Trip to New York (not paid yet)
(4, 1, '2023-05-02', '2023-05-16'); -- Emily Davis -> Trip to Paris
