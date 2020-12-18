--Part 1
Id = INT, Name = LONGTEXT, EmployerId = INT.
--Part 2
select * from employers where (location = 'St. Louis');

--Part 3

SELECT Name, Description FROM techjobs.Skills inner join techjobs.JobSkills on (JobSkills.SkillId = Skills.Id)order by Name ASC;