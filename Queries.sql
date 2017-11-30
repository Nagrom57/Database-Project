--Movies with a genre of comedy
select Project.Movies.Title, Project.Genres.GenreName
from Project.Movies join Project.MovieGenre
	on Project.Movies.MovieID = Project.MovieGenre.MovieID
join Project.Genres
	on Project.MovieGenre.GenreID = Project.Genres.GenreID
where Project.Genres.GenreName = 'Comedy'


--Movies with theie corrosponding language 
select Project.Movies.MovieID, Title, Project.Languages.LanguageID, Language
from Project.Movies join Project.MovieLanguages
	on Project.Movies.MovieID = Project.MovieLanguages.MovieID
join Project.Languages
	on Project.Languages.LanguageID = Project.MovieLanguages.LanguageID


-- Movies with their ratings with best rated movies first
select project.Movies.Title, project.Ratings.NumberOfRatings, project.Ratings.IMDBRating
from project.Movies join project.Ratings
	on project.Movies.MovieID = Project.Ratings.MovieID
order by project.Ratings.IMDBRating desc


-- Movies with their director is desc order
select project.Movies.Title, project.Directors.DirectorName
from project.Movies join project.MovieDirectors
	on project.Movies.MovieID = project.MovieDirectors.MovieID
join project.Directors
	on project.MovieDirectors.DirectorID = project.Directors.DirectorID
order by Project.Directors.DirectorName desc


--Average rating for each genre of movies, from best to worst
select project.Genres.GenreName, avg(project.Ratings.IMDBRating) as RatingAverage
from project.movies join project.Ratings
	on project.Ratings.MovieID = project.Movies.MovieID
join project.MovieGenre
	on project.Movies.MovieID = project.MovieGenre.MovieID
join project.Genres
	on project.MovieGenre.GenreID = project.Genres.GenreID
group by project.Genres.GenreName
order by RatingAverage desc