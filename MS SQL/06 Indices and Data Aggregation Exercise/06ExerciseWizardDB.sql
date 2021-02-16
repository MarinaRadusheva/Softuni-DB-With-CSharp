--Import the database and send the total count of records from the one and only table to Mr. Bodrog. Make sure nothing got lost.

SELECT COUNT(*) AS [Count]FROM WizzardDeposits

-- Select the size of the longest magic wand. Rename the new column appropriately. LongestMagicWand
SELECT MAX(MagicWandSize) AS  LongestMagicWand FROM WizzardDeposits 

--For wizards in each deposit group show the longest magic wand. Rename the new column appropriately.
--DepositGroup	LongestMagicWand
SELECT DepositGroup, MAX(MagicWandSize) AS LongestMagicWand FROM WizzardDeposits
GROUP BY DepositGroup

--Select the two deposit groups with the lowest average wand size.
SELECT TOP(2) DepositGroup FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)

--Select all deposit groups and their total deposit sums.
--DepositGroup	TotalSum

SELECT DepositGroup, SUM(DepositAmount) AS TotalSum FROM WizzardDeposits
GROUP BY DepositGroup

--Select all deposit groups and their total deposit sums but only for the wizards who have their magic wands crafted by Ollivander family.
SELECT DepositGroup, SUM(DepositAmount) AS TotalSum FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup

--Select all deposit groups and their total deposit sums but only for the wizards who have their magic wands crafted by Ollivander family. Filter total deposit amounts lower than 150000. Order by total deposit amount in descending order.
SELECT DepositGroup, SUM(DepositAmount) AS TotalSum FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY TotalSum DESC

--Create a query that selects: Deposit group , Magic wand creator, Minimum deposit charge for each group . Select the data in ascending ordered by MagicWandCreator and DepositGroup.

SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS MinDepositCharge FROM WizzardDeposits
GROUP BY DepositGroup, MagicWandCreator
ORDER BY MagicWandCreator ASC, DepositGroup ASC

/*--Write down a query that creates 7 different groups based on their age.
Age groups should be as follows:
[0-10]
[11-20]
[21-30]
[31-40]
[41-50]
[51-60]
[61+]
The query should return
Age groups
Count of wizards in it
*/

SELECT AgeGroup, COUNT(*) AS WizardCount FROM 
		(SELECT
			CASE 
            when T.Age BETWEEN 0 AND 10 then '[0-10]'
            when T.Age BETWEEN 11 AND 20 then '[11-20]'
			when T.Age BETWEEN 21 AND 30 then '[21-30]'
			when T.Age BETWEEN 31 AND 40 then '[31-40]'
			when T.Age BETWEEN 41 AND 50 then '[41-50]'
			when T.Age BETWEEN 51 AND 60 then '[51-60]'
			when T.Age >60 then '[61+]'
        END AS AgeGroup
     FROM WizzardDeposits AS T) AS A
	 GROUP BY AgeGroup

--Write a query that returns all unique wizard first letters of their first names only if they have deposit of type Troll Chest. Order them alphabetically. Use GROUP BY for uniqueness.
SELECT LEFT(FirstName, 1) FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
GROUP BY LEFT(FirstName, 1)

--Mr. Bodrog is highly interested in profitability. He wants to know the average interest of all deposit groups split by whether the deposit has expired or not. But that’s not all. He wants you to select deposits with start date after 01/01/1985. Order the data descending by Deposit Group and ascending by Expiration Flag.
--DepositGroup	IsDepositExpired	AverageInterest

SELECT DepositGroup, IsDepositExpired , AVG(DepositInterest)
		FROM(SELECT DepositGroup, IsDepositExpired, DepositInterest FROM WizzardDeposits
WHERE DepositStartDate>'01/01/1985') AS T
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired

--Mr. Bodrog definitely likes his werewolves more than you. This is your last chance to survive! Give him some data to play his favorite game Rich Wizard, Poor Wizard. The rules are simple: You compare the deposits of every wizard with the wizard after him. If a wizard is the last one in the database, simply ignore it. In the end you have to sum the difference between the deposits.
--Host Wizard	Host Wizard Deposit	Guest Wizard	Guest Wizard Deposit	Difference

SELECT SUM(A.[Host Wizard Deposit]-A.[Guest Wizard Deposit]) FROM	
		(SELECT T.[Host Wizard], T.[Host Wizard Deposit], LEAD(T.[Host Wizard]) OVER (ORDER BY T.Id) AS [Guest Wizard], LEAD(T.[Host Wizard Deposit]) OVER (ORDER BY T.Id) AS [Guest Wizard Deposit] FROM 
			(SELECT Id, FirstName AS [Host Wizard], DepositAmount AS [Host Wizard Deposit] FROM WizzardDeposits
			) AS T
		) AS A
	


