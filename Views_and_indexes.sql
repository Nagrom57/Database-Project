create view GenreContentRatingView
as
select ContentRating, GenreName, Title, Runtime
from Project.Movies as m join Project.MovieGenre as mg
		on m.MovieID = mg.MovieID
	join Project.Genres as g 
		on g.GenreID = mg.GenreID
where GenreName = 'Comedy' and ContentRating = 'PG-13' and Runtime between 60 and 120


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


create view MovieAndDirector
as
select project.Movies.Title, project.Directors.DirectorName
from project.Movies join project.MovieDirectors
	on project.Movies.MovieID = project.MovieDirectors.MovieID
join project.Directors
	on project.MovieDirectors.DirectorID = project.Directors.DirectorID
	
	
	create index dir on project.MovieDirectors (DirectorId)
	create index movCast on project.Cast (ActorID)
	create index movLangCast on project.MovieLanguages (LanguageId)
	create index movGenre on project.MovieGenre (GenreID)
	create index movWriters on project.MovieWriters (Writerid)