-- Creates psPeriodMonths to define partition filegroups.
IF NOT EXISTS (SELECT NULL FROM sys.partition_schemes WHERE name = 'psPeriodMonths')
CREATE PARTITION SCHEME [psPeriodMonths] AS PARTITION [pfPeriodMonths] ALL TO ([PRIMARY])
