--drop table Project.Trailer
--drop table Project.Cast
--drop table Project.MovieWriters
--drop table Project.MovieLanguages
--drop table Project.MovieDirectors
--drop table Project.MovieGenre
--drop table Project.Genres
--drop table Project.Languages
--drop table Project.Ratings
--drop table Project.Directors
--drop table Project.Actors
--drop table Project.Writers
--drop table Project.Movies

Create Table Project.Movies (
	MovieID		int				Primary Key Identity
	, Title			varchar(100)	not null
	, ContentRating	varchar(16)
	, ReleaseDate	date
	, Runtime		int				null
)

create table Project.Trailer (
	TrailerID			int			Primary Key Identity
	, MovieID			int			Foreign Key
			References Project.Movies (MovieID)
	, TrailerSequence	int	not null
	, URL				varchar(600)
)

create table Project.Languages (
	LanguageID		int				Primary Key Identity
	, Language		varchar(15)		not null
)

create table Project.MovieLanguages (
	MovieLanguageID		int		Primary Key Identity
	, LanguageID		int		Foreign Key
			References Project.Languages (LanguageID)
	, MovieID			int			Foreign Key
		References Project.Movies (MovieID)
)

create table Project.Ratings (
	RatingsID			int		Primary Key Identity
	, NumberOfRatings	int
	, IMDBRating		float
	, MovieID			int		Foreign Key
			References Project.Movies (MovieID)
)

create table Project.Actors (
	ActorID		int			Primary Key Identity
	, Name		varchar(50)	not null
)

create table Project.Cast (
	CastID				int		Primary Key Identity
	, ActorID			int		Foreign Key
			references Project.Actors (ActorID)
	, MovieID			int		Foreign Key
			references Project.Movies (MovieID)
	, CharacterName		varchar(150)	
)

create table Project.Writers (
	WriterID		int			Primary Key Identity
	, WriterName	varchar(50)	not null
)

create table Project.MovieWriters (
	MovieWriterID	int			Primary Key Identity
	, WriterID		int			Foreign Key
			References Project.Writers (WriterID)
	, MovieID		int			Foreign Key
			References Project.Movies (MovieID)
)

create table Project.Directors (
	DirectorID	int		Primary Key Identity
	, DirectorName	varchar(50)
)
create table Project.MovieDirectors (
	MovieDirectorsID		int		Primary Key Identity
	, DirectorID			int		Foreign Key
			References Project.Directors (DirectorID)
	, MovieID		int			Foreign Key
		References Project.Movies (MovieID)
)

create table Project.Genres (
	GenreID			int			Primary Key Identity
	, GenreName		varchar(25)	not null
)

create table Project.MovieGenre (
	MovieGenreID	int			Primary Key Identity
	, GenreID		int			Foreign Key
			References Project.Genres (GenreID)
	, MovieID		int			Foreign Key
			References Project.Movies (MovieID)
)
