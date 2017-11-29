

create view GenreContentRatingView
as
select ContentRating, Genre, Title, Runtime
from Project.Movies as m join Project.MovieGenre as mg
		on m.MovieID = mg.MovieID
	join Project.Genres as g 
		on g.GenreID = mg.GenreID
where GenreName = 'Comedy' and ContentRating = 'PG-13' and Runtime between 60 and 120
order by Runtime, Title


create view DirectorRatingView
as
select DirectorName, Title, IMDBRating
from Project.Directors as d join Project.MovieDirectors as md
		on d.DirectorID = md.DirectorID
	join Project.Movies as m
		on md.MovieID = m.MovieID
	join Project.Ratings as r
		on m.MovieID = r.MovieID
where IMDBRating > 7.0
order by IMDBRating, Title